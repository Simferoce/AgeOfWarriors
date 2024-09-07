using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace StatisticCodeGenerator
{
    public class AttributeSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> ClassesWithAttribute { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
                HasAttribute(classDeclarationSyntax, SourceGenerator.ClassAttributeName))
            {
                ClassesWithAttribute.Add(classDeclarationSyntax);
            }
        }

        private bool HasAttribute(ClassDeclarationSyntax classDeclarationSyntax, string attributeName)
        {
            return classDeclarationSyntax.AttributeLists
                .SelectMany(x => x.Attributes)
                .Any(x => x.Name.ToString() == attributeName || x.Name.ToString() == attributeName + "Attribute");
        }
    }

    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public static string ClassAttributeName = "StatisticClass";
        public static string PropertyAttributeName = "Statistic";
        public static string ProviderInterfaceName = "IStatisticProvider";
        private static string currentLoggingPath;

        public static readonly DiagnosticDescriptor NonPartialClassWarning = new DiagnosticDescriptor(
               id: "STATISTICODEGEN001",
               title: "Class should be partial",
               messageFormat: "Class '{0}' contains the '{1}' attribute but is not declared as partial.",
               category: "Usage",
               defaultSeverity: DiagnosticSeverity.Warning,
               isEnabledByDefault: true
           );

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            InitializeLogging(context);

            try
            {
                if (!(context.SyntaxReceiver is AttributeSyntaxReceiver receiver))
                    return;

                foreach (ClassDeclarationSyntax classDeclaration in receiver.ClassesWithAttribute)
                {
                    ProcessClassDeclaration(context, classDeclaration);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
            }
        }

        private void InitializeLogging(GeneratorExecutionContext context)
        {
            currentLoggingPath = Path.Combine(Path.GetTempPath(), $"StatisticGeneratorCodeLog_{context.Compilation.AssemblyName}.txt");
            Logger.Initialize(currentLoggingPath, true);
        }

        private void ProcessClassDeclaration(GeneratorExecutionContext context, ClassDeclarationSyntax classDeclaration)
        {
            SemanticModel semanticModel = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            INamedTypeSymbol symbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

            if (symbol == null)
                return;

            if (!IsPartial(classDeclaration))
            {
                ReportNonPartialClassWarning(context, classDeclaration, symbol);
                return;
            }

            string className = symbol.Name;
            string namespaceName = GetNamespaceName(symbol);

            bool isChild = IsChildOfClassWithAttribute(classDeclaration, semanticModel);

            StringBuilder stringBuilder = new StringBuilder();
            AppendHeader(stringBuilder);
            AppendNamespace(stringBuilder, namespaceName);
            AppendClassDefinition(stringBuilder, className, isChild);
            AppendTryGetStatisticMethod(stringBuilder, classDeclaration, semanticModel, isChild);
            AppendNameMethod(stringBuilder, classDeclaration, semanticModel, isChild);
            AppendFooter(stringBuilder, namespaceName);

            string newMethodCode = stringBuilder.ToString();
            Logger.Log($"Success: {className}_Generated.cs");
            context.AddSource($"{className}_Generated.cs", newMethodCode);
        }

        private void AppendHeader(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"// Generated at {DateTime.Now}");
            stringBuilder.AppendLine("using UnityEngine;");
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using Game;");
            stringBuilder.AppendLine();
        }

        private void AppendNamespace(StringBuilder stringBuilder, string namespaceName)
        {
            if (!string.IsNullOrEmpty(namespaceName))
            {
                stringBuilder.AppendLine($"namespace {namespaceName}");
                stringBuilder.AppendLine("{");
            }
        }

        private void AppendClassDefinition(StringBuilder stringBuilder, string className, bool isChild)
        {
            string classInterface = !isChild ? $" : {ProviderInterfaceName}" : "";

            stringBuilder.AppendLine($"    public partial class {className}{classInterface}");
            stringBuilder.AppendLine("    {");
        }

        private void AppendTryGetStatisticMethod(StringBuilder stringBuilder, ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, bool isChild)
        {
            List<PropertyDeclarationSyntax> propertyDeclarations = GetAllStatisticProperties(classDeclaration, semanticModel);

            string methodModifier = isChild ? "override" : "virtual";

            stringBuilder.AppendLine($"        public {methodModifier} bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic)");
            stringBuilder.AppendLine("        {");

            for (int i = 0; i < propertyDeclarations.Count; i++)
            {
                PropertyDeclarationSyntax propertyDeclaration = propertyDeclarations[i];
                AddStatistic(stringBuilder, propertyDeclaration, semanticModel, i == 0);
            }

            if (propertyDeclarations.Count > 0)
                stringBuilder.AppendLine();

            if (isChild)
            {
                stringBuilder.AppendLine("            return base.TryGetStatistic<T>(path, out statistic);");
            }
            else
            {
                stringBuilder.AppendLine("            statistic = default;");
                stringBuilder.AppendLine("            return false;");
            }

            stringBuilder.AppendLine("        }");
        }

        private void AppendNameMethod(StringBuilder stringBuilder, ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, bool isChild)
        {
            AttributeSyntax attributeSyntax = classDeclaration.AttributeLists.SelectMany(x => x.Attributes).FirstOrDefault(x => x.Name.ToString() == ClassAttributeName);
            INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            ImmutableArray<AttributeData> attributeDatas = classSymbol.GetAttributes();
            AttributeData attributeData = attributeDatas.FirstOrDefault(x => x.ApplicationSyntaxReference.GetSyntax().Equals(attributeSyntax));

            string methodModifier = isChild ? "override" : "virtual";

            string attributeNameValue = GetConstructorArgumentValue(attributeData, 0);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"        public {methodModifier} bool IsName(string name)");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine($"            if(name == \"{attributeNameValue}\")");
            stringBuilder.AppendLine("                return true;");
            stringBuilder.AppendLine();
            if (isChild)
                stringBuilder.AppendLine("            return base.IsName(name);");
            else
                stringBuilder.AppendLine("            return false;");
            stringBuilder.AppendLine("        }");
        }

        private void AppendFooter(StringBuilder stringBuilder, string namespaceName)
        {
            stringBuilder.AppendLine("    }");

            if (!string.IsNullOrEmpty(namespaceName))
            {
                stringBuilder.AppendLine("}");
            }
        }

        private void AddStatistic(StringBuilder stringBuilder, PropertyDeclarationSyntax propertyDeclarationSyntax, SemanticModel semanticModel, bool isFirst)
        {
            AttributeSyntax attributeSyntax = GetAttributeSyntax(propertyDeclarationSyntax, PropertyAttributeName);

            IPropertySymbol propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclarationSyntax);
            ImmutableArray<AttributeData> attributeDatas = propertySymbol.GetAttributes();
            AttributeData attributeData = attributeDatas.FirstOrDefault(attr => attr.ApplicationSyntaxReference.GetSyntax().Equals(attributeSyntax));

            string statisticName = GetConstructorArgumentValue(attributeData, 0);
            string propertyName = propertyDeclarationSyntax.Identifier.Text;

            string conditionalToken = isFirst ? "if" : "else if";
            stringBuilder.AppendLine($"            {conditionalToken}(path.SequenceEqual(\"{statisticName}\"))");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                statistic = StatisticUtility.ConvertGeneric<T, {propertySymbol.Type.ToDisplayString()}>({propertyName});");
            stringBuilder.AppendLine("                return true;");
            stringBuilder.AppendLine("            }");
        }

        private bool IsPartial(ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);
        }

        private void ReportNonPartialClassWarning(GeneratorExecutionContext context, ClassDeclarationSyntax classDeclaration, INamedTypeSymbol symbol)
        {
            Diagnostic diagnostic = Diagnostic.Create(
                NonPartialClassWarning,
                classDeclaration.Identifier.GetLocation(),
                new[] { symbol.Name, ClassAttributeName }
            );

            context.ReportDiagnostic(diagnostic);
        }

        private string GetNamespaceName(INamedTypeSymbol symbol)
        {
            return symbol.ContainingNamespace.IsGlobalNamespace
                ? null
                : symbol.ContainingNamespace.ToDisplayString();
        }

        private AttributeSyntax GetAttributeSyntax(PropertyDeclarationSyntax propertyDeclarationSyntax, string attributeName)
        {
            return propertyDeclarationSyntax.AttributeLists
                .SelectMany(x => x.Attributes)
                .FirstOrDefault(x => x.Name.ToString() == attributeName);
        }

        private string GetConstructorArgumentValue(AttributeData attributeData, int argumentIndex)
        {
            if (attributeData.ConstructorArguments.Length > argumentIndex)
            {
                TypedConstant argument = attributeData.ConstructorArguments[argumentIndex];
                return argument.Value?.ToString();
            }

            return null;
        }

        private bool IsChildOfClassWithAttribute(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
        {
            INamedTypeSymbol namedTypeSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            INamedTypeSymbol parent = namedTypeSymbol.BaseType;

            while (parent != null)
            {
                if (parent.GetAttributes().Any(x => x.AttributeClass.Name == ClassAttributeName || x.AttributeClass.Name == ClassAttributeName + "Attribute"))
                {
                    return true;
                }

                parent = parent.BaseType;
            }

            return false;
        }

        private List<PropertyDeclarationSyntax> GetAllStatisticProperties(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
        {
            return classDeclaration.Members
                .OfType<PropertyDeclarationSyntax>()
                .Where(property => HasAttribute(property, PropertyAttributeName))
                .ToList();
        }

        private bool HasAttribute(MemberDeclarationSyntax memberDeclarationSyntax, string attributeName)
        {
            return memberDeclarationSyntax.AttributeLists
                .SelectMany(x => x.Attributes)
                .Any(y => y.Name.ToString() == attributeName);
        }
    }
}

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
            try
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
            catch (Exception e)
            {
                Logger.Log(e.ToString());
            }

        }

        private void AppendHeader(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"// Generated at {DateTime.Now}");
            stringBuilder.AppendLine("using UnityEngine;");
            stringBuilder.AppendLine("using UnityEngine.Pool;");
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.Collections.Generic;");
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

            stringBuilder.AppendLine($"        public {methodModifier} bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic, HashSet<object> visited = null)");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            bool found = false;");
            stringBuilder.AppendLine("            statistic = default;");

            bool addedAtLeastOneStatistic = false;
            for (int i = 0; i < propertyDeclarations.Count; i++)
            {
                PropertyDeclarationSyntax propertyDeclaration = propertyDeclarations[i];
                addedAtLeastOneStatistic |= AddStatistic(stringBuilder, propertyDeclaration, semanticModel, i == 0);
            }

            if (isChild)
            {
                if (addedAtLeastOneStatistic)
                {
                    stringBuilder.AppendLine("            else");
                    stringBuilder.AppendLine("            {");

                    bool addedAtLeastOneElement = false;
                    for (int i = 0; i < propertyDeclarations.Count; i++)
                    {
                        PropertyDeclarationSyntax propertyDeclaration = propertyDeclarations[i];

                        if (AddStatisticProvider(stringBuilder, propertyDeclaration, semanticModel, !addedAtLeastOneElement))
                            addedAtLeastOneElement = true;
                    }

                    stringBuilder.AppendLine("                found = base.TryGetStatistic<T>(path, out statistic, visited);");
                    stringBuilder.AppendLine("            }");
                }
                else
                {
                    bool addedAtLeastOneElement = false;
                    for (int i = 0; i < propertyDeclarations.Count; i++)
                    {
                        PropertyDeclarationSyntax propertyDeclaration = propertyDeclarations[i];

                        if (AddStatisticProvider(stringBuilder, propertyDeclaration, semanticModel, !addedAtLeastOneElement))
                            addedAtLeastOneElement = true;
                    }

                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("            if(!found)");
                    stringBuilder.AppendLine("                found = base.TryGetStatistic<T>(path, out statistic, visited);");
                }
            }
            else
            {
                if (addedAtLeastOneStatistic)
                {
                    stringBuilder.AppendLine("            else");
                    stringBuilder.AppendLine("            {");
                    bool addedAtLeastOneElement = false;
                    for (int i = 0; i < propertyDeclarations.Count; i++)
                    {
                        PropertyDeclarationSyntax propertyDeclaration = propertyDeclarations[i];

                        if (AddStatisticProvider(stringBuilder, propertyDeclaration, semanticModel, !addedAtLeastOneElement))
                            addedAtLeastOneElement = true;
                    }

                    stringBuilder.AppendLine("            }");
                }
                else
                {
                    bool addedAtLeastOneElement = false;
                    for (int i = 0; i < propertyDeclarations.Count; i++)
                    {
                        PropertyDeclarationSyntax propertyDeclaration = propertyDeclarations[i];

                        if (AddStatisticProvider(stringBuilder, propertyDeclaration, semanticModel, !addedAtLeastOneElement))
                            addedAtLeastOneElement = true;
                    }

                    stringBuilder.AppendLine();
                }
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("            return found;");
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

        private bool AddStatistic(StringBuilder stringBuilder, PropertyDeclarationSyntax propertyDeclarationSyntax, SemanticModel semanticModel, bool isFirst)
        {
            AttributeSyntax attributeSyntax = GetAttributeSyntax(propertyDeclarationSyntax, PropertyAttributeName);

            IPropertySymbol propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclarationSyntax);
            ImmutableArray<AttributeData> attributeDatas = propertySymbol.GetAttributes();
            AttributeData attributeData = attributeDatas.FirstOrDefault(attr => attr.ApplicationSyntaxReference.GetSyntax().Equals(attributeSyntax));

            string statisticName = GetConstructorArgumentValue(attributeData, 0);
            string propertyName = propertyDeclarationSyntax.Identifier.Text;

            if (statisticName == "")
                return false;

            string conditionalToken = isFirst ? "if" : "else if";
            stringBuilder.AppendLine($"            {conditionalToken}(path.SequenceEqual(\"{statisticName}\"))");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                statistic = StatisticUtility.ConvertGeneric<T, {propertySymbol.Type.ToDisplayString()}>({propertyName});");
            stringBuilder.AppendLine("                found = true;");
            stringBuilder.AppendLine("            }");

            return true;
        }

        private bool AddStatisticProvider(StringBuilder stringBuilder, PropertyDeclarationSyntax propertyDeclarationSyntax, SemanticModel semanticModel, bool firstElement)
        {
            AttributeSyntax attributeSyntax = GetAttributeSyntax(propertyDeclarationSyntax, PropertyAttributeName);

            IPropertySymbol propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclarationSyntax);
            ImmutableArray<AttributeData> attributeDatas = propertySymbol.GetAttributes();
            AttributeData attributeData = attributeDatas.FirstOrDefault(attr => attr.ApplicationSyntaxReference.GetSyntax().Equals(attributeSyntax));

            string statisticName = GetConstructorArgumentValue(attributeData, 0);
            string propertyName = propertyDeclarationSyntax.Identifier.Text;

            bool statisticProviderProperty = propertySymbol.Type.Name == ProviderInterfaceName || HasAttribute(propertySymbol.Type) || IsChildOfClassWithAttribute(propertySymbol.Type);
            bool isEnumerableOfProvider = IsEnumerableOfProviderInterface(propertySymbol.Type, ProviderInterfaceName);

            if (isEnumerableOfProvider)
            {
                if (!firstElement)
                    stringBuilder.AppendLine();

                string extraCondition = statisticName == "" ? $" && (!visited.Contains({propertyName}) || visited == null)" : $" && path.StartsWith(\"{statisticName}\")";
                stringBuilder.AppendLine($"                if(!found{extraCondition})");
                stringBuilder.AppendLine("                {");
                if (statisticName == "")
                {
                    stringBuilder.AppendLine("                    bool ownsVisitedPool = false;");
                    stringBuilder.AppendLine("                    if (visited == null)");
                    stringBuilder.AppendLine("                    {");
                    stringBuilder.AppendLine("                        visited = HashSetPool<object>.Get();");
                    stringBuilder.AppendLine("                        ownsVisitedPool = true;");
                    stringBuilder.AppendLine("                    }");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("                    visited.Add(this);");
                }

                stringBuilder.AppendLine();

                if (statisticName != "")
                {
                    stringBuilder.AppendLine($"                    ReadOnlySpan<char> slicedPath = path.Slice(\"{statisticName}\".Length + 1);");
                    stringBuilder.AppendLine();
                }

                string path = statisticName != "" ? "slicedPath" : "path";
                string pool = statisticName != "" ? "null" : "visited";

                stringBuilder.AppendLine($"                    foreach (IStatisticProvider provider in {propertyName})");
                stringBuilder.AppendLine("                    {");
                stringBuilder.AppendLine($"                        found = provider.TryGetStatistic({path}, out statistic, {pool});");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("                        if(found)");
                stringBuilder.AppendLine("                            break;");
                stringBuilder.AppendLine("                    }");
                if (statisticName == "")
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("                    if (ownsVisitedPool)");
                    stringBuilder.AppendLine("                        HashSetPool<object>.Release(visited);");
                }

                stringBuilder.AppendLine("                }");

                return true;
            }
            else if (statisticProviderProperty)
            {
                if (!firstElement)
                    stringBuilder.AppendLine();

                string extraCondition = statisticName == "" ? $" && (!visited.Contains({propertyName}) || visited == null)" : $" && path.StartsWith(\"{statisticName}\")";
                stringBuilder.AppendLine($"                if(!found{extraCondition})");
                stringBuilder.AppendLine("                {");
                if (statisticName == "")
                {
                    stringBuilder.AppendLine("                    bool ownsVisitedPool = false;");
                    stringBuilder.AppendLine("                    if (visited == null)");
                    stringBuilder.AppendLine("                    {");
                    stringBuilder.AppendLine("                        visited = HashSetPool<object>.Get();");
                    stringBuilder.AppendLine("                        ownsVisitedPool = true;");
                    stringBuilder.AppendLine("                    }");
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("                    visited.Add(this);");
                    stringBuilder.AppendLine();
                }

                if (statisticName != "")
                {
                    stringBuilder.AppendLine($"                    ReadOnlySpan<char> slicedPath = path.Slice(\"{statisticName}\".Length + 1);");
                    stringBuilder.AppendLine();
                }

                string path = statisticName != "" ? "slicedPath" : "path";
                string pool = statisticName != "" ? "null" : "visited";

                stringBuilder.AppendLine($"                    found = {propertyName}.TryGetStatistic({path}, out statistic, {pool});");

                if (statisticName == "")
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("                    if (ownsVisitedPool)");
                    stringBuilder.AppendLine("                        HashSetPool<object>.Release(visited);");
                }

                stringBuilder.AppendLine("                }");

                return true;
            }

            return false;
        }

        private bool IsEnumerableOfProviderInterface(ITypeSymbol typeSymbol, string providerInterfaceName)
        {
            return ImplementsIEnumerableOfProvider(typeSymbol, providerInterfaceName);
        }

        private bool ImplementsIEnumerableOfProvider(ITypeSymbol typeSymbol, string providerInterfaceName)
        {
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                foreach (var iface in namedTypeSymbol.AllInterfaces)
                {
                    if (iface.Name == "IEnumerable" && iface.TypeArguments.Length == 1)
                    {
                        var typeArgument = iface.TypeArguments[0];
                        if (InheritsOrImplements(typeArgument, providerInterfaceName))
                        {
                            return true;
                        }
                    }
                }
            }

            var baseType = typeSymbol.BaseType;
            while (baseType != null)
            {
                if (ImplementsIEnumerableOfProvider(baseType, providerInterfaceName))
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }

        private bool InheritsOrImplements(ITypeSymbol typeSymbol, string interfaceName)
        {
            if (typeSymbol.Name == interfaceName)
                return true;

            foreach (var iface in typeSymbol.AllInterfaces)
            {
                if (iface.Name == interfaceName)
                    return true;
            }

            var baseType = typeSymbol.BaseType;
            while (baseType != null)
            {
                if (baseType.Name == interfaceName || baseType.AllInterfaces.Any(i => i.Name == interfaceName))
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
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
            return IsChildOfClassWithAttribute(namedTypeSymbol);
        }

        private bool IsChildOfClassWithAttribute(ITypeSymbol typeSymbol)
        {
            Logger.Log($"IsChildOfClassWithAttribute: {typeSymbol.Name}");
            ITypeSymbol parent = typeSymbol.BaseType;
            while (parent != null)
            {
                Logger.Log($"Parent: {parent.Name} - {string.Join(", ", parent.GetAttributes().Select(x => x.AttributeClass.Name))}");
                if (HasAttribute(parent))
                {
                    return true;
                }

                parent = parent.BaseType;
            }

            return false;
        }

        private bool HasAttribute(ITypeSymbol parent)
        {
            return parent.GetAttributes().Any(x => x.AttributeClass.Name == ClassAttributeName || x.AttributeClass.Name == ClassAttributeName + "Attribute");
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

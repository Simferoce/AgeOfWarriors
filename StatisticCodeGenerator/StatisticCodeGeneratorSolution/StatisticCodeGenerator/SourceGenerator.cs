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
        public List<ClassDeclarationSyntax> AllClass { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
            {
                AllClass.Add(classDeclarationSyntax);
            }
        }
    }

    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public class StatisticProperty
        {
            public IPropertySymbol PropertySymbol { get; set; }
            public bool AppendStatisticClassName { get; set; }
            public string PropertyName { get; set; }
            public string StatisticName { get; set; }
        }

        public class StatisticGathering
        {
            public INamedTypeSymbol NamedTypeSymbol { get; set; }
            public List<ClassDeclarationSyntax> ClassDeclarationSyntaxes { get; set; } = new List<ClassDeclarationSyntax>();
            public List<StatisticProperty> Properties { get; set; } = new List<StatisticProperty>();
            public string StatisticClassName { get; set; }
            public bool IsChild { get; set; }
        }

        public static string ClassAttributeName = "StatisticClass";
        public static string PropertyAttributeName = "Statistic";
        public static string ProviderInterfaceName = "IStatisticProvider";
        private static string currentLoggingPath;

        public static readonly DiagnosticDescriptor NonPartialClassWarning = new DiagnosticDescriptor(
               id: "STATISTICODEGEN001",
               title: "Class should be partial",
               messageFormat: "Class '{0}' contains the '{1}' attribute or has properties mark as '{2}' but is not declared as partial.",
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

                List<StatisticGathering> statisticGatherings = new List<StatisticGathering>();
                foreach (ClassDeclarationSyntax classDeclaration in receiver.AllClass)
                {
                    SemanticModel semanticModel = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);

                    if (HasAttribute(classDeclaration, ClassAttributeName)
                        || IsChildOfClassWithAttribute(classDeclaration, semanticModel))
                    {
                        INamedTypeSymbol namedTypeSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                        List<PropertyDeclarationSyntax> propertyDeclarationSyntaxes = GetAllStatisticProperties(classDeclaration, semanticModel);
                        if (propertyDeclarationSyntaxes.Count == 0 && !HasAttribute(classDeclaration, ClassAttributeName))
                            continue;

                        if (!IsPartial(classDeclaration))
                        {
                            ReportNonPartialClassWarning(context, classDeclaration, namedTypeSymbol);
                            continue;
                        }

                        StatisticGathering statisticGathering = statisticGatherings.FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.NamedTypeSymbol, namedTypeSymbol));
                        if (statisticGathering == null)
                        {
                            statisticGathering = new StatisticGathering();
                            statisticGathering.NamedTypeSymbol = namedTypeSymbol;

                            statisticGatherings.Add(statisticGathering);
                        }

                        statisticGathering.ClassDeclarationSyntaxes.Add(classDeclaration);
                        List<StatisticProperty> statisticProperties = new List<StatisticProperty>();
                        foreach (PropertyDeclarationSyntax propertyDeclarationSyntaxe in propertyDeclarationSyntaxes)
                        {
                            GetStatisticPropertyData(propertyDeclarationSyntaxe, semanticModel, out IPropertySymbol propertySymbol, out string statisticName, out bool appendStatisticClassName, out string propertyName);
                            StatisticProperty statisticProperty = new StatisticProperty()
                            {
                                PropertySymbol = propertySymbol,
                                PropertyName = propertyName,
                                StatisticName = statisticName,
                                AppendStatisticClassName = appendStatisticClassName,
                            };

                            statisticProperties.Add(statisticProperty);
                        }
                        statisticGathering.Properties.AddRange(statisticProperties);

                        if (HasAttribute(classDeclaration, ClassAttributeName))
                            statisticGathering.StatisticClassName = GetStatisticClassName(classDeclaration, semanticModel);

                        if (IsChildOfClassWithAttribute(classDeclaration, semanticModel))
                            statisticGathering.IsChild = true;
                    }
                }

                foreach (StatisticGathering statisticGathering in statisticGatherings)
                {
                    ProcessClassDeclaration(context, statisticGathering);
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

        private void ProcessClassDeclaration(GeneratorExecutionContext context, StatisticGathering statisticGathering)
        {

            SymbolDisplayFormat classNameFormat = new SymbolDisplayFormat(genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters);
            string className = statisticGathering.NamedTypeSymbol.ToDisplayString(classNameFormat);
            Logger.Log($"Process: {className}");

            string namespaceName = GetNamespaceName(statisticGathering.NamedTypeSymbol);

            StringBuilder stringBuilder = new StringBuilder();
            AppendHeader(stringBuilder);
            AppendNamespace(stringBuilder, namespaceName);

            AppendClassDefinition(stringBuilder, className, statisticGathering.IsChild);
            if (!string.IsNullOrEmpty(statisticGathering.StatisticClassName))
            {
                AppendNameMethod(stringBuilder, statisticGathering.StatisticClassName, statisticGathering.IsChild);
                stringBuilder.AppendLine();
            }
            AppendTryGetStatisticMethod(stringBuilder, statisticGathering.Properties, statisticGathering.IsChild);
            stringBuilder.AppendLine();
            AppendProviderStatistic(stringBuilder, statisticGathering.Properties, statisticGathering.IsChild);
            AppendFooter(stringBuilder, namespaceName);

            Logger.Log($"Is Generic: {statisticGathering.NamedTypeSymbol.IsGenericType}");
            Logger.Log($"Arity: {statisticGathering.NamedTypeSymbol.Arity}");
            Logger.Log($"TypeParameters count: {statisticGathering.NamedTypeSymbol.TypeParameters.Count()}");

            SymbolDisplayFormat format = new SymbolDisplayFormat(
               genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
               typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

            string fileName = $"{statisticGathering.NamedTypeSymbol.ToDisplayString(format)}_Generated.cs";
            fileName = fileName
                .Replace('<', '_')
                .Replace('>', '_')
                .Replace(',', '_')
                .Replace('.', '_')
                .Replace(" ", string.Empty);

            string newMethodCode = stringBuilder.ToString();
            Logger.Log($"Success: {fileName}");
            context.AddSource(fileName, newMethodCode);
        }

        private void AppendHeader(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"// Generated at {DateTime.Now}");
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using System.Collections.Generic;");
            stringBuilder.AppendLine("using UnityEngine;");
            stringBuilder.AppendLine("using Game;");
            stringBuilder.AppendLine("using static IStatisticProvider;");
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

        private void AppendTryGetStatisticMethod(StringBuilder stringBuilder, List<StatisticProperty> statisticProperties, bool isChild)
        {
            string methodModifier = isChild ? "override" : "virtual";

            stringBuilder.AppendLine($"        public {methodModifier} bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic)");
            stringBuilder.AppendLine("        {");

            bool hasAddedAStatistic = false;
            for (int i = 0; i < statisticProperties.Count; i++)
            {
                StatisticProperty statisticProperty = statisticProperties[i];
                hasAddedAStatistic = AddStatistic(stringBuilder, statisticProperty, !hasAddedAStatistic);
            }

            if (hasAddedAStatistic)
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

        private void AppendProviderStatistic(StringBuilder stringBuilder, List<StatisticProperty> allProperties, bool isChild)
        {
            Logger.Log("AppendProviderStatistic");

            List<StatisticProperty> allStatisticProviderProperties = allProperties.Where(x => InheritsOrImplements(x.PropertySymbol.Type, ProviderInterfaceName)).ToList();
            List<StatisticProperty> allEnumerableOfStatisticProviderProperties = allProperties.Where(x => ImplementsIEnumerableOfProvider(x.PropertySymbol.Type, ProviderInterfaceName)).ToList();

            string methodModifier = isChild ? "override" : "virtual";

            stringBuilder.AppendLine($"        public {methodModifier} IEnumerator<StatisticProviderResolution> GetStatisticProvider()");
            stringBuilder.AppendLine("        {");

            for (int i = 0; i < allStatisticProviderProperties.Count; i++)
            {
                StatisticProperty statisticProperty = allStatisticProviderProperties[i];

                stringBuilder.AppendLine($"            yield return new StatisticProviderResolution({statisticProperty.PropertyName}, \"{statisticProperty.StatisticName}\", {statisticProperty.AppendStatisticClassName.ToString().ToLower()});");

                if (isChild || i < allStatisticProviderProperties.Count - 1 || allEnumerableOfStatisticProviderProperties.Count > 0)
                    stringBuilder.AppendLine();
            }

            for (int i = 0; i < allEnumerableOfStatisticProviderProperties.Count; i++)
            {
                StatisticProperty statisticProperty = allEnumerableOfStatisticProviderProperties[i];

                stringBuilder.AppendLine($"            foreach(IStatisticProvider provider in {statisticProperty.PropertyName})");
                stringBuilder.AppendLine($"                yield return new StatisticProviderResolution(provider, \"{statisticProperty.StatisticName}\", {statisticProperty.AppendStatisticClassName.ToString().ToLower()});");

                if (isChild || i < allEnumerableOfStatisticProviderProperties.Count - 1)
                    stringBuilder.AppendLine();
            }

            if (isChild)
            {
                stringBuilder.AppendLine($"            IEnumerator<StatisticProviderResolution> baseEnumerator = base.GetStatisticProvider();");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"            while (baseEnumerator.MoveNext())");
                stringBuilder.AppendLine($"                yield return baseEnumerator.Current;");
            }

            if (!isChild && allStatisticProviderProperties.Count == 0 && allEnumerableOfStatisticProviderProperties.Count == 0)
                stringBuilder.AppendLine($"            yield break;");

            stringBuilder.AppendLine("        }");
        }

        private void AppendNameMethod(StringBuilder stringBuilder, string attributeNameValue, bool isChild)
        {
            string methodModifier = isChild ? "override" : "virtual";

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

        private string GetStatisticClassName(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
        {
            AttributeSyntax attributeSyntax = classDeclaration.AttributeLists.SelectMany(x => x.Attributes).FirstOrDefault(x => x.Name.ToString() == ClassAttributeName);
            INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            ImmutableArray<AttributeData> attributeDatas = classSymbol.GetAttributes();
            AttributeData attributeData = attributeDatas.FirstOrDefault(x => x.ApplicationSyntaxReference.GetSyntax().Equals(attributeSyntax));
            string attributeNameValue = GetAttributeArgumentValue(attributeData, 0, "Name");
            return attributeNameValue;
        }

        private void AppendFooter(StringBuilder stringBuilder, string namespaceName)
        {
            stringBuilder.AppendLine("    }");

            if (!string.IsNullOrEmpty(namespaceName))
            {
                stringBuilder.AppendLine("}");
            }
        }

        private bool AddStatistic(StringBuilder stringBuilder, StatisticProperty statisticProperty, bool isFirst)
        {
            Logger.Log("AddStatistic");

            if (string.IsNullOrEmpty(statisticProperty.StatisticName))
                return false;

            string conditionalToken = isFirst ? "if" : "else if";
            stringBuilder.AppendLine($"            {conditionalToken}(path.SequenceEqual(\"{statisticProperty.StatisticName}\"))");
            stringBuilder.AppendLine("            {");
            stringBuilder.AppendLine($"                statistic = StatisticUtility.ConvertGeneric<T, {statisticProperty.PropertySymbol.Type.ToDisplayString()}>({statisticProperty.PropertyName});");
            stringBuilder.AppendLine("                return true;");
            stringBuilder.AppendLine("            }");

            return true;
        }

        private void GetStatisticPropertyData(PropertyDeclarationSyntax propertyDeclarationSyntax, SemanticModel semanticModel, out IPropertySymbol propertySymbol, out string statisticName, out bool appendStatisticClassName, out string propertyName)
        {
            Logger.Log($"Get Property of {propertyDeclarationSyntax}.");

            AttributeSyntax attributeSyntax = GetAttributeSyntax(propertyDeclarationSyntax, PropertyAttributeName);

            propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclarationSyntax);
            ImmutableArray<AttributeData> attributeDatas = propertySymbol.GetAttributes();
            AttributeData attributeData = attributeDatas.FirstOrDefault(attr => attr.ApplicationSyntaxReference.GetSyntax().Equals(attributeSyntax));

            Logger.Log($"GetAttributeArgumentValue for \"Name\".");
            statisticName = GetAttributeArgumentValue(attributeData, 0, "Name");
            string appendStatisticClassNameValue = GetAttributeArgumentValue(attributeData, 1, "AppendStatisticClassName");
            appendStatisticClassName = !string.IsNullOrEmpty(appendStatisticClassNameValue) ? bool.Parse(appendStatisticClassNameValue) : false;
            propertyName = propertyDeclarationSyntax.Identifier.Text;
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
                new[] { symbol.Name, ClassAttributeName, PropertyAttributeName }
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

        private string GetAttributeArgumentValue(AttributeData attributeData, int constructorArgumentIndex, string namedArgumentName)
        {
            string value = GetNamedArgumentValue(attributeData, namedArgumentName);
            if (value != null)
                return value;

            value = GetConstructorArgumentValue(attributeData, constructorArgumentIndex);
            if (value != null)
                return value;

            return null;
        }

        private string GetConstructorArgumentValue(AttributeData attributeData, int constructorArgumentIndex)
        {
            if (attributeData.ConstructorArguments.Length > constructorArgumentIndex)
            {
                TypedConstant constructorArgument = attributeData.ConstructorArguments[constructorArgumentIndex];
                return constructorArgument.Value?.ToString();
            }

            return null;
        }

        private string GetNamedArgumentValue(AttributeData attributeData, string namedArgumentName)
        {
            Logger.Log($"Attempt to get value of {namedArgumentName} from {attributeData}");

            foreach (var namedArgument in attributeData.NamedArguments)
            {
                if (namedArgument.Key == namedArgumentName)
                {
                    Logger.Log($"Found Value: {namedArgument.Value.Value?.ToString()}");
                    return namedArgument.Value.Value?.ToString();
                }
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
    }
}

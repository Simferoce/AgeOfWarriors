using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatisticCodeGenerator
{
    public class AttributeSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> ClassesWithAttribute { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists
                    .SelectMany(x => x.Attributes)
                    .Any(x => x.Name.ToString() == SourceGenerator.ClassAttributeName))
            {
                ClassesWithAttribute.Add(classDeclarationSyntax);
            }
        }
    }

    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public static string ClassAttributeName = "StatisticClass";

        public static readonly DiagnosticDescriptor NonPartialClassWarning = new DiagnosticDescriptor(
                id: "STATISTICODEGEN001",
                title: "Class should be partial",
                messageFormat: "Class '{0}' contains the '{1}' attribute but is not declared as partial.",
                category: "Usage",
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true
            );

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is AttributeSyntaxReceiver receiver))
                return;

            foreach (ClassDeclarationSyntax classDeclaration in receiver.ClassesWithAttribute)
            {
                INamedTypeSymbol symbol = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree).GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                if (symbol == null)
                    continue;

                bool isPartial = classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);
                if (!isPartial)
                {
                    Diagnostic diagnostic = Diagnostic.Create(NonPartialClassWarning, classDeclaration.Identifier.GetLocation(), symbol.Name, ClassAttributeName);
                    context.ReportDiagnostic(diagnostic);

                    continue;
                }

                string className = symbol.Name;
                string namespaceName = symbol.ContainingNamespace.IsGlobalNamespace
                    ? null
                    : symbol.ContainingNamespace.ToDisplayString();

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"using UnityEngine;");
                stringBuilder.AppendLine("");

                // Conditionally add namespace
                if (!string.IsNullOrEmpty(namespaceName))
                {
                    stringBuilder.AppendLine($"namespace {namespaceName}");
                    stringBuilder.AppendLine("{");
                }

                // Append partial class definition and method
                stringBuilder.AppendLine($"   partial class {className}");
                stringBuilder.AppendLine("    {");
                stringBuilder.AppendLine("        public void NewGeneratedMethod()");
                stringBuilder.AppendLine("        {");
                stringBuilder.AppendLine($"            Debug.Log(\"NewGeneratedMethod called in {className}\");");
                stringBuilder.AppendLine("        }");
                stringBuilder.AppendLine("    }");

                // Close namespace if it was added
                if (!string.IsNullOrEmpty(namespaceName))
                {
                    stringBuilder.AppendLine("}");
                }

                string newMethodCode = stringBuilder.ToString();
                context.AddSource($"{className}_Generated.cs", newMethodCode);
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new AttributeSyntaxReceiver());
        }
    }
}
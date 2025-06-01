using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIRouterGenerator
{
    [Generator]
    public class UIRouterGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ViewSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ViewSyntaxReceiver receiver)
                return;

            var compilation = context.Compilation;
            var mappings = new List<(string viewName, string presenterName)>();

            foreach (var classDecl in receiver.CandidateClasses)
            {
                var model = compilation.GetSemanticModel(classDecl.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;
                if (symbol == null)
                    continue;

                string? presenterType = null;

                var attr = symbol.GetAttributes().FirstOrDefault(attr =>
                    attr.AttributeClass?.Name == "BindPresenterAttribute");

                if (attr != null && attr.ConstructorArguments.Length == 1)
                {
                    presenterType = attr.ConstructorArguments[0].Value?.ToString();
                }
                else
                {
                    var baseType = symbol.BaseType;
                    while (baseType != null)
                    {
                        if (baseType.Name.StartsWith("BaseUIView") && baseType.IsGenericType)
                        {
                            presenterType = baseType.TypeArguments[0].ToDisplayString();
                            break;
                        }
                        baseType = baseType.BaseType;
                    }
                }

                if (presenterType != null)
                {
                    mappings.Add((symbol.ToDisplayString(), presenterType));
                }
            }

            var source = GenerateRouterCode(mappings);
            context.AddSource("UIRouterGenerated.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private string GenerateRouterCode(List<(string view, string presenter)> mappings)
        {
            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine("using LiteFramework.Core.Module.UI;");
            builder.AppendLine("namespace LiteFramework.Module.UI");
            builder.AppendLine("{");
            builder.AppendLine("    public static class UIRouterGenerated");
            builder.AppendLine("    {");

            builder.AppendLine("        public static Type GetPresenterType<TView>() where TView : IView");
            builder.AppendLine("        {");
            builder.AppendLine("            var type = typeof(TView);");
            foreach (var (view, presenter) in mappings)
            {
                builder.AppendLine($"            if (type == typeof({view})) return typeof({presenter});");
            }
            builder.AppendLine("            throw new InvalidOperationException(\"Unknown view type: \" + type.Name);");
            builder.AppendLine("        }");

            builder.AppendLine("        public static void Open<TView>(IUIManager manager, UIType type, Transform parent) where TView : IView");
            builder.AppendLine("        {");
            foreach (var (view, presenter) in mappings)
            {
                builder.AppendLine($"            if (typeof(TView) == typeof({view})) {{ manager.OpenUI<{presenter}, {view}>(type, parent); return; }}");
            }
            builder.AppendLine("            throw new InvalidOperationException(\"Unsupported view type: \" + typeof(TView).Name);");
            builder.AppendLine("        }");

            builder.AppendLine("        public static void Close<TView>(IUIManager manager, UIType type, Transform parent) where TView : IView");
            builder.AppendLine("        {");
            foreach (var (view, presenter) in mappings)
            {
                builder.AppendLine($"            if (typeof(TView) == typeof({view})) {{ manager.CloseUI<{presenter}, {view}>(type, parent); return; }}");
            }
            builder.AppendLine("            throw new InvalidOperationException(\"Unsupported view type: \" + typeof(TView).Name);");
            builder.AppendLine("        }");

            builder.AppendLine("    }");
            builder.AppendLine("}");
            return builder.ToString();
        }

        private class ViewSyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDecl &&
                    classDecl.BaseList != null &&
                    classDecl.Modifiers.Any(m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword)))
                {
                    CandidateClasses.Add(classDecl);
                }
            }
        }
    }
}
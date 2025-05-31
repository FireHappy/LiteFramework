// // Assets/Scripts/Editor/UIRouterGenerator.cs
// #if UNITY_EDITOR

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Microsoft.CodeAnalysis.Text;
// using UnityEditor;
// using UnityEngine;

// [assembly: System.Reflection.AssemblyVersion("1.0.0.0")]
// namespace LiteFramework.Module.UI
// {
//     [Generator]
//     public class UIRouterGenerator : ISourceGenerator
//     {
//         private const string BindPresenterAttribute = "LiteFramework.Core.MVP.BindPresenterAttribute";
//         private const string BaseUIView = "LiteFramework.Core.MVP.BaseUIView`1";

//         public void Initialize(GeneratorInitializationContext context)
//         {
//             // 注册语法接收器
//             context.RegisterForSyntaxNotifications(() => new ViewSyntaxReceiver());
//         }

//         public void Execute(GeneratorExecutionContext context)
//         {
//             try
//             {
//                 if (context.SyntaxReceiver is not ViewSyntaxReceiver receiver)
//                     return;

//                 var compilation = context.Compilation;
//                 var viewPresenterMap = new Dictionary<string, string>();
//                 var missingPresenters = new List<string>();

//                 foreach (var classDeclaration in receiver.ViewCandidates)
//                 {
//                     var model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
//                     var classSymbol = model.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

//                     if (classSymbol == null) continue;

//                     // 检查是否实现了 IView 接口
//                     if (!classSymbol.AllInterfaces.Any(i => i.Name == "IView"))
//                         continue;

//                     // 查找 BindPresenter 特性
//                     var presenterType = FindPresenterFromAttribute(classSymbol);
//                     if (presenterType != null)
//                     {
//                         viewPresenterMap.Add(
//                             classSymbol.ToDisplayString(),
//                             presenterType.ToString()
//                         );
//                         continue;
//                     }

//                     // 尝试从基类推导
//                     presenterType = FindPresenterFromBaseClass(classSymbol);
//                     if (presenterType != null)
//                     {
//                         viewPresenterMap.Add(
//                             classSymbol.ToDisplayString(),
//                             presenterType.ToString()
//                         );
//                         continue;
//                     }

//                     // 记录缺失 Presenter 的类型
//                     missingPresenters.Add(classSymbol.ToDisplayString());
//                 }

//                 // 生成源代码
//                 var source = GenerateRouterClass(viewPresenterMap);
//                 context.AddSource("UIRouter.Generated.cs", SourceText.From(source, Encoding.UTF8));

//                 // 输出警告信息
//                 if (missingPresenters.Count > 0)
//                 {
//                     var warningBuilder = new StringBuilder();
//                     warningBuilder.AppendLine("// UI Router Generator Warnings:");
//                     warningBuilder.AppendLine("// The following views are missing Presenter bindings:");
//                     foreach (var view in missingPresenters)
//                     {
//                         warningBuilder.AppendLine($"// - {view}");
//                     }
//                     warningBuilder.AppendLine("// Add [BindPresenter] attribute or inherit from BaseUIView<TPresenter>");

//                     context.AddSource("UIRouter.Warnings.cs", SourceText.From(warningBuilder.ToString(), Encoding.UTF8));

//                     // 在 Unity 控制台显示警告
//                     Debug.LogWarning(warningBuilder.ToString());
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"UIRouter Generator Error: {ex}");
//             }
//         }

//         private static string FindPresenterFromAttribute(INamedTypeSymbol classSymbol)
//         {
//             var attribute = classSymbol.GetAttributes()
//                 .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == BindPresenterAttribute);

//             return attribute?.ConstructorArguments[0].Value?.ToString();
//         }

//         private static string FindPresenterFromBaseClass(INamedTypeSymbol classSymbol)
//         {
//             var baseType = classSymbol.BaseType;
//             while (baseType != null)
//             {
//                 if (baseType.IsGenericType &&
//                     baseType.OriginalDefinition.ToDisplayString() == BaseUIView)
//                 {
//                     return baseType.TypeArguments[0].ToDisplayString();
//                 }
//                 baseType = baseType.BaseType;
//             }
//             return null;
//         }

//         private static string GenerateRouterClass(Dictionary<string, string> viewPresenterMap)
//         {
//             var sb = new StringBuilder();

//             sb.AppendLine("// <auto-generated/>");
//             sb.AppendLine("// Generated by UIRouterGenerator");
//             sb.AppendLine("// Unity compatible source generator for Mac/Windows");
//             sb.AppendLine();
//             sb.AppendLine("using System;");
//             sb.AppendLine("using System.Collections.Generic;");
//             sb.AppendLine("using LiteFramework.Core.Module.UI;");
//             sb.AppendLine("using UnityEngine;");
//             sb.AppendLine();
//             sb.AppendLine("namespace LiteFramework.Module.UI");
//             sb.AppendLine("{");
//             sb.AppendLine("    public static class UIRouterGenerated");
//             sb.AppendLine("    {");
//             sb.AppendLine("        private static readonly Dictionary<Type, Action<IUIManager, UIType, Transform>> OpenActions = new Dictionary<Type, Action<IUIManager, UIType, Transform>>();");
//             sb.AppendLine("        private static readonly Dictionary<Type, Action<IUIManager, UIType, Transform>> CloseActions = new Dictionary<Type, Action<IUIManager, UIType, Transform>>();");
//             sb.AppendLine("        private static bool isInitialized = false;");
//             sb.AppendLine();
//             sb.AppendLine("        public static void Initialize()");
//             sb.AppendLine("        {");
//             sb.AppendLine("            if (isInitialized) return;");
//             sb.AppendLine("            isInitialized = true;");
//             sb.AppendLine();

//             // 注册 Open 方法
//             foreach (var kvp in viewPresenterMap)
//             {
//                 sb.AppendLine($"            OpenActions[typeof({kvp.Key})] = (manager, type, parent) => {{");
//                 sb.AppendLine($"                manager.OpenUI<{kvp.Value}, {kvp.Key}>(type, parent);");
//                 sb.AppendLine("            };");
//             }

//             sb.AppendLine();

//             // 注册 Close 方法
//             foreach (var kvp in viewPresenterMap)
//             {
//                 sb.AppendLine($"            CloseActions[typeof({kvp.Key})] = (manager, type, parent) => {{");
//                 sb.AppendLine($"                manager.CloseUI<{kvp.Value}, {kvp.Key}>(type, parent);");
//                 sb.AppendLine("            };");
//             }

//             sb.AppendLine("        }");
//             sb.AppendLine();
//             sb.AppendLine("        public static void Open(Type viewType, IUIManager manager, UIType type, Transform parent)");
//             sb.AppendLine("        {");
//             sb.AppendLine("            if (!isInitialized) Initialize();");
//             sb.AppendLine();
//             sb.AppendLine("            if (OpenActions.TryGetValue(viewType, out var action))");
//             sb.AppendLine("            {");
//             sb.AppendLine("                action(manager, type, parent);");
//             sb.AppendLine("            }");
//             sb.AppendLine("            else");
//             sb.AppendLine("            {");
//             sb.AppendLine("                Debug.LogError($\"UIRouterGenerated: No Open action registered for {viewType.Name}\");");
//             sb.AppendLine("            }");
//             sb.AppendLine("        }");
//             sb.AppendLine();
//             sb.AppendLine("        public static void Close(Type viewType, IUIManager manager, UIType type, Transform parent)");
//             sb.AppendLine("        {");
//             sb.AppendLine("            if (!isInitialized) Initialize();");
//             sb.AppendLine();
//             sb.AppendLine("            if (CloseActions.TryGetValue(viewType, out var action))");
//             sb.AppendLine("            {");
//             sb.AppendLine("                action(manager, type, parent);");
//             sb.AppendLine("            }");
//             sb.AppendLine("            else");
//             sb.AppendLine("            {");
//             sb.AppendLine("                Debug.LogError($\"UIRouterGenerated: No Close action registered for {viewType.Name}\");");
//             sb.AppendLine("            }");
//             sb.AppendLine("        }");
//             sb.AppendLine("    }");
//             sb.AppendLine("}");

//             return sb.ToString();
//         }

//         private class ViewSyntaxReceiver : ISyntaxReceiver
//         {
//             public List<ClassDeclarationSyntax> ViewCandidates { get; } = new List<ClassDeclarationSyntax>();

//             public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
//             {
//                 // 收集所有可能包含 [BindPresenter] 特性或继承自 BaseUIView 的类
//                 if (syntaxNode is ClassDeclarationSyntax classDeclaration)
//                 {
//                     ViewCandidates.Add(classDeclaration);
//                 }
//             }
//         }
//     }
// }

// #endif
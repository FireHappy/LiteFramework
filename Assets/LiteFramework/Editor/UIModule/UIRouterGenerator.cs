#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using LiteFramework.Core.Module.UI;
using LiteFramework.Core.MVP;
using UnityEditor;
using UnityEngine;

namespace LiteFramework.EditorTools
{
    public static class UIRouterGeneratorEditor
    {
        public static void GenerateRouterRegister(string outputPath)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.StartsWith("Unity") && !a.FullName.StartsWith("System"));

            var viewTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IView).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("// 自动生成，请勿手动修改");
            sb.AppendLine("using LiteFramework.Core.Module.UI;");
            sb.AppendLine("using LiteFramework.Core.MVP;");
            sb.AppendLine();
            sb.AppendLine("namespace LiteFramework.Generated");
            sb.AppendLine("{");
            sb.AppendLine("    public static class UIRouterRegister");
            sb.AppendLine("    {");
            sb.AppendLine("        static Register()");
            sb.AppendLine("        {");

            foreach (var view in viewTypes)
            {
                var presenter = GetPresenterFromView(view);
                if (presenter == null)
                {
                    Debug.LogWarning($"跳过 View：{view.FullName}，未能解析 Presenter。");
                    continue;
                }

                sb.AppendLine($"            UIRouter.Register<{presenter.FullName}, {view.FullName}>();");
            }

            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
            File.WriteAllText(outputPath, sb.ToString());
            AssetDatabase.Refresh();
            Debug.Log($"✅ UIRouterRegister.cs 生成完毕，共生成 {viewTypes.Count} 条映射。");
        }

        private static Type GetPresenterFromView(Type viewType)
        {
            var attr = viewType.GetCustomAttribute<BindPresenterAttribute>();
            if (attr != null)
                return attr.PresenterType;

            var baseType = viewType.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseUIView<>))
                    return baseType.GetGenericArguments()[0];

                baseType = baseType.BaseType;
            }

            return null;
        }
    }
}
#endif



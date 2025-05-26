using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using LFramework.EditorTools;
using System;
using System.Collections.Generic;

public static class UIPrefabCodeGenerator
{
    [MenuItem("Assets/生成 UI MVP 模板", true)]
    private static bool ValidateGenerate()
    {
        var obj = Selection.activeObject;
        return obj is GameObject && AssetDatabase.GetAssetPath(obj).EndsWith(".prefab");
    }

    [MenuItem("Assets/生成 UI MVP 模板")]
    private static void GenerateUIMVP()
    {
        var go = Selection.activeObject as GameObject;
        string prefabName = go.name.Replace("View", "");

        var config = LoadUIGeneratorConfig();
        if (config == null)
        {
            Debug.LogError("找不到 UIGeneratorConfig");
            return;
        }

        // 路径
        string viewTemplatePath = Path.Combine(config.templateRootPath, "UIViewTemplate.txt");
        string presenterTemplatePath = Path.Combine(config.templateRootPath, "UIPresenterTemplate.txt");
        string viewTemplate = File.ReadAllText(viewTemplatePath);
        string presenterTemplate = File.ReadAllText(presenterTemplatePath);

        // 自动字段生成
        var fields = GenerateComponentFields(go.transform, config);

        // 内容替换
        string viewCode = viewTemplate
            .Replace("{UI_NAME}", prefabName)
            .Replace("{UI_Name}", prefabName)
            .Replace("{AutoWriteComponent}", fields);

        string presenterCode = presenterTemplate
            .Replace("{UI_NAME}", prefabName)
            .Replace("{UI_Name}", prefabName);

        // 写入文件
        string outputDir = Path.Combine(config.outputRootPath, prefabName);
        Directory.CreateDirectory(outputDir);

        File.WriteAllText(Path.Combine(outputDir, $"{prefabName}View.cs"), viewCode);
        File.WriteAllText(Path.Combine(outputDir, $"{prefabName}Presenter.cs"), presenterCode);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("生成成功", $"生成 {prefabName} 的 MVP 代码成功", "确定");
    }

    private static string GenerateComponentFields(Transform root, UIGeneratorConfig config)
    {
        StringBuilder sb = new StringBuilder();
        List<string> usedNames = new List<string>();

        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            string name = child.name;
            string[] parts = name.Split('_');
            if (parts.Length < 2) continue;

            string prefix = parts[0];
            string filedName = parts[1];

            Type type = config.GetComponentTypeFromPrefix(prefix);
            if (type == null) continue;

            var component = child.GetComponent(type);
            if (component == null) continue;

            string varName = prefix + filedName;
            filedName = prefix.ToLower() + filedName;
            if (usedNames.Contains(varName)) continue;
            usedNames.Add(varName);

            sb.AppendLine($"\t[Autowrited(\"{name.ToLower()}\")] public {type.Name} {filedName};");
        }

        return sb.ToString();
    }

    private static UIGeneratorConfig LoadUIGeneratorConfig()
    {
        string[] guids = AssetDatabase.FindAssets("t:UIGeneratorConfig");
        if (guids.Length == 0) return null;
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<UIGeneratorConfig>(path);
    }
}

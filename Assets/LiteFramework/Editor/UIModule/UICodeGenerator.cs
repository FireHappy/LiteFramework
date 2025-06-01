using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using LiteFramework.EditorTools;
using System;
using System.Collections.Generic;
using System.Data.Common;

public static class UIPrefabCodeGenerator
{
    [MenuItem("Assets/Generate UI MVP Template", true)]
    private static bool ValidateGenerate()
    {
        var obj = Selection.activeObject;
        return obj is GameObject && AssetDatabase.GetAssetPath(obj).EndsWith(".prefab");
    }

    [MenuItem("Assets/Generate UI MVP Template")]
    private static void GenerateUIMVP()
    {
        var go = Selection.activeObject as GameObject;
        string uiName = go.name.Replace("View", "");

        var config = LoadUIGeneratorConfig();
        if (config == null)
        {
            Debug.LogError("Can't Find UIGeneratorConfig");
            return;
        }

        // 路径
        string viewTemplatePath = Path.Combine(config.templateRootPath, "UIViewTemplate.txt");
        string viewAutoTemplatePath = Path.Combine(config.templateRootPath, "UIViewAutoTemplate.txt");
        string presenterTemplatePath = Path.Combine(config.templateRootPath, "UIPresenterTemplate.txt");
        string viewTemplate = File.ReadAllText(viewTemplatePath);
        string viewAutoTemplate = File.ReadAllText(viewAutoTemplatePath);
        string presenterTemplate = File.ReadAllText(presenterTemplatePath);

        // 自动字段生成
        var (fields, fieldsFind) = GenerateComponentFields(go.transform, config);

        var nameSpace = config.nameSpace;

        // 内容替换
        string viewCode = viewTemplate
            .Replace("{UI_NAME}", uiName)
            .Replace("{NAMESPACE}", nameSpace);

        string viewAutoCode = viewAutoTemplate
            .Replace("{UI_NAME}", uiName)
            .Replace("{Fields}", fields)
            .Replace("{FieldsFind}", fieldsFind)
            .Replace("{NAMESPACE}", nameSpace);

        string presenterCode = presenterTemplate
            .Replace("{UI_NAME}", uiName)
            .Replace("{NAMESPACE}", nameSpace);

        // 写入文件
        string outputDir = Path.Combine(config.outputRootPath, uiName);
        Directory.CreateDirectory(outputDir);

        //autoView ui 每次改动都会重新覆盖生成
        File.WriteAllText(Path.Combine(outputDir, $"{uiName}View.Auto.cs"), viewAutoCode);

        var viewFile = Path.Combine(outputDir, $"{uiName}View.cs");
        if (!File.Exists(viewFile))
        {
            File.WriteAllText(Path.Combine(outputDir, $"{uiName}View.cs"), viewCode);
        }

        var presenterFile = Path.Combine(outputDir, $"{uiName}Presenter.cs");
        if (!File.Exists(presenterFile))
        {
            File.WriteAllText(Path.Combine(outputDir, $"{uiName}Presenter.cs"), presenterCode);
        }

        AssetDatabase.Refresh();
        UIRouterGeneratorEditor.GenerateRouterFiles(config.outputRootPath);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Generate success!!!", $"Generate {uiName}  MVP code success.", "Sure");
    }

    private static (string, string) GenerateComponentFields(Transform root, UIGeneratorConfig config)
    {
        StringBuilder fields = new StringBuilder();
        StringBuilder fieldsFind = new StringBuilder();
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

            fields.AppendLine($"\t\tpublic {type.Name} {filedName};");
            Debug.Log("fieldName:" + filedName);
            fieldsFind.AppendLine($"\t\t\t{filedName} = transform.Find(\"{GetFindPath(root, child)}\").GetComponent<{type.Name}>();");
        }

        return (fields.ToString(), fieldsFind.ToString());
    }

    private static string GetFindPath(Transform root, Transform child)
    {
        string path = $"{child.name}";
        var parent = child.parent;
        while (parent != root)
        {
            parent = parent.parent;
            path = child.parent.name + '/' + path;
        }
        return path;
    }

    private static UIGeneratorConfig LoadUIGeneratorConfig()
    {
        string[] guids = AssetDatabase.FindAssets("t:UIGeneratorConfig");
        if (guids.Length == 0) return null;
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<UIGeneratorConfig>(path);
    }
}

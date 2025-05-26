using UnityEditor;
using UnityEngine;
using System.IO;

public class MVPCodeGeneratorWindow : EditorWindow
{
    private GameObject uiPrefab;
    private string outputFolder = "Assets/Scripts/UI/";
    private string templatePath = "Assets/Templates/";

    [MenuItem("Tools/MVP代码生成器")]
    public static void ShowWindow()
    {
        GetWindow<MVPCodeGeneratorWindow>("MVP代码生成器");
    }

    private void OnGUI()
    {
        GUILayout.Label("MVP代码生成", EditorStyles.boldLabel);

        uiPrefab = (GameObject)EditorGUILayout.ObjectField("UI预制体", uiPrefab, typeof(GameObject), false);

        outputFolder = EditorGUILayout.TextField("输出目录", outputFolder);

        if (GUILayout.Button("生成代码"))
        {
            if (uiPrefab == null)
            {
                EditorUtility.DisplayDialog("错误", "请指定一个 UI 预制体！", "确定");
                return;
            }

            GenerateMVPScripts(uiPrefab.name);
        }
    }

    private void GenerateMVPScripts(string uiName)
    {
        string className = uiName.Replace("View", "");

        string viewTemplatePath = Path.Combine(templatePath, "ViewTemplate.txt");
        string presenterTemplatePath = Path.Combine(templatePath, "PresenterTemplate.txt");

        if (!File.Exists(viewTemplatePath) || !File.Exists(presenterTemplatePath))
        {
            Debug.LogError("模板文件未找到！");
            return;
        }

        string viewCode = File.ReadAllText(viewTemplatePath).Replace("{UI_NAME}", className);
        string presenterCode = File.ReadAllText(presenterTemplatePath).Replace("{UI_NAME}", className);

        string outputPath = Path.Combine(outputFolder, className);
        if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);

        File.WriteAllText(Path.Combine(outputPath, $"{className}View.cs"), viewCode);
        File.WriteAllText(Path.Combine(outputPath, $"{className}Presenter.cs"), presenterCode);

        AssetDatabase.Refresh();

        Debug.Log($"MVP代码生成成功：{outputPath}");
    }
}

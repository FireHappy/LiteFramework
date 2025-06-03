using UnityEditor;
using UnityEngine;
using LiteFramework.Configs;
using System.IO;

public static class UIConfigCreator
{
    private const string UICreatorConfigPath = "Packages/com.liteframework.unity/Runtime/DefaultAssets/Configs/UIGeneratorConfig.asset";

    [MenuItem("Assets/Create/LiteFramework/UI Generator Config")]
    public static void CreateConfigFromDefault()
    {
        var defaultConfig = AssetDatabase.LoadAssetAtPath<UIGeneratorConfig>(UICreatorConfigPath);
        if (defaultConfig == null)
        {
            Debug.LogError("默认配置未找到: " + UICreatorConfigPath);
            return;
        }

        var newConfig = Object.Instantiate(defaultConfig);

        string savePath = EditorUtility.SaveFilePanelInProject(
            "保存 UIGeneratorConfig",
            "UIGeneratorConfig",
            "asset",
            "选择保存路径");

        if (string.IsNullOrEmpty(savePath))
            return;

        AssetDatabase.CreateAsset(newConfig, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newConfig;
    }



    private const string UIRootConfigPath = "Packages/com.liteframework.unity/Runtime/DefaultAssets/Configs/UIRootConfig.asset";

    [MenuItem("Assets/Create/LiteFramework/UI Root Config")]
    public static void CreateUIRootConfig()
    {
        var defaultConfig = AssetDatabase.LoadAssetAtPath<UIRootConfig>(UIRootConfigPath);
        if (defaultConfig == null)
        {
            Debug.LogError("默认配置未找到: " + UIRootConfigPath);
            return;
        }

        var newConfig = Object.Instantiate(defaultConfig);

        string savePath = EditorUtility.SaveFilePanelInProject(
            "保存 UIRootConfig",
            "UIRootConfig",
            "asset",
            "选择保存路径");

        if (string.IsNullOrEmpty(savePath))
            return;

        AssetDatabase.CreateAsset(newConfig, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newConfig;
    }
}

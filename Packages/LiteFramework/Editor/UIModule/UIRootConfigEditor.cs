using UnityEngine;
using UnityEditor;
using LiteFramework.Configs;

[CustomEditor(typeof(UIRootConfig))]
public class UIRootConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("LiteFramework - UI 根配置", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // UIPath
        EditorGUILayout.PropertyField(serializedObject.FindProperty("UIPath"), new GUIContent("UI 路径", "用于加载 UI 资源的 Resources 相对路径"));

        // Default UI Tag
        DrawTagField("DefaultUIParentTag", "默认 UI Tag", "场景中 UI 根 Canvas 的 Tag（如 UIParent）");

        // Default Dialog Tag
        DrawTagField("DefaultUIDialogTag", "默认 Dialog Tag", "场景中 Dialog Canvas 的 Tag（如 DialogParent）");

        // RootUIPrefab
        EditorGUILayout.PropertyField(serializedObject.FindProperty("RootUIPrefab"), new GUIContent("UI 根预制体", "UI 根 Canvas（带 Camera、EventSystem）的预制体"));

        EditorGUILayout.HelpBox("建议在场景中使用 Tag 管理 UI 根节点。RootUIPrefab 可用于动态创建 Canvas 根结构。", MessageType.Info);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTagField(string propertyName, string label, string tooltip)
    {
        SerializedProperty prop = serializedObject.FindProperty(propertyName);
        string currentTag = prop.stringValue;

        EditorGUILayout.BeginHorizontal();
        prop.stringValue = EditorGUILayout.TagField(new GUIContent(label, tooltip), currentTag);
        EditorGUILayout.EndHorizontal();

        if (!IsTagDefined(currentTag))
        {
            EditorGUILayout.HelpBox($"Tag “{currentTag}” 在项目中未定义，请检查 Tags 设置（Edit > Project Settings > Tags and Layers）", MessageType.Warning);
        }
    }

    private bool IsTagDefined(string tag)
    {
        foreach (var definedTag in UnityEditorInternal.InternalEditorUtility.tags)
        {
            if (definedTag == tag)
                return true;
        }
        return false;
    }
}

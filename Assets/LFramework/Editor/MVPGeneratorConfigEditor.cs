using UnityEditor;
using UnityEngine;
using LFramework.Utility;

[CustomEditor(typeof(MVPGeneratorConfig))]
public class MVPGeneratorConfigEditor : Editor
{
    private SerializedProperty outputRootPath;
    private SerializedProperty templateRootPath;
    private SerializedProperty mappings;

    private string[] commonComponentTypes = new[]
    {
        "UnityEngine.UI.Button",
        "UnityEngine.UI.Image",
        "UnityEngine.UI.Text",
        "TMPro.TMP_Text",
        "TMPro.TMP_InputField",
        "UnityEngine.UI.Toggle",
        "UnityEngine.UI.Slider",
    };

    private void OnEnable()
    {
        outputRootPath = serializedObject.FindProperty("outputRootPath");
        templateRootPath = serializedObject.FindProperty("templateRootPath");
        mappings = serializedObject.FindProperty("mappings");
    }
    private void DrawPathSelector(SerializedProperty property, string label)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);

        EditorGUI.BeginChangeCheck();
        string currentPath = property.stringValue;

        // 可输入路径
        currentPath = EditorGUILayout.TextField(currentPath);

        // 可视化按钮：选择文件夹
        if (GUILayout.Button("选择", GUILayout.Width(60)))
        {
            string newPath = EditorUtility.OpenFolderPanel($"选择 {label}", Application.dataPath, "");
            if (!string.IsNullOrEmpty(newPath))
            {
                // 转换为相对路径
                if (newPath.StartsWith(Application.dataPath))
                {
                    newPath = "Assets" + newPath.Substring(Application.dataPath.Length);
                }
                currentPath = newPath;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            property.stringValue = currentPath;
        }

        EditorGUILayout.EndHorizontal();

        // 路径存在性提示
        if (!AssetDatabase.IsValidFolder(currentPath))
        {
            EditorGUILayout.HelpBox($"路径不存在: {currentPath}", MessageType.Warning);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("通用配置", EditorStyles.boldLabel);
        DrawPathSelector(outputRootPath, "生成代码路径");
        DrawPathSelector(templateRootPath, "模板路径");


        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("组件命名前缀映射", EditorStyles.boldLabel);

        for (int i = 0; i < mappings.arraySize; i++)
        {
            var element = mappings.GetArrayElementAtIndex(i);
            var prefix = element.FindPropertyRelative("prefix");
            var typeName = element.FindPropertyRelative("componentTypeName");

            EditorGUILayout.BeginHorizontal();
            prefix.stringValue = EditorGUILayout.TextField(prefix.stringValue, GUILayout.Width(100));

            // 支持常用组件类型下拉选择
            int selectedIndex = Mathf.Max(0, System.Array.IndexOf(commonComponentTypes, typeName.stringValue));
            int newIndex = EditorGUILayout.Popup(selectedIndex, commonComponentTypes);
            typeName.stringValue = commonComponentTypes[newIndex];

            // 删除按钮
            if (GUILayout.Button("🗑", GUILayout.Width(25)))
            {
                mappings.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("添加映射"))
        {
            mappings.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LFramework.EditorTools
{
    [CustomEditor(typeof(UIGeneratorConfig))]
    public class UIGeneratorConfigEditor : Editor
    {
        private SerializedProperty mappings;
        private SerializedProperty outputRootPathProp;
        private SerializedProperty templateRootPathProp;

        private List<Type> availableTypes;

        private void OnEnable()
        {
            mappings = serializedObject.FindProperty("mappings");
            outputRootPathProp = serializedObject.FindProperty("outputRootPath");
            templateRootPathProp = serializedObject.FindProperty("templateRootPath");

            availableTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => typeof(Component).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .OrderBy(t => t.FullName)
                .ToList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("MVP 路径配置", EditorStyles.boldLabel);

            // 输出路径选择
            DrawFolderPathField("输出路径", outputRootPathProp);

            // 模板路径选择
            DrawFolderPathField("模板路径", templateRootPathProp);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("前缀组件映射", EditorStyles.boldLabel);

            for (int i = 0; i < mappings.arraySize; i++)
            {
                var element = mappings.GetArrayElementAtIndex(i);
                var prefixProp = element.FindPropertyRelative("prefix");
                var typeNameProp = element.FindPropertyRelative("componentType").FindPropertyRelative("typeName");

                EditorGUILayout.BeginHorizontal();

                prefixProp.stringValue = EditorGUILayout.TextField(prefixProp.stringValue, GUILayout.Width(100));

                // 显示当前类型名（简短）
                string currentTypeShort = "(未设置)";
                var currentType = Type.GetType(typeNameProp.stringValue);
                if (currentType != null) currentTypeShort = currentType.FullName;

                if (GUILayout.Button(currentTypeShort, EditorStyles.popup))
                {
                    var buttonRect = GUILayoutUtility.GetLastRect();
                    var screenPos = GUIUtility.GUIToScreenPoint(new Vector2(buttonRect.x, buttonRect.y));
                    var dropdownRect = new Rect(screenPos.x, screenPos.y + buttonRect.height, buttonRect.width, 300); // 下方显示

                    SearchableTypePopup.Show(dropdownRect, availableTypes, typeNameProp.stringValue, selected =>
                    {
                        typeNameProp.stringValue = selected;
                        serializedObject.ApplyModifiedProperties();
                    });
                }

                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    mappings.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+ 添加新映射"))
            {
                mappings.arraySize++;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFolderPathField(string label, SerializedProperty pathProp)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            pathProp.stringValue = EditorGUILayout.TextField(pathProp.stringValue);
            if (GUILayout.Button("浏览", GUILayout.Width(60)))
            {
                string defaultPath = string.IsNullOrEmpty(pathProp.stringValue) ? Application.dataPath : System.IO.Path.Combine(Application.dataPath, pathProp.stringValue.TrimStart("Assets/".ToCharArray()));
                string folder = EditorUtility.OpenFolderPanel($"选择{label}", defaultPath, "");
                if (!string.IsNullOrEmpty(folder))
                {
                    // 转成项目相对路径
                    if (folder.StartsWith(Application.dataPath))
                    {
                        folder = "Assets" + folder.Substring(Application.dataPath.Length);
                    }
                    pathProp.stringValue = folder;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}

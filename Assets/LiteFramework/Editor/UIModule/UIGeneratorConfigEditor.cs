using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using LiteFramework.Configs;

namespace LiteFramework.EditorTools
{
    [CustomEditor(typeof(UIGeneratorConfig))]
    public class UIGeneratorConfigEditor : Editor
    {
        private SerializedProperty mappings;
        private SerializedProperty outputRootPathProp;
        private SerializedProperty templateRootPathProp;
        private SerializedProperty nameSpaceProp;

        private List<Type> availableTypes;

        // 存储每个按钮的位置
        private Dictionary<int, Rect> buttonRects = new Dictionary<int, Rect>();
        // 记录当前被点击的按钮索引
        private int clickedButtonIndex = -1;

        private void OnEnable()
        {
            mappings = serializedObject.FindProperty("mappings");
            outputRootPathProp = serializedObject.FindProperty("outputRootPath");
            templateRootPathProp = serializedObject.FindProperty("templateRootPath");
            nameSpaceProp = serializedObject.FindProperty("nameSpace");

            availableTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => typeof(Component).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .OrderBy(t => t.FullName)
                .ToList();

            buttonRects.Clear();
            clickedButtonIndex = -1;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("MVP 路径配置", EditorStyles.boldLabel);

            // 输出路径选择
            DrawFolderPathField("输出路径", outputRootPathProp);

            // 模板路径选择
            DrawFolderPathField("模板路径", templateRootPathProp);

            //绘制命名空间
            DrawField("命名空间", nameSpaceProp);

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
                if (currentType != null) currentTypeShort = currentType.Name; // 使用短名称显示

                // 绘制按钮
                if (GUILayout.Button(currentTypeShort, EditorStyles.popup))
                {
                    clickedButtonIndex = i;
                }

                // 在绘制后立即获取按钮位置（在事件处理之前）
                Rect buttonRect = GUILayoutUtility.GetLastRect();
                buttonRects[i] = buttonRect;

                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    mappings.DeleteArrayElementAtIndex(i);
                    // 删除后需要重置索引
                    clickedButtonIndex = -1;
                    buttonRects.Clear();
                    break; // 跳出循环避免索引错误
                }

                EditorGUILayout.EndHorizontal();
            }

            // 在布局完成后处理点击事件
            if (clickedButtonIndex >= 0 && Event.current.type == EventType.Repaint)
            {
                if (buttonRects.TryGetValue(clickedButtonIndex, out Rect buttonRect))
                {
                    var element = mappings.GetArrayElementAtIndex(clickedButtonIndex);
                    var typeNameProp = element.FindPropertyRelative("componentType").FindPropertyRelative("typeName");

                    // 转换为屏幕坐标
                    Rect screenRect = GUIUtility.GUIToScreenRect(buttonRect);

                    // 计算弹窗位置（按钮正下方）
                    float maxHeight = Mathf.Min(300, Screen.height - screenRect.yMax - 20);
                    var dropdownRect = new Rect(
                        screenRect.x,
                        screenRect.yMax,
                        screenRect.width,
                        maxHeight
                    );

                    SearchableTypePopup.Show(dropdownRect, availableTypes, typeNameProp.stringValue, selected =>
                    {
                        typeNameProp.stringValue = selected;
                        serializedObject.ApplyModifiedProperties();
                    });
                }

                clickedButtonIndex = -1; // 重置点击状态
            }

            if (GUILayout.Button("+ 添加新映射"))
            {
                mappings.arraySize++;
                // 添加新元素后重置状态
                clickedButtonIndex = -1;
                buttonRects.Clear();
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

        private void DrawField(string label, SerializedProperty prop)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            prop.stringValue = EditorGUILayout.TextField(prop.stringValue);
            EditorGUILayout.EndHorizontal();
        }
    }
}
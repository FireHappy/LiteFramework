using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LFramework.EditorTools
{
    [CustomEditor(typeof(MVPGeneratorConfig))]
    public class MVPGeneratorConfigEditor : Editor
    {
        private SerializedProperty mappings;
        private List<Type> availableTypes;

        private void OnEnable()
        {
            mappings = serializedObject.FindProperty("mappings");

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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("outputRootPath"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("templateRootPath"));

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
                    var rect = GUILayoutUtility.GetLastRect();
                    SearchableTypePopup.Show(rect, availableTypes, typeNameProp.stringValue, selected =>
                    {
                        typeNameProp.stringValue = selected;
                        serializedObject.ApplyModifiedProperties();
                    });
                }

                if (GUILayout.Button("🗑", GUILayout.Width(25)))
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
    }
}

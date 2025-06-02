using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LiteFramework.EditorTools
{
    public class SearchableTypePopup : EditorWindow
    {
        private Action<string> onSelected;
        private List<string> displayNames;
        private List<string> fullTypeNames;
        private string searchText = "";
        private Vector2 scroll;
        private static GUIStyle SafeSearchFieldStyle => GUI.skin.FindStyle("ToolbarSearchTextField") ?? EditorStyles.textField;

        private static Rect? activatorRect; // 存储激活位置

        public static void Show(Rect activatorRect, List<Type> types, string currentType, Action<string> onSelected)
        {
            // 存储激活位置用于计算正确位置
            SearchableTypePopup.activatorRect = activatorRect;

            var window = CreateInstance<SearchableTypePopup>();
            window.displayNames = new List<string>();
            window.fullTypeNames = new List<string>();
            window.onSelected = onSelected;

            foreach (var type in types)
            {
                window.displayNames.Add(type.FullName);
                window.fullTypeNames.Add(type.AssemblyQualifiedName);
            }

            float height = Mathf.Min(300, types.Count * 18 + 30);

            // 计算正确的位置 - 确保在屏幕范围内
            Vector2 position = activatorRect.position;
            position.y += activatorRect.height;

            // 检查是否会超出屏幕底部
            float maxHeight = Mathf.Min(height, Screen.height - position.y - 30);

            // 如果高度不够，显示在按钮上方
            if (maxHeight < 100)
            {
                position.y = activatorRect.y - maxHeight;
            }

            window.ShowAsDropDown(new Rect(position.x, position.y, activatorRect.width, 1),
                                new Vector2(activatorRect.width, maxHeight));
        }

        private void OnGUI()
        {
            // 如果没有设置激活位置，尝试从当前事件获取
            if (activatorRect == null && Event.current != null)
            {
                activatorRect = new Rect(position.x, position.y - 30, position.width, 30);
            }

            // 使用统一结构：搜索框 + 清除按钮
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            searchText = EditorGUILayout.TextField(searchText, SafeSearchFieldStyle, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                searchText = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();

            scroll = EditorGUILayout.BeginScrollView(scroll);

            for (int i = 0; i < displayNames.Count; i++)
            {
                if (!string.IsNullOrEmpty(searchText) && !displayNames[i].ToLower().Contains(searchText.ToLower()))
                    continue;

                if (GUILayout.Button(displayNames[i], EditorStyles.label))
                {
                    onSelected?.Invoke(fullTypeNames[i]);
                    Close();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        void OnLostFocus()
        {
            Close();
        }
    }
}
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
        private static GUIStyle searchFieldStyle;

        public static void Show(Rect activatorRect, List<Type> types, string currentType, Action<string> onSelected)
        {
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
            window.ShowAsDropDown(activatorRect, new Vector2(500, height));
        }

        private void OnGUI()
        {
            if (searchFieldStyle == null)
                searchFieldStyle = GUI.skin.FindStyle("ToolbarSeachTextField");

            EditorGUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            searchText = EditorGUILayout.TextField(searchText, searchFieldStyle);
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
    }
}

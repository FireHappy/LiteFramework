using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

public class NamespaceBatchEditor : EditorWindow
{
    private string folderPath = "Assets";
    private string targetNamespace = "";

    [MenuItem("Tools/创建命名空间")]
    public static void ShowWindow()
    {
        GetWindow<NamespaceBatchEditor>("批量命名空间修改");
    }

    private void OnGUI()
    {
        GUILayout.Label("批量修改或创建命名空间", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择文件夹：", GUILayout.Width(70));
        folderPath = EditorGUILayout.TextField(folderPath);
        if (GUILayout.Button("选择", GUILayout.Width(60)))
        {
            string selectPath = EditorUtility.OpenFolderPanel("选择脚本文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(selectPath))
            {
                if (selectPath.StartsWith(Application.dataPath))
                {
                    folderPath = "Assets" + selectPath.Substring(Application.dataPath.Length);
                }
                else
                {
                    Debug.LogError("请选择Assets文件夹下的路径！");
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        targetNamespace = EditorGUILayout.TextField("命名空间：", targetNamespace);

        EditorGUILayout.Space();

        if (GUILayout.Button("执行修改"))
        {
            if (string.IsNullOrEmpty(targetNamespace))
            {
                EditorUtility.DisplayDialog("错误", "命名空间不能为空", "确定");
                return;
            }

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                EditorUtility.DisplayDialog("错误", "无效的文件夹路径", "确定");
                return;
            }

            ModifyNamespaceInFolder(folderPath, targetNamespace);
        }
    }

    private void ModifyNamespaceInFolder(string folder, string newNamespace)
    {
        string relativePath = folder;
        if (relativePath.StartsWith("Assets"))
        {
            relativePath = relativePath.Substring("Assets".Length);
        }

        string fullPath = Path.Combine(Application.dataPath, relativePath.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

        if (!Directory.Exists(fullPath))
        {
            Debug.LogError($"文件夹不存在：{fullPath}");
            return;
        }

        string[] files = Directory.GetFiles(fullPath, "*.cs", SearchOption.AllDirectories);
        int modifiedCount = 0;

        foreach (string file in files)
        {
            string code = File.ReadAllText(file);
            string newCode = ModifyNamespaceInCode(code, newNamespace);

            if (newCode != code)
            {
                // 备份
                string backupFile = file + ".bak";
                if (!File.Exists(backupFile))
                {
                    File.Copy(file, backupFile);
                }

                File.WriteAllText(file, newCode);
                modifiedCount++;
                Debug.Log($"修改文件：{file}");
            }
        }

        // 删除所有 .bak 备份文件
        string[] bakFiles = Directory.GetFiles(fullPath, "*.cs.bak", SearchOption.AllDirectories);
        foreach (string bak in bakFiles)
        {
            try
            {
                File.Delete(bak);
                Debug.Log($"删除备份文件：{bak}");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"删除备份文件失败：{bak}，原因：{e.Message}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", $"处理完成，共修改 {modifiedCount} 个文件，已删除备份文件", "确定");
    }

    private string ModifyNamespaceInCode(string code, string newNamespace)
    {
        var lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        // 找using结束的索引(最后一个using语句所在行)
        int lastUsingIndex = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.StartsWith("using ") || line == "" || line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*"))
            {
                // 允许using后有注释或空行
                lastUsingIndex = i;
            }
            else
            {
                // 遇到第一条非using且非注释的语句就停止
                break;
            }
        }

        // 分割文件：using部分 + 其他代码部分
        var usingLines = new List<string>();
        var restLines = new List<string>();

        for (int i = 0; i <= lastUsingIndex; i++)
        {
            usingLines.Add(lines[i]);
        }
        for (int i = lastUsingIndex + 1; i < lines.Length; i++)
        {
            restLines.Add(lines[i]);
        }

        // 判断 restLines 是否已经有 namespace
        int namespaceLineIndex = -1;
        for (int i = 0; i < restLines.Count; i++)
        {
            if (restLines[i].TrimStart().StartsWith("namespace "))
            {
                namespaceLineIndex = i;
                break;
            }
        }

        if (namespaceLineIndex >= 0)
        {
            // 已经有namespace，替换命名空间名称
            string oldNamespaceLine = restLines[namespaceLineIndex];
            string indent = oldNamespaceLine.Substring(0, oldNamespaceLine.IndexOf("namespace"));
            restLines[namespaceLineIndex] = indent + "namespace " + newNamespace;

            // 直接把using + 修改后的restLines合并
            var finalLines = new List<string>();
            finalLines.AddRange(usingLines);
            finalLines.AddRange(restLines);
            return string.Join("\n", finalLines);
        }
        else
        {
            // 没有namespace，给restLines包裹namespace
            var finalLines = new List<string>();
            finalLines.AddRange(usingLines);
            finalLines.Add(""); // 空行

            finalLines.Add("namespace " + newNamespace);
            finalLines.Add("{");

            // 给restLines每行前加4空格缩进
            foreach (var line in restLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    finalLines.Add("");
                else
                    finalLines.Add("    " + line);
            }

            finalLines.Add("}");

            return string.Join("\n", finalLines);
        }
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefixComponentMapping
{
    public string prefix;
    public string componentTypeName; // 用字符串存储，支持反射
}

[CreateAssetMenu(fileName = "MVPGeneratorConfig", menuName = "Tools/MVPGeneratorConfig", order = 0)]
public class MVPGeneratorConfig : ScriptableObject
{
    public string outputRootPath = "Assets/Scripts/UI/";
    public string templateRootPath = "Assets/Templates/";


    public List<PrefixComponentMapping> mappings = new List<PrefixComponentMapping>();

    public Type GetComponentTypeFromPrefix(string prefix)
    {
        foreach (var map in mappings)
        {
            if (string.Equals(map.prefix, prefix, StringComparison.OrdinalIgnoreCase))
            {
                return Type.GetType(map.componentTypeName);
            }
        }

        return null;
    }
}

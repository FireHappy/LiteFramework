using UnityEngine;

public static class UIUtility
{
    public static T CreatePanel<T>(Transform parent, string panelPath, bool setActive = true) where T : BasePanel
    {
        string prefabName = typeof(T).Name;
        var go = Resources.Load<GameObject>($"{panelPath}/{prefabName}");
        if (go == null)
        {
            Debug.LogWarning($"找不到 Panel 资源：{panelPath}/{prefabName}");
            return null;
        }

        var instance = GameObject.Instantiate(go, parent).transform;
        instance.name = prefabName;
        instance.localScale = Vector3.one;
        instance.gameObject.SetActive(setActive);
        return instance.GetComponent<T>();
    }

    public static T CreateItem<T>(Transform parent, string itemPath, bool setActive = true) where T : BaseItem
    {
        string prefabName = typeof(T).Name;
        var go = Resources.Load<GameObject>($"{itemPath}/{prefabName}");
        if (go == null)
        {
            Debug.LogWarning($"找不到 Item 资源：{itemPath}/{prefabName}");
            return null;
        }

        var instance = GameObject.Instantiate(go, parent).transform;
        instance.name = prefabName;
        instance.localScale = Vector3.one;
        instance.gameObject.SetActive(setActive);
        return instance.GetComponent<T>();
    }

    public static void DestroyUI(Transform uiTransform)
    {
#if UNITY_EDITOR
        Object.DestroyImmediate(uiTransform.gameObject);
#else
        Object.Destroy(uiTransform.gameObject);
#endif
    }

    public static Transform FindUI<T>(Transform parent)
    {
        return parent.Find(typeof(T).Name);
    }
}

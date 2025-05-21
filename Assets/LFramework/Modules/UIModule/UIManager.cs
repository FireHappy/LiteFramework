using UnityEngine;

public class UIManager
{
    private Transform uiParent;
    private Transform dialogParent;

    public T OpenPanel<T>(bool dialog = false, Transform parent = null) where T : BasePanel
    {
        if (uiParent == null)
        {
            parent = dialog ? GetDialogParent() : GetUIParent();
        }
        var existing = UIUtility.FindUI<T>(parent);
        if (existing != null)
        {
            existing.SetAsLastSibling();
            existing.gameObject.SetActive(true);
            return existing.GetComponent<T>();
        }

        return UIUtility.CreatePanel<T>(parent, "UI/Panel");
    }

    public void ClosePanel<T>(Transform parent = null) where T : BasePanel
    {
        if (parent == null)
            parent = GetUIParent();
        var tsf = UIUtility.FindUI<T>(parent);
        if (tsf != null)
        {
            UIUtility.DestroyUI(tsf);
        }

        // 恢复上一个界面
        var top = GetTopChild(parent);
        if (top != null)
        {
            top.gameObject.SetActive(true);
        }
    }

    private Transform GetTopChild(Transform tsf)
    {
        if (tsf.childCount > 0)
        {
            return tsf.GetChild(tsf.childCount - 1);
        }
        return null;
    }

    public T CreateItem<T>(Transform parent) where T : BaseItem
    {
        return UIUtility.CreateItem<T>(parent, "UI/Item");
    }

    public Transform GetDialogParent()
    {
        if (dialogParent == null)
        {
            dialogParent = GameObject.FindWithTag("DialogParent")?.transform;
        }
        if (dialogParent == null)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("UI/UICanvas"));
            go.name = "UICanvas";
            dialogParent = GameObject.FindWithTag("DialogParent")?.transform;
        }
        return dialogParent;
    }

    public Transform GetUIParent()
    {
        if (uiParent == null)
        {
            uiParent = GameObject.FindWithTag("UIParent")?.transform;
        }
        if (uiParent == null)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("UI/UICanvas"));
            go.name = "UICanvas";
            uiParent = GameObject.FindWithTag("UIParent")?.transform;
        }
        return uiParent;
    }
}

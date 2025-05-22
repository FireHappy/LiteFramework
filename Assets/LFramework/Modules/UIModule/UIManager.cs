using LFramework.Utility;
using UnityEngine;
using VContainer;

public enum UIType
{
    Panel,
    Item,
    Dialog
}

public class UIManager : IUIManager
{
    private string panelPath = "UI/Panel";
    private string itemPath = "UI/Item";
    private string dialogPath = "UI/Dialog";
    private string defaultUIParentTag = "UIParent";
    private string defaultUIDialogTag = "DialogParent";
    private string rootUIPath = "UI/UICanvas";

    private readonly IObjectResolver container;
    private Transform uiParent;
    private Transform dialogParent;

    public UIManager(IObjectResolver container)
    {
        this.container = container;
    }

    public TPresenter OpenUI<TPresenter, TView>(UIType type, Transform parent = null)
    where TPresenter : BasePresenter<TView>
    where TView : BaseView<TPresenter>
    {
        string uiPath = "";
        switch (type)
        {
            case UIType.Panel:
                parent ??= GetUIParent();
                uiPath = panelPath;
                break;
            case UIType.Dialog:
                parent ??= GetDialogParent();
                uiPath = dialogPath;
                break;
            case UIType.Item:
                parent ??= GetUIParent();
                uiPath = itemPath;
                break;
        }
        TView view;
        var existing = UIUtility.FindUI<TView>(parent);
        if (existing != null)
        {
            existing.SetAsLastSibling();
            existing.gameObject.SetActive(true);
            view = existing.GetComponent<TView>();
        }
        else
        {
            view = UIUtility.CreateUI<TView>(parent, uiPath);
            //处理ui组件的注入
            AutoInjectComponent.AutoInject(view.transform, view);
        }
        var presenter = container.Resolve<TPresenter>();
        view.BindPresenter(presenter);
        presenter.SetView(view);
        return presenter;
    }

    public void CloseUI<TPresenter, TView>(UIType type, Transform parent = null) where TPresenter : BasePresenter<TView>
    where TView : BaseView<TPresenter>
    {
        switch (type)
        {
            case UIType.Panel:
                parent ??= GetUIParent();
                break;
            case UIType.Dialog:
                parent ??= GetDialogParent();
                break;
            case UIType.Item:
                parent ??= GetUIParent();
                break;
        }
        var tsf = UIUtility.FindUI<TView>(parent);
        if (tsf != null)
        {
            var view = tsf.GetComponent<TView>();
            if (view != null && view.presenter != null)
            {
                view.presenter.DetachView();
            }
            UIUtility.DestroyUI(tsf);
        }
        if (type == UIType.Panel)
        {
            var top = GetTopChild(parent);
            if (top != null)
            {
                top.gameObject.SetActive(true);
            }
        }
    }

    private Transform GetDialogParent()
    {
        if (dialogParent == null)
        {
            dialogParent = GameObject.FindWithTag(defaultUIDialogTag)?.transform;
        }
        if (dialogParent == null)
        {
            GameObject.Instantiate(Resources.Load<GameObject>(rootUIPath));
            dialogParent = GameObject.FindWithTag(defaultUIDialogTag)?.transform;
        }
        return dialogParent;
    }

    private Transform GetUIParent()
    {
        if (uiParent == null)
        {
            uiParent = GameObject.FindWithTag(defaultUIParentTag)?.transform;
        }
        if (uiParent == null)
        {
            GameObject.Instantiate(Resources.Load<GameObject>(rootUIPath));
            uiParent = GameObject.FindWithTag(defaultUIParentTag)?.transform;
        }
        return uiParent;
    }


    private Transform GetTopChild(Transform tsf)
    {
        if (tsf.childCount > 0)
        {
            return tsf.GetChild(tsf.childCount - 1);
        }
        return null;
    }
}

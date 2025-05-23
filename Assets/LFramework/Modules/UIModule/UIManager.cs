using LFramework.Utility;
using LFramework.Core.MVP;
using UnityEngine;
using VContainer;

namespace LFramework.Module.UI
{
    public enum UIType
    {
        Panel,
        Item,
        Dialog
    }

    public class UIManager : IUIManager
    {

        private string uiPath = "UI";
        private string defaultUIParentTag = "UIParent";
        private string defaultUIDialogTag = "DialogParent";
        private string rootUIPrefab = "UI/UICanvas";

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
            switch (type)
            {
                case UIType.Panel:
                    parent ??= GetUIParent();
                    GetTopChild(parent)?.gameObject.SetActive(false);
                    break;
                case UIType.Dialog:
                    parent ??= GetDialogParent();
                    break;
                case UIType.Item:
                    parent ??= GetUIParent();
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
            presenter.AttachView(view);
            view.BindPresenter(presenter);
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
                    view.UnBindPresenter();
                }
                UIUtility.DestroyUI(tsf);
            }
            if (type == UIType.Panel)
            {
                GetTopChild(parent)?.gameObject.SetActive(false);
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
                GameObject.Instantiate(Resources.Load<GameObject>(rootUIPrefab));
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
                GameObject.Instantiate(Resources.Load<GameObject>(rootUIPrefab));
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

}

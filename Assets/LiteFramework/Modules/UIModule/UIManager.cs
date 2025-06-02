using UnityEngine;
using VContainer;
using LiteFramework.Core.Module.UI;
using LiteFramework.Core.Utility;
using System;
using LiteFramework.Configs;

namespace LiteFramework.Module.UI
{
    public enum UIType
    {
        Panel,
        Item,
        Dialog
    }

    public class UIManager : IUIManager
    {

        private readonly IObjectResolver container;
        private readonly UIRootConfig config;
        private Transform uiParent;
        private Transform dialogParent;

        public UIManager(IObjectResolver container, UIRootConfig config)
        {
            this.container = container;
            this.config = config;
        }

        public void OpenUI<TPresenter, TView>(UIType type = UIType.Panel, Transform parent = null)
        where TPresenter : BaseUIPresenter<TView>
        where TView : BaseUIView<TPresenter>
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
                view = UIUtility.CreateUI<TView>(parent, config.UIPath);
                //初始化组件
                view.InitComponents();
            }
            var presenter = container.Resolve<TPresenter>();
            presenter.AttachView(view);
            view.BindPresenter(presenter);
        }

        public void CloseUI<TPresenter, TView>(UIType type = UIType.Panel, Transform parent = null) where TPresenter : BaseUIPresenter<TView>
        where TView : BaseUIView<TPresenter>
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
                GetTopChild(parent)?.gameObject.SetActive(true);
            }
        }


        public void OpenUIAsync<TPresenter, TView>(UIType type = UIType.Panel, Transform parent = null, Action success = null, Action<string> failed = null)
                   where TPresenter : BaseUIPresenter<TView>
                   where TView : BaseUIView<TPresenter>
        {
            //todo 实现UI的异步加载
        }

        public void CloseUIAsync<TPresenter, TView>(UIType type = UIType.Panel, Transform parent = null, Action success = null, Action<string> failed = null)
            where TPresenter : BaseUIPresenter<TView>
            where TView : BaseUIView<TPresenter>
        {
            //todo 实现UI的异步销毁
        }

        private Transform GetDialogParent()
        {
            if (dialogParent == null)
            {
                dialogParent = GameObject.FindWithTag(config.DefaultUIDialogTag)?.transform;
            }
            if (dialogParent == null && config.RootUIPrefab != null)
            {
                GameObject.Instantiate(config.RootUIPrefab);
                dialogParent = GameObject.FindWithTag(config.DefaultUIDialogTag)?.transform;
            }
            return dialogParent;
        }

        private Transform GetUIParent()
        {
            if (uiParent == null)
            {
                uiParent = GameObject.FindWithTag(config.DefaultUIParentTag)?.transform;
            }
            if (uiParent == null && config.RootUIPrefab != null)
            {
                GameObject.Instantiate(config.RootUIPrefab);
                uiParent = GameObject.FindWithTag(config.DefaultUIParentTag)?.transform;
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

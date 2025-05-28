using System;
using System.Collections.Generic;
using UnityEngine;
using LiteFramework.Core.Module.UI;
using LiteFramework.Core.MVP;

namespace LiteFramework.Module.UI
{
    public class UIRouter
    {
        private readonly IUIManager uiManager;

        private static readonly Dictionary<Type, Type> PresenterTypeCache = new();

        private static readonly Dictionary<(Type presenter, Type view), Func<UIType, Transform, IPresenter>> OpenDelegates = new();
        private static readonly Dictionary<(Type presenter, Type view), Action<UIType, Transform>> CloseDelegates = new();

        public UIRouter(IUIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        public IPresenter Open<TView>(UIType type = UIType.Panel, Transform parent = null)
            where TView : IView
        {
            var presenterType = GetPresenterTypeFromView<TView>();
            if (presenterType == null)
            {
                Debug.LogError($"UIRouter.Open -> Failed to resolve Presenter type for view {typeof(TView).Name}");
                return null;
            }

            var openDel = GetOpenDelegate(presenterType, typeof(TView));
            return openDel(type, parent);
        }

        public void Close<TView>(UIType type = UIType.Panel, Transform parent = null)
            where TView : IView
        {
            var presenterType = GetPresenterTypeFromView<TView>();
            if (presenterType == null)
            {
                Debug.LogError($"UIRouter.Close -> Failed to resolve Presenter type for view {typeof(TView).Name}");
                return;
            }

            var closeDel = GetCloseDelegate(presenterType, typeof(TView));
            closeDel(type, parent);
        }

        private Type GetPresenterTypeFromView<TView>() where TView : IView
        {
            var viewType = typeof(TView);

            if (PresenterTypeCache.TryGetValue(viewType, out var cachedType))
                return cachedType;

            var baseType = viewType.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseUIView<>))
                {
                    var presenterType = baseType.GetGenericArguments()[0];
                    PresenterTypeCache[viewType] = presenterType;
                    return presenterType;
                }

                baseType = baseType.BaseType;
            }

            return null;
        }

        private Func<UIType, Transform, IPresenter> GetOpenDelegate(Type presenterType, Type viewType)
        {
            var key = (presenterType, viewType);
            if (OpenDelegates.TryGetValue(key, out var dlg))
                return dlg;

            var method = typeof(IUIManager).GetMethod("OpenUI")?.MakeGenericMethod(presenterType, viewType);
            var del = (Func<UIType, Transform, IPresenter>)Delegate.CreateDelegate(
                typeof(Func<UIType, Transform, IPresenter>), uiManager, method);
            OpenDelegates[key] = del;
            return del;
        }

        private Action<UIType, Transform> GetCloseDelegate(Type presenterType, Type viewType)
        {
            var key = (presenterType, viewType);
            if (CloseDelegates.TryGetValue(key, out var dlg))
                return dlg;

            var method = typeof(IUIManager).GetMethod("CloseUI")?.MakeGenericMethod(presenterType, viewType);
            var del = (Action<UIType, Transform>)Delegate.CreateDelegate(
                typeof(Action<UIType, Transform>), uiManager, method);
            CloseDelegates[key] = del;
            return del;
        }
    }

}

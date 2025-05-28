using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using LiteFramework.Core.Module.UI;
using LiteFramework.Core.MVP;

namespace LiteFramework.Module.UI
{
    public class UIRouter
    {
        private readonly IUIManager uiManager;

        private static readonly Dictionary<Type, Type> PresenterTypeCache = new();

        private static readonly Dictionary<(Type presenter, Type view), Action<IUIManager, UIType, Transform>> OpenDelegates = new();
        private static readonly Dictionary<(Type presenter, Type view), Action<IUIManager, UIType, Transform>> CloseDelegates = new();

        public UIRouter(IUIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        public void Open<TView>(UIType type = UIType.Panel, Transform parent = null)
            where TView : IView
        {
            var presenterType = GetPresenterTypeFromView<TView>();
            if (presenterType == null)
            {
                Debug.LogError($"UIRouter.Open -> Failed to resolve Presenter type for view {typeof(TView).Name}");
                return;
            }

            var openDel = GetOpenDelegate(presenterType, typeof(TView));
            openDel(uiManager, type, parent);
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
            closeDel(uiManager, type, parent);
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

        private Action<IUIManager, UIType, Transform> GetOpenDelegate(Type presenterType, Type viewType)
        {
            var key = (presenterType, viewType);
            if (OpenDelegates.TryGetValue(key, out var dlg))
            {
                return dlg;
            }

            var method = typeof(IUIManager).GetMethod("OpenUI");
            if (method == null)
                throw new InvalidOperationException("OpenUI method not found on IUIManager");

            var genericMethod = method.MakeGenericMethod(presenterType, viewType);

            // 构建参数表达式
            var paramManager = Expression.Parameter(typeof(IUIManager), "manager");
            var paramType = Expression.Parameter(typeof(UIType), "type");
            var paramParent = Expression.Parameter(typeof(Transform), "parent");

            // manager.OpenUI<Presenter, View>(type, parent)
            var call = Expression.Call(paramManager, genericMethod, paramType, paramParent);

            var lambda = Expression.Lambda<Action<IUIManager, UIType, Transform>>(call, paramManager, paramType, paramParent);
            dlg = lambda.Compile();

            OpenDelegates[key] = dlg;
            return dlg;
        }

        private Action<IUIManager, UIType, Transform> GetCloseDelegate(Type presenterType, Type viewType)
        {
            var key = (presenterType, viewType);
            if (CloseDelegates.TryGetValue(key, out var dlg))
            {
                return dlg;
            }

            var method = typeof(IUIManager).GetMethod("CloseUI");
            if (method == null)
                throw new InvalidOperationException("CloseUI method not found on IUIManager");

            var genericMethod = method.MakeGenericMethod(presenterType, viewType);

            var paramManager = Expression.Parameter(typeof(IUIManager), "manager");
            var paramType = Expression.Parameter(typeof(UIType), "type");
            var paramParent = Expression.Parameter(typeof(Transform), "parent");

            var call = Expression.Call(paramManager, genericMethod, paramType, paramParent);

            var lambda = Expression.Lambda<Action<IUIManager, UIType, Transform>>(call, paramManager, paramType, paramParent);
            dlg = lambda.Compile();

            CloseDelegates[key] = dlg;
            return dlg;
        }
    }
}

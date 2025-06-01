using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using LiteFramework.Core.Module.UI;
using LiteFramework.Core.MVP;
using LiteFramework.Generated;

namespace LiteFramework.Module.UI
{
    public class UIRouter
    {
        private readonly IUIManager uiManager;
        private static readonly Dictionary<Type, Type> PresenterTypeCache = new();
        private static readonly Dictionary<(Type, Type), Action<IUIManager, UIType, Transform>> OpenDelegates = new();
        private static readonly Dictionary<(Type, Type), Action<IUIManager, UIType, Transform>> CloseDelegates = new();

        public UIRouter(IUIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        public static void Register<TPresenter, TView>()
            where TPresenter : class, IPresenter
            where TView : class, IView
        {
            PresenterTypeCache[typeof(TView)] = typeof(TPresenter);
        }

        public static void RegisterDel<IUIManager, TView>(Transform parent)
        {

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

            Action<IUIManager, UIType, Transform> del;
            if (UIRouterMethodMapping.TryGetOpenDelegate(presenterType, typeof(TView), out del))
            {
                del(uiManager, type, parent);
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

            // 优先读取缓存
            if (PresenterTypeCache.TryGetValue(viewType, out var cachedType))
                return cachedType;

            // 其次读取特性绑定
            var attr = viewType.GetCustomAttribute<BindPresenterAttribute>();
            if (attr != null)
            {
                PresenterTypeCache[viewType] = attr.PresenterType;
                return attr.PresenterType;
            }

            // 最后推导自 BaseUIView<TPresenter>
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

            throw new InvalidOperationException(
                $"无法推导 {viewType.Name} 的 Presenter 类型。\n" +
                $"请使用 [BindPresenter(typeof(XPresenter))] 或 BaseUIView<XPresenter> 或手动注册 UIRouter.Register<XPresenter, {viewType.Name}>。");
        }

        private Action<IUIManager, UIType, Transform> GetOpenDelegate(Type presenterType, Type viewType)
        {

            var key = (presenterType, viewType);
            if (OpenDelegates.TryGetValue(key, out var dlg))
                return dlg;

            var method = typeof(IUIManager).GetMethod("OpenUI");
            if (method == null)
                throw new InvalidOperationException("OpenUI method not found on IUIManager");

            var genericMethod = method.MakeGenericMethod(presenterType, viewType);

            var paramManager = Expression.Parameter(typeof(IUIManager), "manager");
            var paramType = Expression.Parameter(typeof(UIType), "type");
            var paramParent = Expression.Parameter(typeof(Transform), "parent");

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
                return dlg;

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


        public static (Type presenterType, Type viewType) Resolve<TView>()
        {
            return (typeof(IPresenter), typeof(IView));
        }
    }
}

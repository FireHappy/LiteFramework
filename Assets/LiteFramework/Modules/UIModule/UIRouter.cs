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

            var method = typeof(IUIManager).GetMethod("OpenUI")?.MakeGenericMethod(presenterType, typeof(TView));
            return method?.Invoke(uiManager, new object[] { type, parent }) as IPresenter;
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

            var method = typeof(IUIManager).GetMethod("CloseUI")?.MakeGenericMethod(presenterType, typeof(TView));
            method?.Invoke(uiManager, new object[] { type, parent });
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
    }
}

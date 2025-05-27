
using System;
using LiteFramework.Core.Module.UI;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using UnityEngine;

public static class UIRouter
{
    private static IUIManager uiManager;

    public static void Init(IUIManager manager)
    {
        uiManager = manager;
    }

    public static IPresenter Open<TView>(UIType type = UIType.Panel, Transform parent = null)
        where TView : Component
    {
        EnsureInitialized();

        var presenterType = GetPresenterTypeFromView<TView>();
        if (presenterType == null)
        {
            Debug.LogError($"UIRouter.Open -> Failed to resolve Presenter type for view {typeof(TView).Name}");
            return null;
        }

        var method = typeof(IUIManager).GetMethod("OpenUI")?.MakeGenericMethod(presenterType, typeof(TView));
        return method?.Invoke(uiManager, new object[] { type, parent }) as IPresenter;
    }

    public static void Close<TView>(UIType type = UIType.Panel, Transform parent = null)
        where TView : Component
    {
        EnsureInitialized();

        var presenterType = GetPresenterTypeFromView<TView>();
        if (presenterType == null)
        {
            Debug.LogError($"UIRouter.Close -> Failed to resolve Presenter type for view {typeof(TView).Name}");
            return;
        }

        var method = typeof(IUIManager).GetMethod("CloseUI")?.MakeGenericMethod(presenterType, typeof(TView));
        method?.Invoke(uiManager, new object[] { type, parent });
    }

    private static Type GetPresenterTypeFromView<TView>() where TView : Component
    {
        var baseType = typeof(TView).BaseType;

        while (baseType != null)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseUIView<>))
            {
                return baseType.GetGenericArguments()[0];
            }
            baseType = baseType.BaseType;
        }

        return null;
    }

    private static void EnsureInitialized()
    {
        if (uiManager == null)
        {
            Debug.LogError("UIRouter not initialized. Call UIRouter.Init(uiManager) before use.");
        }
    }
}


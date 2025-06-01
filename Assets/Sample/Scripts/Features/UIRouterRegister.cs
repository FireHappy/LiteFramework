#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using LiteFramework.Core.Module.UI;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using UnityEditor;
using UnityEngine;

namespace LiteFramework.Generated
{
    public static class UIRouterRegister
    {
        private static readonly System.Collections.Generic.Dictionary<(Type presenter, Type view), Action<IUIManager, UIType, Transform>> OpenDelegates = new();
        private static readonly System.Collections.Generic.Dictionary<(Type presenter, Type view), Action<IUIManager, UIType, Transform>> CloseDelegates = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register()
        {
            Debug.Log("⚙️ UIRouterRegister: 正在注册所有 UI 映射...");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.StartsWith("Unity") && !a.FullName.StartsWith("System"));

            var viewTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IView).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .ToList();

            foreach (var view in viewTypes)
            {
                var presenter = GetPresenterFromView(view);
                if (presenter == null)
                {
                    Debug.LogWarning($"跳过 View：{view.FullName}，未能解析 Presenter。");
                    continue;
                }

                // 注册映射委托
                OpenDelegates[(presenter, view)] = (mgr, type, parent) =>
                {
                    var method = typeof(IUIManager).GetMethod("OpenUI")?.MakeGenericMethod(presenter, view);
                    method?.Invoke(mgr, new object[] { type, parent });
                };

                CloseDelegates[(presenter, view)] = (mgr, type, parent) =>
                {
                    var method = typeof(IUIManager).GetMethod("CloseUI")?.MakeGenericMethod(presenter, view);
                    method?.Invoke(mgr, new object[] { type, parent });
                };

                Debug.Log($"注册成功：Presenter={presenter.FullName}，View={view.FullName}");
            }
        }

        public static bool TryOpen(IUIManager manager, Type presenter, Type view, UIType uiType = UIType.Panel, Transform parent = null)
        {
            if (OpenDelegates.TryGetValue((presenter, view), out var action))
            {
                action(manager, uiType, parent);
                return true;
            }
            Debug.LogWarning($"未找到 Open 映射：Presenter={presenter.FullName}，View={view.FullName}");
            return false;
        }

        public static bool TryClose(IUIManager manager, Type presenter, Type view, UIType uiType = UIType.Panel, Transform parent = null)
        {
            if (CloseDelegates.TryGetValue((presenter, view), out var action))
            {
                action(manager, uiType, parent);
                return true;
            }
            Debug.LogWarning($"未找到 Close 映射：Presenter={presenter.FullName}，View={view.FullName}");
            return false;
        }

        private static Type GetPresenterFromView(Type viewType)
        {
            var attr = viewType.GetCustomAttribute<BindPresenterAttribute>();
            if (attr != null)
                return attr.PresenterType;

            var baseType = viewType.BaseType;
            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseUIView<>))
                    return baseType.GetGenericArguments()[0];

                baseType = baseType.BaseType;
            }
            return null;
        }
    }
}
#endif

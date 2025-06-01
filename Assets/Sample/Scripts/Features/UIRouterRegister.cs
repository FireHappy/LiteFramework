// 自动生成，请勿手动修改
using System;
using System.Collections.Generic;
using LiteFramework.Module.UI;
using UnityEngine;

namespace LiteFramework.Generated
{
    public static class UIRouterRegister
    {
        private static readonly Dictionary<(Type, Type), Action<IUIManager, UIType, Transform>> OpenDelegates = new();
        private static readonly Dictionary<(Type, Type), Action<IUIManager, UIType, Transform>> CloseDelegates = new();

        [RuntimeInitializeOnLoadMethod]
        static void Register()
        {
            UIRouter.Register<LiteFramework.Sample.LoginPresenter, LiteFramework.Sample.LoginView>();
            OpenDelegates[(typeof(LiteFramework.Sample.LoginPresenter), typeof(LiteFramework.Sample.LoginView))] = (mgr, type, parent) => mgr.OpenUI<LiteFramework.Sample.LoginPresenter, LiteFramework.Sample.LoginView>(type, parent);
            CloseDelegates[(typeof(LiteFramework.Sample.LoginPresenter), typeof(LiteFramework.Sample.LoginView))] = (mgr, type, parent) => mgr.CloseUI<LiteFramework.Sample.LoginPresenter, LiteFramework.Sample.LoginView>(type, parent);
            UIRouter.Register<LiteFramework.Sample.MainPresenter, LiteFramework.Sample.MainView>();
            OpenDelegates[(typeof(LiteFramework.Sample.MainPresenter), typeof(LiteFramework.Sample.MainView))] = (mgr, type, parent) => mgr.OpenUI<LiteFramework.Sample.MainPresenter, LiteFramework.Sample.MainView>(type, parent);
            CloseDelegates[(typeof(LiteFramework.Sample.MainPresenter), typeof(LiteFramework.Sample.MainView))] = (mgr, type, parent) => mgr.CloseUI<LiteFramework.Sample.MainPresenter, LiteFramework.Sample.MainView>(type, parent);
            UIRouter.Register<LiteFramework.Sample.SettingPresenter, LiteFramework.Sample.SettingView>();
            OpenDelegates[(typeof(LiteFramework.Sample.SettingPresenter), typeof(LiteFramework.Sample.SettingView))] = (mgr, type, parent) => mgr.OpenUI<LiteFramework.Sample.SettingPresenter, LiteFramework.Sample.SettingView>(type, parent);
            CloseDelegates[(typeof(LiteFramework.Sample.SettingPresenter), typeof(LiteFramework.Sample.SettingView))] = (mgr, type, parent) => mgr.CloseUI<LiteFramework.Sample.SettingPresenter, LiteFramework.Sample.SettingView>(type, parent);
            UIRouter.Register<LiteFramework.Sample.DummyPresenter, LiteFramework.Sample.DummyView>();
            OpenDelegates[(typeof(LiteFramework.Sample.DummyPresenter), typeof(LiteFramework.Sample.DummyView))] = (mgr, type, parent) => mgr.OpenUI<LiteFramework.Sample.DummyPresenter, LiteFramework.Sample.DummyView>(type, parent);
            CloseDelegates[(typeof(LiteFramework.Sample.DummyPresenter), typeof(LiteFramework.Sample.DummyView))] = (mgr, type, parent) => mgr.CloseUI<LiteFramework.Sample.DummyPresenter, LiteFramework.Sample.DummyView>(type, parent);
        }

        public static bool TryGetOpenDelegate(Type presenter, Type view, out Action<IUIManager, UIType, Transform> del)
            => OpenDelegates.TryGetValue((presenter, view), out del);

        public static bool TryGetCloseDelegate(Type presenter, Type view, out Action<IUIManager, UIType, Transform> del)
            => CloseDelegates.TryGetValue((presenter, view), out del);
    }
}

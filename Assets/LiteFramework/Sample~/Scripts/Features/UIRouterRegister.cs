// 自动生成，请勿手动修改
using System;
using System.Collections.Generic;
using LiteFramework.Module.UI;
using UnityEngine;

namespace LiteFramework.Generated
{
    public static class UIRouterRegister
    {
        [RuntimeInitializeOnLoadMethod]
        static void Register()
        {
            UIRouter.Register<LiteFramework.Sample.LoginPresenter, LiteFramework.Sample.LoginView>();
            UIRouter.Register<LiteFramework.Sample.MainPresenter, LiteFramework.Sample.MainView>();
            UIRouter.Register<LiteFramework.Sample.SettingPresenter, LiteFramework.Sample.SettingView>();
            UIRouter.Register<LiteFramework.Sample.DummyPresenter, LiteFramework.Sample.DummyView>();
        }
    }
}

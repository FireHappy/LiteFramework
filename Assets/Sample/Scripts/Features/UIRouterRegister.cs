// 自动生成，请勿手动修改
using System;
using System.Collections.Generic;
using LiteFramework.Module.UI;
using UnityEngine;

namespace LiteFramework.Sample
{
    public static class UIRouterRegister
    {
        [RuntimeInitializeOnLoadMethod]
        static void Register()
        {
            UIRouter.Register<LiteFramework.Sample.LoginPresenter, LiteFramework.Sample.LoginView>();
        }
    }
}

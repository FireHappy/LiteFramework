// 自动生成，请勿手动修改
using LiteFramework.Core.Module.UI;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;

namespace LiteFramework.Generated
{
    public static class UIRouterMappings
    {
        static UIRouterMappings()
        {
            UIRouter.Register<LiteFramework.Sample.LoginPresenter, LiteFramework.Sample.LoginView>();
            UIRouter.Register<LiteFramework.Sample.MainPresenter, LiteFramework.Sample.MainView>();
            UIRouter.Register<LiteFramework.Sample.SettingPresenter, LiteFramework.Sample.SettingView>();
            UIRouter.Register<LiteFramework.Sample.DummyPresenter, LiteFramework.Sample.DummyView>();
        }
    }
}

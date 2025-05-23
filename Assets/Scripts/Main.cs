using VContainer;
using VContainer.Unity;
using LFramework.Module.UI;
using LFramework.Core.Utility;
using LFramework.Core.MVP;

namespace LFramework.Demo
{
    public class Main : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // 注册 UIManager 和其它服务
            builder.Register<IUIManager, UIManager>(Lifetime.Singleton);

            // 自动注册所有带 [AutoRegister] 的 Presenter
            var assemblies = new[] { typeof(BasePresenter<>).Assembly };
            VContainerAutoRegister.RegisterWithAttribute(builder, assemblies);

            // 也可以注册其它 Service、Model 等
        }

        private void Start()
        {
            // 在容器启动后安全地访问已注入的服务
            var uiManager = Container.Resolve<IUIManager>();

            // 加载 Login UI
            uiManager.OpenUI<LoginPresenter, LoginView>(UIType.Panel);
        }
    }
}




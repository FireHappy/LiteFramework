using VContainer;
using VContainer.Unity;
using System.Reflection;
using System.Linq;

public class Main : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // 注册 UIManager 和其它服务
        builder.Register<IUIManager, UIManager>(Lifetime.Singleton);

        // 自动注册
        var assemblies = new[] { typeof(BasePresenter<>).Assembly };
        VContainerAutoRegister.RegisterWithAttribute(builder, assemblies);

        // 可选：注册 Model、NetModule 等其他模块
    }

}

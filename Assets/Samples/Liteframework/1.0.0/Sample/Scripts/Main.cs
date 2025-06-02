using VContainer;
using VContainer.Unity;
using LiteFramework.Module.UI;
using LiteFramework.Core.Utility;
using LiteFramework.Core.MVP;
using LiteFramework.Configs;
using UnityEngine;

namespace LiteFramework.Sample
{
    public class Main : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterUIModule(builder);
            RegisterAllAutoRegister(builder);
        }

        /// <summary>
        /// RegisterUIModule
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterUIModule(IContainerBuilder builder)
        {
            var configFromAsset = Resources.Load<UIRootConfig>("Configs/UIRootConfig");
            builder.RegisterInstance(configFromAsset).As<UIRootConfig>();
            builder.Register<IUIManager, UIManager>(Lifetime.Singleton);
            builder.Register<UIRouter>(Lifetime.Singleton);
        }

        /// <summary>
        /// AutoRegister
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterAllAutoRegister(IContainerBuilder builder)
        {
            var assemblies = new[] { typeof(BasePresenter<>).Assembly,
             typeof(LoginPresenter).Assembly };
            VContainerAutoRegister.RegisterWithAttribute(builder, assemblies);
        }


        private void Start()
        {
            //Start Logic
            var uiRouter = Container.Resolve<UIRouter>();
            uiRouter.Open<LoginView>();
        }
    }
}




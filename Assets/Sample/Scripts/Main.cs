using System.Reflection;
using LiteFramework.Module.UI;
using VContainer;


namespace LiteFramework.Sample
{
    public class Main : LiteStartupBase
    {

        protected override void OnRegisterCustomServices(IContainerBuilder builder)
        {
            //todo 注册自定义服务
        }

        protected override void OnStart()
        {
            //使用VContainer容器获取ui路由
            // var router = Container.Resolve<UIRouter>();
            // router.Open<LoginView>();
        }
    }
}




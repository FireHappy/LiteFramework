using LiteFramework.Core.Utility;
using LiteFramework.Module.UI;
using VContainer;

namespace LiteFramework.Sample
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class LoginPresenter : BaseUIPresenter<LoginView>
    {

        public LoginPresenter(UIRouter router, IObjectResolver container) : base(router, container)
        {

        }

        public override void OnViewCreate()
        {

        }

        public override void OnViewShow()
        {

        }

        public override void OnViewHide()
        {

        }

        public override void OnViewDispose()
        {

        }
    }
}



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

        protected override void OnViewReady()
        {
            view.btnLogin.onClick.AddListener(() =>
            {
                router.Open<MainView>();
            });

            view.btnCancel.onClick.AddListener(() =>
            {
                router.Close<LoginView>();
            });
        }

        protected override void OnViewDispose()
        {
            view.btnLogin.onClick.RemoveAllListeners();
            view.btnCancel.onClick.RemoveAllListeners();
        }
    }
}



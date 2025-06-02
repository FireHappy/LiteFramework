using LiteFramework.Core.Utility;
using LiteFramework.Module.UI;
using VContainer;
using LiteFramework.Core.Module.UI;

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
            UserModel userModel = container.Resolve<UserModel>();

            view.inputPassword.onValueChanged.AddListener(value =>
            {
                userModel.password = value;
            });

            view.inputUserName.onValueChanged.AddListener(value =>
            {
                userModel.userName.Value = value;
            });

            view.btnLogin.onClick.AddListener(() =>
            {
                router.Open<MainView>();
            });
        }

        protected override void OnViewDispose()
        {
            view.btnLogin.onClick.RemoveAllListeners();
            view.inputUserName.onValueChanged.RemoveAllListeners();
            view.inputPassword.onValueChanged.RemoveAllListeners();
        }
    }

}


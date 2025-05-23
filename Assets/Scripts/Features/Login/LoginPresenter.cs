using LFramework.Core.Utility;
using LFramework.Core.MVP;
using LFramework.Module.UI;
using VContainer;


namespace LFramework.Demo
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class LoginPresenter : BasePresenter<LoginView>
    {
        UserModel userModel;
        public LoginPresenter(UserModel userModel, IObjectResolver container) : base(container)
        {
            this.userModel = userModel;
        }
        internal void OnLoginButtonClicked(string userName, string password)
        {
            this.userModel.userName = userName;
            this.userModel.password = password;
            var uiManager = Container.Resolve<IUIManager>();
            uiManager.OpenUI<MainPresenter, MainView>(UIType.Panel);
        }
    }
}


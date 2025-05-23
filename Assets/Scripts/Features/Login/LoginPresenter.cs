using LFramework.Core.Utility;
using LFramework.Core.MVP;
using LFramework.Module.UI;
using VContainer;
using UnityEngine;


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
        protected override void OnViewReady()
        {
            View.loginButton.onClick.AddListener(() =>
            {
                this.userModel.userName = View.nameInput.text;
                this.userModel.password = View.passwordInput.text;
                var uiManager = Container.Resolve<IUIManager>();
                uiManager.OpenUI<MainPresenter, MainView>(UIType.Panel);
            });
        }

        private void OnDispose()
        {
            View.loginButton.onClick.RemoveAllListeners();
        }
    }
}


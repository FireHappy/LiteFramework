using LFramework.Core.Utility;
using LFramework.Core.MVP;
using VContainer;
using LFramework.Module.UI;

namespace LFramework.Demo
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class MainPresenter : BasePresenter<MainView>
    {
        private UserModel userModel;
        public MainPresenter(UserModel userModel, IObjectResolver container) : base(container)
        {
            this.userModel = userModel;
        }
        protected override void OnViewReady()
        {
            var uiManager = Container.Resolve<IUIManager>();
            View.userName.text = userModel.userName;
            View.settingButton.onClick.AddListener(() =>
            {
                //todo open setting view

            });
            View.exitButton.onClick.AddListener(() =>
            {
                uiManager.CloseUI<MainPresenter, MainView>(UIType.Panel);
            });
        }
    }
}



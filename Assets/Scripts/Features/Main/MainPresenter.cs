using LiteFramework.Core.Utility;
using LiteFramework.Core.MVP;
using VContainer;
using LiteFramework.Module.UI;
using UniRx;

namespace LiteFramework.Demo
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
            // 将 Model 的 userName 绑定到 UI 显示
            userModel.userName
                .Subscribe(value => View.userName.text = value)
                .AddTo(View); // 自动在 View 销毁时取消订阅

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



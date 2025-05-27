using LiteFramework.Core.Utility;
using LiteFramework.Module.UI;
using LiteFramework.Core.Module.UI;
using VContainer;
using UnityEngine.UIElements.Experimental;
using UniRx;

namespace LiteFramework.Sample
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class MainPresenter : BaseUIPresenter<MainView>
    {

        public MainPresenter(IUIManager uiManager, IObjectResolver container) : base(uiManager, container)
        {

        }

        protected override void OnViewReady()
        {
            UserModel userModel = container.Resolve<UserModel>();
            view.btnExit.onClick.AddListener(() =>
            {
                uiManager.CloseUI<MainPresenter, MainView>();
            });
            //使用UniRx绑定按钮事件
            view.btnSetting.OnClickAsObservable().Subscribe(_ =>
            {
                //todo 打开设置界面
            }).AddTo(view);
            //使用UniRx绑定数据的变化
            userModel.userName.Subscribe(value => view.txtuUserName.text = value).AddTo(view);
        }

        protected override void OnViewDispose()
        {

        }
    }
}



using LiteFramework.Core.Utility;
using LiteFramework.Module.UI;
using LiteFramework.Core.Module.UI;
using VContainer;
using UniRx;

namespace LiteFramework.Sample
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class MainPresenter : BaseUIPresenter<MainView>
    {

        public MainPresenter(UIRouter router, IObjectResolver container) : base(router, container)
        {

        }

        protected override void OnViewReady()
        {
            UserModel userModel = container.Resolve<UserModel>();
            view.btnExit.onClick.AddListener(() =>
            {
                router.Close<MainView>();
            });
            //使用UniRx绑定按钮事件
            view.btnSetting.OnClickAsObservable().Subscribe(_ =>
            {
                router.Open<MainView>();
            }).AddTo(view);
            //使用UniRx绑定数据的变化
            userModel.userName.Subscribe(value => view.txtuUserName.text = value).AddTo(view);
        }

        protected override void OnViewDispose()
        {

        }
    }
}



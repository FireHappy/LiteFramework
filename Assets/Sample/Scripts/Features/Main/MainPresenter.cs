using LiteFramework.Core.Utility;
using LiteFramework.Module.UI;
using VContainer;

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
            view.btnSetting.onClick.AddListener(() =>
            {
                router.Open<SettingView>(UIType.Dialog);
            });
            view.btnExit.onClick.AddListener(() =>
            {
                router.Close<MainView>();
            });
        }

        protected override void OnViewDispose()
        {

        }
    }
}



using LiteFramework.Core.Utility;
using LiteFramework.Module.UI;
using Unity.VisualScripting;
using VContainer;

namespace LiteFramework.Sample
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class SettingPresenter : BaseUIPresenter<SettingView>
    {

        public SettingPresenter(UIRouter router, IObjectResolver container) : base(router, container)
        {

        }

        protected override void OnViewReady()
        {
            view.btnExit.onClick.AddListener(() =>
            {
                router.Close<SettingView>(UIType.Dialog);
            });
        }

        protected override void OnViewDispose()
        {

        }
    }
}



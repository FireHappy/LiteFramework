using LiteFramework.Core.Utility;
using LiteFramework.Module.UI;
using LiteFramework.Core.Module.UI;
using VContainer;

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
            view.btnExit.onClick.AddListener(() =>
            {
                uiManager.CloseUI<MainPresenter, MainView>();
            });
        }

        protected override void OnViewDispose()
        {

        }
    }
}



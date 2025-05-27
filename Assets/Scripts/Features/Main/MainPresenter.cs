using LiteFramework.Core.Utility;


using LiteFramework.Core.Module.UI;
using VContainer;
using LiteFramework.Module.UI;

[AutoRegister(VContainer.Lifetime.Transient)]
public class MainPresenter : BaseUIPresenter<MainView>
{

    public MainPresenter(UIManager uiManager, IObjectResolver container) : base(uiManager, container)
    {

    }

    protected override void OnViewReady()
    {

    }

    protected override void OnViewDispose()
    {

    }
}


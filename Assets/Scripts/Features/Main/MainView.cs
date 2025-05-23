using UnityEngine.UI;
using LFramework.Utility;

public class MainView : BaseView<MainPresenter>
{
    [Autowrited] Button settingButton;
    [Autowrited] Button exitButton;
    [Autowrited] Text userName;

    protected override void OnBind()
    {

    }

}

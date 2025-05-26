using UnityEngine.UI;
using LiteFramework.Utility;
using LiteFramework.Core.MVP;
using TMPro;

namespace LiteFramework.Demo
{
    public class MainView : BaseView<MainPresenter>
    {
        [Autowrited] public Button settingButton;
        [Autowrited] public Button exitButton;
        [Autowrited] public TMP_Text userName;
    }
}



using UnityEngine.UI;
using LFramework.Utility;
using LFramework.Core.MVP;
using TMPro;

namespace LFramework.Demo
{
    public class MainView : BaseView<MainPresenter>
    {
        [Autowrited] public Button settingButton;
        [Autowrited] public Button exitButton;
        [Autowrited] public TMP_Text userName;
    }
}



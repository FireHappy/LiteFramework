using UnityEngine.UI;
using LFramework.Utility;
using LFramework.Core.MVP;

namespace LFramework.Demo
{
    public class MainView : BaseView<MainPresenter>
    {
        [Autowrited] Button settingButton;
        [Autowrited] Button exitButton;
        [Autowrited] Text userName;

        protected override void OnBind()
        {
            exitButton.onClick.AddListener(() =>
            {

            });
        }

    }
}



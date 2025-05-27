using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Utility;
using LiteFramework.Core.Module.UI;
using TMPro;
namespace LiteFramework.Sample
{
    public partial class MainView : BaseUIView<MainPresenter>
    {
        [Autowrited("btn_setting")] public Button btnSetting;
        [Autowrited("btn_exit")] public Button btnExit;

    }
}



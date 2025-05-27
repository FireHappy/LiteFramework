using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Utility;
using LiteFramework.Core.Module.UI;
using TMPro;
namespace LiteFramework.Sample
{
    public partial class SettingView : BaseUIView<SettingPresenter>
    {
		[Autowrited("input_username")] public TMP_InputField inputUserName;
		[Autowrited("input_password")] public TMP_InputField inputPassword;
		[Autowrited("btn_change")] public Button btnChange;
		[Autowrited("btn_exit")] public Button btnExit;

    }
}



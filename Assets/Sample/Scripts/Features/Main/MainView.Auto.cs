using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Module.UI;
using TMPro;
using LiteFramework.Core.Utility;
namespace LiteFramework.Sample
{
	public partial class MainView : BaseUIView<MainPresenter>
	{
		[Autowrited("txtu_username")] public TextMeshProUGUI txtuUserName;
		[Autowrited("btn_setting")] public Button btnSetting;
		[Autowrited("btn_exit")] public Button btnExit;

	}
}



using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Utility;
using LiteFramework.Core.Module.UI;
using TMPro;
namespace LiteFramework.Sample
{
    public partial class MainView : BaseUIView<MainPresenter>
    {
		public TextMeshProUGUI txtuUserName;
		public Button btnSetting;
		public Button btnExit;

        public override void InitComponents()
        {
			txtuUserName = transform.Find("TxtU_UserName").GetComponent<TextMeshProUGUI>();
			btnSetting = transform.Find("Btn_Setting").GetComponent<Button>();
			btnExit = transform.Find("Btn_Exit").GetComponent<Button>();

        }
    }
}



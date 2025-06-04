using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Module.UI;
using TMPro;
namespace LiteFramework.Sample
{
    public partial class MainView : BaseUIView<MainPresenter>
    {
		public TextMeshProUGUI txtUUserName;
		public Button btnSetting;
		public Button btnExit;

        public override void FindComponents()
        {
			txtUUserName = transform.Find("TxtU_UserName").GetComponent<TextMeshProUGUI>();
			btnSetting = transform.Find("Btn_Setting").GetComponent<Button>();
			btnExit = transform.Find("Btn_Exit").GetComponent<Button>();

        }
    }
}



using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Utility;
using LiteFramework.Core.Module.UI;
using TMPro;
namespace LiteFramework.Sample
{
    public partial class SettingView : BaseUIView<SettingPresenter>
    {
		public TMP_InputField inputUserName;
		public TMP_InputField inputPassword;
		public Button btnChange;
		public Button btnExit;

        public override void InitComponents()
        {
			inputUserName = transform.Find("Bg/Input_UserName").GetComponent<TMP_InputField>();
			inputPassword = transform.Find("Bg/Input_Password").GetComponent<TMP_InputField>();
			btnChange = transform.Find("Bg/Btn_Change").GetComponent<Button>();
			btnExit = transform.Find("Bg/Btn_Exit").GetComponent<Button>();

        }
    }
}



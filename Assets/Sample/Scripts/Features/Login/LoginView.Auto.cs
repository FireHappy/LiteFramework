using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Module.UI;
using TMPro;
namespace LiteFramework.Sample
{
    public partial class LoginView : BaseUIView<LoginPresenter>
    {
		public TMP_InputField inputUserName;
		public TMP_InputField inputPassword;
		public Button btnLogin;
		public Button btnCancel;

        public override void FindComponents()
        {
			inputUserName = transform.Find("Input_UserName").GetComponent<TMP_InputField>();
			inputPassword = transform.Find("Input_Password").GetComponent<TMP_InputField>();
			btnLogin = transform.Find("Btn_Login").GetComponent<Button>();
			btnCancel = transform.Find("Btn_Cancel").GetComponent<Button>();

        }
    }
}



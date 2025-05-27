using UnityEngine.UI;
using LiteFramework.Utility;
using TMPro;
using LiteFramework.Core.Module.UI;

public partial class LoginView : BaseUIView<LoginPresenter>
{
	[Autowrited("input_username")] public TMP_InputField inputUserName;
	[Autowrited("input_password")] public TMP_InputField inputPassword;
	[Autowrited("btn_login")] public Button btnLogin;
	[Autowrited("btn_cancel")] public Button btnCancel;

}


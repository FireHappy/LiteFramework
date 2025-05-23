using UnityEngine.UI;
using LFramework.Utility;
using LFramework.Core.MVP;
using TMPro;
namespace LFramework.Demo
{
    public class LoginView : BaseView<LoginPresenter>
    {
        [Autowrited] public Button loginButton;
        [Autowrited] public TMP_InputField nameInput;
        [Autowrited] public TMP_InputField passwordInput;
    }
}



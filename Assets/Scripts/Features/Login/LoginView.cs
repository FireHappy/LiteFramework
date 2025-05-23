using UnityEngine.UI;
using LFramework.Utility;
using LFramework.Core.MVP;
using TMPro;
namespace LFramework.Demo
{
    public class LoginView : BaseView<LoginPresenter>
    {
        [Autowrited] private Button loginButton;
        [Autowrited] private TMP_InputField nameInput;
        [Autowrited] private TMP_InputField passwordInput;

        protected override void OnBind()
        {
            // 绑定点击事件给 Presenter
            loginButton.onClick.AddListener(() =>
            {
                presenter.OnLoginButtonClicked(nameInput.text, passwordInput.text);
            });
        }
    }

}



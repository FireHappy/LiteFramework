using UnityEngine.UI;
using LFramework.Utility;

public class LoginView : BaseView<LoginPresenter>
{
    [Autowrited] private Button loginButton;
    [Autowrited] private InputField userNameInputFiled;
    [Autowrited] private InputField passwordInputFiled;

    protected override void OnBind()
    {
        // 绑定点击事件给 Presenter
        loginButton.onClick.AddListener(() =>
        {
            presenter.OnLoginButtonClicked(userNameInputFiled.text, passwordInputFiled.text);
        });
    }

}

using UnityEngine;
using VContainer;

[AutoRegister(VContainer.Lifetime.Transient)]
public class MainPresenter : BasePresenter<MainView>
{
    UserModel userModel;
    public MainPresenter(UserModel userModel, IObjectResolver container) : base(container)
    {
        this.userModel = userModel;
        Debug.Log($"UserName: {userModel.userName}");
    }
}

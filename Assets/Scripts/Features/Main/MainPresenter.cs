using LFramework.Core.Utility;
using LFramework.Core.MVP;
using UnityEngine;
using VContainer;

namespace LFramework.Demo
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class MainPresenter : BasePresenter<MainView>
    {
        UserModel userModel;
        public MainPresenter(UserModel userModel, IObjectResolver container) : base(container)
        {
            this.userModel = userModel;
            Debug.Log($"UserName: {userModel.userName}");
        }

        public void OnExitButtonClick()
        {
            //todo...
        }
    }
}



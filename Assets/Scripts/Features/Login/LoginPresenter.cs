using LiteFramework.Core.Utility;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using VContainer;
using UnityEngine;

[AutoRegister(VContainer.Lifetime.Transient)]
public class LoginPresenter : BasePresenter<LoginView>
{

    public LoginPresenter(IObjectResolver container) : base(container)
    {
        
    }

    protected override void OnViewReady()
    {

    }
}


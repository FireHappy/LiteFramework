using LiteFramework.Core.Utility;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using LiteFramework.Core.Module.UI;
using VContainer;
using UnityEngine;
using UniRx;

namespace LiteFramework.Sample
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class SettingPresenter : BaseUIPresenter<SettingView>
    {

        public SettingPresenter(UIRouter router, IObjectResolver container) : base(router, container)
        {

        }

        protected override void OnViewReady()
        {
            view.btnExit.OnClickAsObservable().Subscribe(_ =>
            {
                router.Close<SettingView>(UIType.Dialog);
            }).AddTo(view);
            view.btnChange.OnClickAsObservable().Subscribe(_ =>
            {
                UserModel userModel = container.Resolve<UserModel>();
                userModel.userName.Value = view.inputUserName.text;
                userModel.password = view.inputPassword.text;
                router.Close<SettingView>(UIType.Dialog);
            }).AddTo(view);
        }

        protected override void OnViewDispose()
        {

        }
    }
}



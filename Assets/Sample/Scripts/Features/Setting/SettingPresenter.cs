using LiteFramework.Core.Utility;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using LiteFramework.Core.Module.UI;
using VContainer;
using UnityEngine;

namespace LiteFramework.Sample
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class SettingPresenter : BaseUIPresenter<SettingView>
    {

        public SettingPresenter(IUIManager uiManager, IObjectResolver container) : base(uiManager, container)
        {

        }

        protected override void OnViewReady()
        {

        }

        protected override void OnViewDispose()
        {

        }
    }
}



using LiteFramework.Core.Utility;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using LiteFramework.Core.Module.UI;
using VContainer;
using UnityEngine;

namespace LiteFramework.Sample
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class MainPresenter : BaseUIPresenter<MainView>
    {

        public MainPresenter(UIManager uiManager, IObjectResolver container) : base(uiManager, container)
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



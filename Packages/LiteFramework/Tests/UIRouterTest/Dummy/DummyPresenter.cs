using LiteFramework.Core.Utility;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using LiteFramework.Core.Module.UI;
using VContainer;
using UnityEngine;

namespace LiteFramework.Tests
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class DummyPresenter : BaseUIPresenter<DummyView>
    {

        public DummyPresenter(UIRouter uiRouter, IObjectResolver container) : base(uiRouter, container)
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



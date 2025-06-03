using LiteFramework.Core.Utility;
using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using LiteFramework.Core.Module.UI;
using VContainer;
using UnityEngine;

namespace LiteFramework.Sample
{
    [AutoRegister(VContainer.Lifetime.Transient)]
    public class TipPresenter : BaseUIPresenter<TipView>
    {

        public TipPresenter(UIRouter router, IObjectResolver container) : base(router, container)
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



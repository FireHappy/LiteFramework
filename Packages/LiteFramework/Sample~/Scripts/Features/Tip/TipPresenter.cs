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
        IModel tipModel;
        public TipPresenter(IUIManager uiManager, IObjectResolver container, IModel tipModel) : base(uiManager, container)
        {
            this.tipModel = tipModel;
        }

        protected override void OnViewReady()
        {

        }

        protected override void OnViewDispose()
        {

        }
    }
}



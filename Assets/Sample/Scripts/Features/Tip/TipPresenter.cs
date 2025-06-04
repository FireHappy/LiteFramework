using LiteFramework.Core.Utility;
using LiteFramework.Module.UI;
using VContainer;

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



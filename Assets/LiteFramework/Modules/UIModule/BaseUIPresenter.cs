using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using VContainer;

namespace LiteFramework.Core.Module.UI
{
    public abstract class BaseUIPresenter<TView> : IPresenter
        where TView : IView
    {
        protected TView view;
        protected readonly IObjectResolver container;
        protected readonly UIRouter router;


        protected BaseUIPresenter(UIRouter router, IObjectResolver container)
        {
            this.container = container;
            this.router = router;
        }

        public void AttachView(IView view)
        {
            this.view = (TView)view;
            OnViewReady();
        }

        public void DetachView()
        {
            view = default;
            OnViewDispose();
        }

        protected virtual void OnViewReady() { }

        protected virtual void OnViewDispose() { }
    }
}


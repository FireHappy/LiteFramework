using LiteFramework.Core.MVP;
using LiteFramework.Module.UI;
using VContainer;

namespace LiteFramework.Core.Module.UI
{
    public abstract class BaseUIPresenter<TView> : IPresenter
        where TView : IView
    {
        protected TView view;
        private IUIManager uiManager;
        protected readonly IObjectResolver container;
        protected readonly UIRouter router;


        protected BaseUIPresenter(UIRouter router, IObjectResolver container)
        {
            this.container = container;
            this.router = router;
        }

        protected BaseUIPresenter(IUIManager uiManager, IObjectResolver container)
        {
            this.uiManager = uiManager;
            this.container = container;
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


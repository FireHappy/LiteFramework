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
        protected readonly IUIManager uiManager;

        protected BaseUIPresenter(IUIManager uiManager, IObjectResolver container)
        {
            this.container = container;
            this.uiManager = uiManager;
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


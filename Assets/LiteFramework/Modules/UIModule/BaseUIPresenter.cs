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

        protected BaseUIPresenter(IUIManager manager, IObjectResolver container)
        {
            this.container = container;
            uiManager = manager;
        }

        public virtual void AttachView(IView view)
        {
            this.view = (TView)view;
            OnViewReady();
        }

        public virtual void DetachView()
        {
            view = default;
        }

        protected virtual void OnViewReady() { }

        public virtual void Dispose() { }
    }
}


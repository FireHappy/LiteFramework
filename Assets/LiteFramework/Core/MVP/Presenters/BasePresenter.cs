using VContainer;

namespace LiteFramework.Core.MVP
{
    public abstract class BasePresenter<TView> : IPresenter
        where TView : IView
    {
        protected TView View;
        protected readonly IObjectResolver Container;

        protected BasePresenter(IObjectResolver container)
        {
            Container = container;
        }

        public virtual void AttachView(IView view)
        {
            View = (TView)view;
            OnViewReady();
        }

        public virtual void DetachView()
        {
            View = default;
        }

        protected virtual void OnViewReady() { }

        public virtual void Dispose() { }
    }
}


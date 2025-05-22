public abstract class BasePresenter<TView> : IPresenter
    where TView : IView
{
    protected TView View;

    public virtual void SetView(TView view)
    {
        View = view;
        OnViewReady();
    }

    public virtual void DetachView()
    {
        View = default;
    }

    protected virtual void OnViewReady() { }

    public virtual void Dispose() { }
}
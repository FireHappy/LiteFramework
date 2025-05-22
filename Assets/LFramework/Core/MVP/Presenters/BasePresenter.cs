public abstract class BasePresenter<TView> where TView : IView
{
    protected TView View;

    public BasePresenter(TView view)
    {
        View = view;
        OnInit();
    }

    protected abstract void OnInit();
}
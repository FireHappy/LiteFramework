

namespace LiteFramework.Core.MVP
{
    public interface IView : System.IDisposable
    {
        void BindPresenter(IPresenter presenter);
        void UnBindPresenter();
    }
}



namespace LiteFramework.Core.MVP
{
    public interface IPresenter : System.IDisposable
    {
        void AttachView(IView view);
        void DetachView();
    }
}

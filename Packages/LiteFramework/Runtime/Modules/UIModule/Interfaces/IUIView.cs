

using LiteFramework.Core.MVP;

namespace LiteFramework.Module.UI
{
    public interface IUIView : IView
    {
        void OnCreate();
        void OnShow();
        void OnHide();
        void OnDispose();
    }
}


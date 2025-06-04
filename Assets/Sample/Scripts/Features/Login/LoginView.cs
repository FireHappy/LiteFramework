using LiteFramework.Module.UI;

namespace LiteFramework.Sample
{
    [BindPresenter(typeof(LoginPresenter))]
    public partial class LoginView : BaseUIView<LoginPresenter>
    {
        public override void OnCreate()
        {
            presenter.OnViewCreate();
        }

        public override void OnShow()
        {
            presenter.OnViewShow();
        }

        public override void OnHide()
        {
            presenter.OnViewHide();
        }

        public override void OnDispose()
        {
            presenter.OnViewDispose();
        }
    }
}


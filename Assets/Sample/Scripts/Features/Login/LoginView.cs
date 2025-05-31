using LiteFramework.Core.Module.UI;
using LiteFramework.Core.MVP;

namespace LiteFramework.Sample
{
    [BindPresenter(typeof(LoginPresenter))]
    public partial class LoginView : BaseUIView<LoginPresenter>
    {
        protected override void OnBind()
        {
            //TODO Init
        }

        protected override void OnUnBind()
        {
            //TODO UnInit
        }
    }
}



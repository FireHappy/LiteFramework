using TMPro;
using LiteFramework.Module.UI;

namespace LiteFramework.Tests
{
    [BindPresenter(typeof(DummyPresenter))]
    public partial class DummyView : BaseUIView<DummyPresenter>
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


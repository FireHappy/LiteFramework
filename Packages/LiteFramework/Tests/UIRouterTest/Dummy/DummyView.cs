using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Module.UI;
using TMPro;

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


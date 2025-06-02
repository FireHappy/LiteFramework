using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Module.UI;
using TMPro;

namespace LiteFramework.Sample
{
    [BindPresenter(typeof(TipPresenter))]
    public partial class TipView : BaseUIView<TipPresenter>
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


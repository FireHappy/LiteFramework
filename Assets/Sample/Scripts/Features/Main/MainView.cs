using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Module.UI;
using TMPro;

namespace LiteFramework.Sample
{
    [BindPresenter(typeof(MainPresenter))]
    public partial class MainView : BaseUIView<MainPresenter>
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


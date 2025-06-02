using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Utility;
using LiteFramework.Core.Module.UI;
using TMPro;

namespace LiteFramework.Sample
{
    [BindPresenter(typeof(SettingPresenter))]
    public partial class SettingView : BaseUIView<SettingPresenter>
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


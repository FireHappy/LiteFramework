using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Module.UI;
using TMPro;
namespace LiteFramework.Sample
{
    public partial class TipView : BaseUIView<TipPresenter>
    {
		public TextMeshProUGUI txtUTipTitle;
		public TextMeshProUGUI txtUTipContent;

        public override void InitComponents()
        {
			txtUTipTitle = transform.Find("Bg/TxtU_TipTitle").GetComponent<TextMeshProUGUI>();
			txtUTipContent = transform.Find("Bg/TxtU_TipContent").GetComponent<TextMeshProUGUI>();

        }
    }
}



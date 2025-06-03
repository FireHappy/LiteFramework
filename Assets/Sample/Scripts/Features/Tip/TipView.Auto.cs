using UnityEngine;
using UnityEngine.UI;
using LiteFramework.Core.Module.UI;
using TMPro;
namespace LiteFramework.Sample
{
    public partial class TipView : BaseUIView<TipPresenter>
    {
		public TextMeshProUGUI txtuTipTitle;
		public TextMeshProUGUI txtuTipContent;

        public override void InitComponents()
        {
			txtuTipTitle = transform.Find("Bg/TxtU_TipTitle").GetComponent<TextMeshProUGUI>();
			txtuTipContent = transform.Find("Bg/TxtU_TipContent").GetComponent<TextMeshProUGUI>();

        }
    }
}



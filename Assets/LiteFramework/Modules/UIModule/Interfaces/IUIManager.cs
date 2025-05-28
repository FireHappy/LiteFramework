using LiteFramework.Core.Module.UI;
using UnityEngine;

namespace LiteFramework.Module.UI
{
    public interface IUIManager
    {
        public void OpenUI<TPresenter, TView>(UIType type = UIType.Panel, Transform parent = null)
            where TPresenter : BaseUIPresenter<TView>
            where TView : BaseUIView<TPresenter>;

        public void CloseUI<TPresenter, TView>(UIType type = UIType.Panel, Transform parent = null) where TPresenter : BaseUIPresenter<TView>
       where TView : BaseUIView<TPresenter>;
    }
}

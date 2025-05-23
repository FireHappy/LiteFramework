using LFramework.Core.MVP;
using UnityEngine;

namespace LFramework.Module.UI
{
    public interface IUIManager
    {
        public TPresenter OpenUI<TPresenter, TView>(UIType type, Transform parent = null)
            where TPresenter : BasePresenter<TView>
            where TView : BaseView<TPresenter>;

        public void CloseUI<TPresenter, TView>(UIType type, Transform parent = null) where TPresenter : BasePresenter<TView>
       where TView : BaseView<TPresenter>;
    }
}

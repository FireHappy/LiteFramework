
using UnityEngine;

public interface IUIManager
{
    public TPresenter OpenUI<TPresenter, TView>(UIType type, Transform parent = null)
        where TPresenter : BasePresenter<TView>
        where TView : BaseView<TPresenter>;

    public void CloseUI<TPresenter, TView>(UIType type, Transform parent = null) where TPresenter : BasePresenter<TView>
   where TView : BaseView<TPresenter>;

}
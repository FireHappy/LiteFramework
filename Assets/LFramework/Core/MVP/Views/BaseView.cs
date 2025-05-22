using UnityEngine;

public abstract class BaseView<TPresenter> : MonoBehaviour, IView
    where TPresenter : class, IPresenter
{
    public TPresenter presenter { get; private set; }

    public void BindPresenter(TPresenter presenter)
    {
        if (this.presenter != null)
        {
            Debug.LogWarning($"{GetType().Name} 已经绑定过 Presenter，重复绑定被忽略。");
            return;
        }

        if (presenter == null)
        {
            Debug.LogError($"试图为 {GetType().Name} 绑定空 Presenter。");
            return;
        }

        this.presenter = presenter;
        OnBind();
    }

    /// <summary>
    /// Presenter 绑定完成后执行，子类可重写进行初始化。
    /// </summary>
    protected virtual void OnBind() { }
}

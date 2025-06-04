using LiteFramework.Core.MVP;
using UnityEngine;

namespace LiteFramework.Module.UI
{
    public abstract class BaseUIView<TPresenter> : MonoBehaviour, IView, IUILifetime
    where TPresenter : class, IPresenter
    {
        public TPresenter presenter { get; private set; }

        public void BindPresenter(IPresenter presenter)
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

            this.presenter = (TPresenter)presenter;
            this.presenter.AttachView(this);
            OnBind();
        }

        public void UnBindPresenter()
        {
            presenter.DetachView();
            presenter = default;
            OnUnBind();
        }

        public virtual void InitComponents()
        {

        }

        protected virtual void OnBind() { }
        protected virtual void OnUnBind() { }

        public void OnCreate()
        {

        }

        public void OnShow()
        {

        }

        public void OnHide()
        {

        }

        public void OnDispose()
        {

        }
    }
}



using LiteFramework.Core.MVP;
using UnityEngine;

namespace LiteFramework.Core.Module.UI
{
    public abstract class BaseUIView<TPresenter> : MonoBehaviour, IView
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
            OnBind();
        }

        public void UnBindPresenter()
        {
            presenter = default;
            OnUnBind();
        }

        public virtual void InitComponents()
        {

        }

        protected virtual void OnBind() { }
        protected virtual void OnUnBind() { }
    }
}



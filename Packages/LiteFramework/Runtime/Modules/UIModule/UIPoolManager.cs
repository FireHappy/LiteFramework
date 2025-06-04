using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace LiteFramework.Module.UI
{
    public class UIPoolEntry
    {
        public IUIView IView;
        public Transform View;
        public float LastHideTime;

        public UIPoolEntry(Transform view)
        {
            View = view;
            IView = view.GetComponent<IUIView>();
            LastHideTime = Time.unscaledTime;
        }
    }


    public class UIPoolManager : ITickable
    {
        private Dictionary<Type, UIPoolEntry> uiPool = new Dictionary<Type, UIPoolEntry>();
        private Transform poolRoot;
        private float keepAliveTime;
        private Queue<Type> removeQueue = new Queue<Type>();

        public UIPoolManager(float keepAliveTime)
        {
            this.keepAliveTime = keepAliveTime;
            InitPoolRoot();
        }

        public void Tick()
        {
            float now = Time.unscaledTime;
            foreach (var kvp in uiPool)
            {
                if (now - kvp.Value.LastHideTime > keepAliveTime)
                {
                    kvp.Value.IView.OnDispose();
                    GameObject.Destroy(kvp.Value.View.gameObject);
                    removeQueue.Enqueue(kvp.Key);
                }
            }
            while (removeQueue.Count > 0)
            {
                Type type = removeQueue.Dequeue();
                //todo...
                // uiPool.Remove();
                // var view = tsf.GetComponent<>();
                // if (view != null && view.presenter != null)
                // {
                //     view.presenter.DetachView();
                //     view.UnBindPresenter();
                // }
                // UIUtility.DestroyUI(tsf);
            }
        }

        public void RecycleUI<TView>(Transform view) where TView : IUIView
        {
            view.transform.SetParent(poolRoot);
            UIPoolEntry entry = new UIPoolEntry(view);
            entry.IView.OnHide();
            uiPool[typeof(TView)] = new UIPoolEntry(view);
        }

        public bool TryGetFromPool<TView>(out Transform view)
        {
            if (uiPool.TryGetValue(typeof(TView), out UIPoolEntry entry))
            {
                uiPool.Remove(typeof(TView));
                view = entry.View;
                return true;
            }
            view = null;
            return false;
        }

        private void InitPoolRoot()
        {
            var go = new GameObject("UIPoolRoot");
            var canvasGroup = go.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            poolRoot = go.transform;
            GameObject.DontDestroyOnLoad(go);
        }
    }
}

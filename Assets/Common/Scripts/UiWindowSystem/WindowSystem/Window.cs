using DG.Tweening;
using System;
using UnityEngine;

namespace Ui.WindowSystem
{
    public class Window : MonoBehaviour
    {
        public bool IsActive => gameObject.activeInHierarchy;
        
        public virtual void Show(object infoToShow = null, Action callback = null)
        {
            if (IsActive) return;
            OnSetInfoToShow(infoToShow);
            OnShowing();
            ActivateObject();
            callback?.Invoke();
            OnShown();
        }

        public virtual void Hide(Action callback = null)
        {
            if (!IsActive) return;
            OnHiding();
            DeactivateObject();
            callback?.Invoke();
            OnHidden();
        }

        private void OnDestroy()
        {
            if(IsActive) Hide();
        }

        protected virtual void DeactivateObject()
        {
            gameObject.SetActive(false);
        }

        protected virtual void ActivateObject()
        {
            gameObject.SetActive(true);
        }


        protected virtual void OnSetInfoToShow(object infoToShow)
        {
        }
        
        protected virtual void OnShowing() {}
        protected virtual void OnShown() {}
        protected virtual void OnHiding() {}
        protected virtual void OnHidden() {}
        
    }
}

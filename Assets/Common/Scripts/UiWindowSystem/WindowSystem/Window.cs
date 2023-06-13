using DG.Tweening;
using System;
using UnityEngine;

namespace Ui.WindowSystem
{
    public class Window : MonoBehaviour
    {
        public WindowAnimatron OnShow;
        public WindowAnimatron OnHide;

        public bool IsActive => gameObject.activeInHierarchy;
        
        public virtual void Show(object infoToShow = null, Action callback = null)
        {
            if (IsActive) return;
            var sequence = DOTween.Sequence()
                .AppendCallback(() => OnSetInfoToShow(infoToShow))
                .AppendCallback(OnShowing)
                .AppendCallback(ActivateObject);
            if (OnShow) OnShow.AppendAnimation(sequence);
            
            sequence.OnComplete(() =>
                {
                    callback?.Invoke();
                    OnShown();
                })
            .Play();
        }

        public virtual void Hide(Action callback = null)
        {
            if (!IsActive) return;
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(OnHiding);
            
            if (OnHide) OnHide.AppendAnimation(sequence);
            sequence.AppendCallback(DeactivateObject);
            
            sequence.OnComplete(() =>
            {
                callback?.Invoke();
                OnHidden();
            });
            sequence.Play();
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

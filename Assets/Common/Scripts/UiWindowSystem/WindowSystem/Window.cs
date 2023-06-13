using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Ui.WindowSystem
{
    public class Window : MonoBehaviour
    {
        public WindowAnimatron OnShow;
        public WindowAnimatron OnHide;

        public bool IsActive => gameObject.activeInHierarchy;
        public virtual void Show(Action callback = null)
        {
            if (IsActive) return;
            var sequence = DOTween.Sequence();
            sequence.OnComplete(() => callback?.Invoke());
            if (OnShow)
            {
                sequence.AppendCallback(() => OnShow.Hidden());
                sequence.Append(OnShow.Show());
            }
            else
            {
                sequence.AppendCallback(() => gameObject.SetActive(true));
            }
            sequence.Play();
        }
        public virtual void Hide(Action callback = null)
        {
            if (!IsActive) return;
            var sequence = DOTween.Sequence();
            sequence.OnComplete(() => callback?.Invoke());
            if (OnHide)
            {
                sequence.AppendCallback(() => OnHide.Shown());
                sequence.Append(OnHide.Hide());
            }
            else
            {
                sequence.AppendCallback(() => gameObject.SetActive(false));
            }
            sequence.Play();
        }
    }
}
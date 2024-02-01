using System;
using DG.Tweening;

namespace Ui.WindowSystem
{
    public class AnimatedWindow: Window
    {
        public WindowAnimatron OnShow;
        public WindowAnimatron OnHide;
        private Sequence _currentSequence;
        public override void Show(object infoToShow = null, Action callback = null)
        {
            if (IsActive) return;
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence()
                .AppendCallback(() => OnSetInfoToShow(infoToShow))
                .AppendCallback(OnShowing)
                .AppendCallback(ActivateObject);
            if (OnShow) OnShow.AppendAnimation(_currentSequence);
            
            _currentSequence.OnComplete(() =>
                {
                    callback?.Invoke();
                    OnShown();
                })
                .Play();
        }

        public override void Hide(Action callback = null)
        {
            if (!IsActive) return;
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence()
                .AppendCallback(OnHiding);
            
            if (OnHide) OnHide.AppendAnimation(_currentSequence);
            _currentSequence.AppendCallback(DeactivateObject)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                    OnHidden();
                })
                .Play();
        }
    }
}
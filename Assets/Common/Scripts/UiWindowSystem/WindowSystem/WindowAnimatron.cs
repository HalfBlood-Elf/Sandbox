using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Ui.WindowSystem
{
    public class WindowAnimatron : MonoBehaviour
    {
        public virtual Tween Show() => DOTween.Sequence();

        public virtual Tween Hide() => DOTween.Sequence();

        public virtual void Shown() {}

        public virtual void Hidden() { }
    }
}
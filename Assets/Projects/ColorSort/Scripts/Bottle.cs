using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Projects.ColorSort
{
    public class Bottle : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private BottlePart[] _bottlePartsBottomToTop;
        
        [Header("Animation")]
        [SerializeField] private Transform _animRoot;

        [SerializeField] private Vector3 _selectedOffset;
        
        public bool IsEmpty => GetTopNonEmptyContentId(out _) == BottlePart.EMPTY_ID;
        private bool IsFull => _bottlePartsBottomToTop.Last().CurrentId != BottlePart.EMPTY_ID;

        
        private IGameManager _gameManager;
        private Sequence _currentSequence;
        

        [Inject]
        private void Construct(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _gameManager?.BottleClicked(this);
        }

        public bool CanGetTransferFrom(Bottle selectedBottle)
        {
            if (selectedBottle == this) return false;
            if (IsEmpty) return true;
            if (IsFull) return false;
            
            return GetTopNonEmptyContentId(out _) == selectedBottle.GetTopNonEmptyContentId(out _);
        }


        public int GetTopNonEmptyContentId(out int partIndex)
        {
            for (int i = _bottlePartsBottomToTop.Length - 1; i >= 0; i--)
            {
                partIndex = i;
                var bottlePart = _bottlePartsBottomToTop[i];
                if (bottlePart.CurrentId != BottlePart.EMPTY_ID) return bottlePart.CurrentId;
            }
            partIndex = -1;
            return BottlePart.EMPTY_ID;
        }

        public void Selected()
        {
            _currentSequence = DOTween.Sequence()
                .AppendCallback(() => _animRoot.localPosition = Vector3.zero)
                .Append(_animRoot.DOLocalMove(_selectedOffset, 0.2f));
        }
        
        public void Deselected()
        {
            _currentSequence.Kill();
            
            _currentSequence = DOTween.Sequence()
                .AppendCallback(()=> _animRoot.localPosition = _selectedOffset)
                .Append(_animRoot.DOLocalMove(Vector3.zero, 0.2f));
        }
        
        public void Setup(SetupParameters setupParameters)
        {
            if (setupParameters.IdsStack.Length > _bottlePartsBottomToTop.Length)
            {
                Debug.LogError($"Parameters are wrong length. Expected {_bottlePartsBottomToTop.Length}, got {setupParameters.IdsStack.Length}");
            }

            for (int i = 0; i < _bottlePartsBottomToTop.Length; i++)
            {
                if(setupParameters.IdsStack.Length<=i) _bottlePartsBottomToTop[i].SetContentId(BottlePart.EMPTY_ID);
                else _bottlePartsBottomToTop[i].SetContentId(setupParameters.IdsStack[^(i+1)]);
                
            }
        }

        public void TransferContentsTo(Bottle toBottle)
        {
            while (toBottle.CanGetTransferFrom(this))
            {
                var idToTransfer = GetTopNonEmptyContentId(out int topIndex);
                toBottle.AddContent(idToTransfer);
                _bottlePartsBottomToTop[topIndex].SetContentId(BottlePart.EMPTY_ID);
            }
        }

        private void AddContent(int contentId)
        {
            for (int i = 0; i < _bottlePartsBottomToTop.Length; i++)
            {
                if(_bottlePartsBottomToTop[i].CurrentId != BottlePart.EMPTY_ID) continue;
                _bottlePartsBottomToTop[i].SetContentId(contentId);
                break;
            }
        }

        [System.Serializable]
        public struct SetupParameters
        {
            public int[] IdsStack;
        }

    }
}
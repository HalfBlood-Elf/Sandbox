using System.Collections;
using System.Collections.Generic;
using LocalObjectPooler;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SpiralPicker
{
    public class SpiralPickerItem : MonoBehaviour, IPoolableItem
    {
        [SerializeField] private Transform _itemImageContainer;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _hoveredColor = Color.white;
        [SerializeField] private Image _itemImage;
        private SetupParameters _currentSetupParameters;

        public void CompensateForParentRotation(float angle)
        {
            _itemImageContainer.localRotation = Quaternion.Euler(0, 0, -angle);
        }

        public void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        public void Setup(SetupParameters setupParameters)
        {
            _currentSetupParameters = setupParameters;
            _itemImage.sprite = _currentSetupParameters.ItemToShow.ItemSprite;
        }

        void ISetupable.Setup(PoolableItemSetupParameters setupParameters)
        {
            Setup(setupParameters as SetupParameters);
        }

        void IReturnableToPool.OnReturnToPool()
        {
            _currentSetupParameters = null;
            _backgroundImage.color = _normalColor;
        }

        public void OnActivate()
        {
            _currentSetupParameters?.ItemToShow.SlotSelectedCallback?.Invoke();
        }

        public void SetHovered(bool isHovered)
        {
            _backgroundImage.color = isHovered? _hoveredColor : _normalColor;
        }
        
        public class SetupParameters: PoolableItemSetupParameters
        {
            public int ItemIndex;
            public ISpiralPickerItemToShow ItemToShow;
        }
    }
}

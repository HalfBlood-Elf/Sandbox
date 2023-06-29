using LocalObjectPooler;
using UnityEngine;
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

        public SetupParameters CurrentSetupParameters => _currentSetupParameters;

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
            if (_currentSetupParameters is null) return;
            _itemImage.sprite = _currentSetupParameters.ItemToShow?.ItemSprite;
            _itemImage.color = _currentSetupParameters.ItemToShow?.ItemSprite? Color.white : Color.clear;
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
        
        [System.Serializable]
        public class SetupParameters: PoolableItemSetupParameters
        {
            public int ItemIndex;
            public int HeadingIndex;
            public ISpiralPickerItemToShow ItemToShow;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using LocalObjectPooler;
using Ui.WindowSystem;
using UnityEngine;

namespace SpiralPicker
{
    public class SpiralPicker: Window
    {
        [SerializeField] private SpiralSettings _spiralSettings = new()
        {
            PaddingDeg = 30,
            SlotFacingDirection = FacingDirection.Up,
        };
        [SerializeField] private ArrowSettings _arrowSettings = new()
        {
            ArrowRadius = 235,
            ArrowRotationOffset = -30,
            ArrowFacingDirection = FacingDirection.Right
        };
        [SerializeField] private uint _minSlotsCount;
        [SerializeField] private Transform _arrowTransform;
        [SerializeField] private SpiralPickerItem _itemPrefab;
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private CanvasGroup _cycleStartGroup;
        [SerializeField] private List<SpiralPickerItem> _activeItems = new();
        
        private ShowSettings _showSettings;

        protected override void OnSetInfoToShow(object infoToShow)
        {
            base.OnSetInfoToShow(infoToShow);
            if (infoToShow is not ShowSettings showSettings)
            {
                Hide();
                return;
            }
            Setup(showSettings);
        }
        
        private void Setup(ShowSettings showSettings)
        {
            //fill slots (including empty)
            uint maxIndex = showSettings.ItemsToShow.Max(x => x.SlotId);
            var itemToShows = new ISpiralPickerItemToShow[maxIndex];
            for (int i = 0; i < showSettings.ItemsToShow.Length; i++)
            {
                itemToShows[showSettings.ItemsToShow[i].SlotId] = showSettings.ItemsToShow[i];
            }
            showSettings.ItemsToShow = itemToShows;
            _showSettings = showSettings;
        }
        
        [System.Serializable]
        public struct ShowSettings
        {
            public ISpiralPickerItemToShow[] ItemsToShow;
            public int Index;
            public bool IsCycled;
        }
    }
}
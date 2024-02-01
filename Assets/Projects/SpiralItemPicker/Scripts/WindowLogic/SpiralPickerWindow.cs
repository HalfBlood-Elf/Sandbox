using Ui.WindowSystem;
using UnityEngine;
using System.Linq;

namespace SpiralPicker
{
    public class SpiralPickerWindow: Window
    {
        [SerializeField] private SpiralPicker _spiralPicker;
        [SerializeField] private MonoInputProvider _inputProvider;
        
        protected override void OnSetInfoToShow(object infoToShow)
        {
            base.OnSetInfoToShow(infoToShow);
            if (infoToShow is not WindowShowSettings showSettings)
            {
                Debug.LogError($"{GetType()} has no info to show", this);
                Hide();
                return;
            }

            Setup(showSettings);
            _inputProvider.Initialize(
                _spiralPicker.transform,
                () => _spiralPicker.CurrentSelectedIndex,
                () => _spiralPicker.SpiralSettings,
                () => _spiralPicker.MinSlotsCount,
                _spiralPicker.SelectSlot
            );
        }
        
        private void Setup(WindowShowSettings showSettings)
        {
            //fill slots (including empty)
            showSettings.ItemsToShow ??= System.Array.Empty<ISpiralPickerItemToShowIndexed>();
            uint maxIndex = (uint)Mathf.Max(
                showSettings.ItemsToShow.Length > 0
                    ? showSettings.ItemsToShow.Max(x => x.SlotId)
                    : 0,
                _spiralPicker.MinSlotsCount - 1);
            var itemToShowFilled = new ISpiralPickerItemToShow[maxIndex + 1];
            for (int i = 0; i < showSettings.ItemsToShow.Length; i++)
            {
                itemToShowFilled[showSettings.ItemsToShow[i].SlotId] = showSettings.ItemsToShow[i];
            }

            _spiralPicker.Setup(new()
            {
                ItemsToShow = itemToShowFilled,
                IsCycled = showSettings.IsCycled,
            });
        }
        
        [System.Serializable]
        public struct WindowShowSettings
        {
            public ISpiralPickerItemToShowIndexed[] ItemsToShow;
            public int Index;
            public bool IsCycled;
        }
    }
}
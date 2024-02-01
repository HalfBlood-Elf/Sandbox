using Ui.WindowSystem;
using UnityEngine;

namespace SpiralPicker
{

    public class SpiralPickerOpener : MonoBehaviour
    {
        [SerializeField] private bool _isCycled;
        [SerializeField] private SimpleItemToShow[] _itemsToShow;
        [SerializeField] private SpiralPickerWindow _spiralPickerWindow;

        private void Start()
        {
            SetupWindow();
        }
        
        [NaughtyAttributes.Button()]
        private void SetupWindow()
        {
            var simpleOpener = new SimpleWindowOpener(_spiralPickerWindow);
            simpleOpener.Show(new SpiralPickerWindow.WindowShowSettings{ItemsToShow = _itemsToShow, IsCycled = _isCycled});
        }
    }
}

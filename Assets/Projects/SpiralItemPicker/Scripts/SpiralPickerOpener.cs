using System;
using System.Collections;
using System.Collections.Generic;
using Ui.WindowSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpiralPicker
{

    public class SpiralPickerOpener : MonoBehaviour
    {
        [SerializeField] private bool _isCycled;
        [SerializeField] private ItemToShow[] _itemsToShow;
        [SerializeField] private SpiralPicker _spiralPickerWindow;
        private RouterCloseAllPrevious _router;

        private void Awake()
        {
            _router = new(new []
            {
                _spiralPickerWindow,
            });
        }

        private void Start()
        {
            _router.Show<SpiralPicker>(new SpiralPicker.ShowSettings{ItemsToShow = _itemsToShow, IsCycled = _isCycled});
        }
        
        [System.Serializable]
        public class ItemToShow: ISpiralPickerItemToShow
        {
            [field:SerializeField] public Sprite ItemSprite { get; private set; }
            [field:SerializeField] public uint SlotId { get; private set; }
            public Action SlotSelectedCallback { get; private set; }
        }
    }
}

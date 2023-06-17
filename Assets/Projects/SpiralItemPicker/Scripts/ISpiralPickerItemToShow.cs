using System;
using UnityEngine;

namespace SpiralPicker
{
    public interface ISpiralPickerItemToShow
    {
        public Sprite ItemSprite { get; }
        public uint SlotId { get; }
        public Action SlotSelectedCallback { get; }
    }
}
using System;
using UnityEngine;

namespace SpiralPicker
{
    public interface ISpiralPickerItemToShow
    {
        public Sprite ItemSprite { get; }
    }

    public interface ISpiralPickerItemToShowIndexed: ISpiralPickerItemToShow
    {
        public uint SlotId { get; }
    }
}
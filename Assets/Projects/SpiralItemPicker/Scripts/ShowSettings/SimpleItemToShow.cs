using UnityEngine;

namespace SpiralPicker
{
    [System.Serializable]
    public class SimpleItemToShow: ISpiralPickerItemToShowIndexed
    {
        [field:SerializeField] public Sprite ItemSprite { get; private set; }
        [field:SerializeField] public uint SlotId { get; private set; }
    }
}
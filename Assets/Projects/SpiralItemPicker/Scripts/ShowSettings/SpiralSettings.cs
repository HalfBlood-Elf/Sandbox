using UnityEngine;
using UnityEngine.Serialization;

namespace SpiralPicker
{
    [System.Serializable]
    public struct SpiralSettings
    {
        public float ZeroHourAngle;
        public FacingDirection SlotFacingDirection;
        public IndexIncreaseType IndexIncrease;
        [FormerlySerializedAs("PaddingDeg")] public float SpacingDegrees;

        public float CircleRadius;
        
        [Header("Spiral")] 
        public float MinRadius;
        public float MaxRadius;
        public float MinScale;
        public float MaxScale;
        [Range(0,1)]public float MinAlpha;
        [Range(0,1)]public float MaxAlpha;

        [Tooltip("How many items to the left/right of the selected item")]
        public ushort WingsSlotsShown;
        public ushort WingsSlotsToFade;
        [Header("Animation")] 
        public float TimeToMove;
        
        
        public float GetSlotAngleDegrees(int slotIndex, ushort baseSlotsCount)
        {
            int indexCycled = slotIndex % baseSlotsCount;
            return (SpacingDegrees * indexCycled * (int)IndexIncrease) + ZeroHourAngle;
        }
    }

    public enum IndexIncreaseType
    {
        Clockwise = -1,
        AntiClockwise = 1,
    }
}
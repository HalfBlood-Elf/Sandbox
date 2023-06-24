using UnityEngine;

namespace SpiralPicker
{
    [System.Serializable]
    public struct SpiralSettings
    {
        public FacingDirection SlotFacingDirection;
        public IndexIncreaseType IndexIncrease;
        public float PaddingDeg;

        public float CircleRadius;
        
        [Header("Spiral")] 
        public float MinRadius;
        public float MaxRadius;
        public float MinScale;
        public float MaxScale;
        [Range(0,1)]public float MinAlpha;
        [Range(0,1)]public float MaxAlpha;

        [Tooltip("How many items to the left/right of the selected item")]
        public int WingsSlotsShown;
        public int WingsSlotsToFade;
        [Header("Animation")] 
        public float TimeToMove;
    }

    public enum IndexIncreaseType
    {
        Clockwise = -1,
        AntiClockwise = 1,
    }
}
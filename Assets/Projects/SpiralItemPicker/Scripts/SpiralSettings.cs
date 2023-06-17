using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace SpiralPicker
{
    [System.Serializable]
    public struct SpiralSettings
    {
        public IndexIncreaseType IndexIncrease; 
        
        public float MinRadius;
        public float MaxRadius;
        public float MinScale;
        public float MaxScale;

        public float PaddingDeg;
        [Tooltip("How many items to the left/right of the selected item")]
        public int WingsSlotsShown;
        public int WingsSlotsToFade;
        [Range(0,1)]public float MinAlpha;
        [Range(0,1)]public float MaxAlpha;
        [Space()]
        public FacingDirection SlotFacingDirection;

        [Header("Animation")] 
        public float TimeToMove;
    }

    public enum IndexIncreaseType
    {
        Clockwise= -1,
        AntiClockwise = 1,
    }
}
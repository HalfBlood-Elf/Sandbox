using UnityEngine;

namespace SpiralPicker
{
    public static class SpiralPickerUtils
    {
        public static float FaceObjectAngleDeg(Vector2 targetPosition, FacingDirection facing)
        {
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
            angle -= (float)facing;
            return angle;
        }
        
        public static Vector2 UiDirectionFromAngle(float angleDegrees)
        {
            float radAngle = angleDegrees * Mathf.Deg2Rad;
            Vector2 posDirection = new(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
            return posDirection;
        }
    }
}
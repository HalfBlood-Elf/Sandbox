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
        
        /// <summary>
        /// Calculation of Greatest Common Factor by Euclidean algorithm.
        /// </summary>
        /// <remarks>
        /// Was yoinked form https://stackoverflow.com/questions/18541832/c-sharp-find-the-greatest-common-divisor
        /// </remarks>
        public static int Gcf(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b) a %= b;
                else b %= a;
            }
            return a | b;
        }
    
        /// <summary>
        /// Calculation of Greatest Least Common Multiple
        /// </summary>
        /// <remarks>
        /// Was yoinked form https://stackoverflow.com/questions/1356910/least-common-multiple
        /// </remarks>
        public static int Lcm(int a, int b) => (a / Gcf(a, b)) * b;
    }
}
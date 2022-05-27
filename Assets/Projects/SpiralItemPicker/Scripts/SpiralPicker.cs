using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpiralPicker
{
    public class SpiralPicker : MonoBehaviour
    {
        [System.Serializable]
        public struct SpiralSettings
        {
            public float minRadius;
            public float maxRadius;
            public float minScale;
            public float maxScale;

            public float paddingDeg;
            [Tooltip("How many items to the left/right of the selected item")]
            public int wingsSlotsShown;
            public int wingsSlotsToFade;
            public float minAlpha;
            [Space()]
            public FacingDirection slotFacingDirection;
            [Header("Arrow settings")]
            public float arrowRotationOffset;
            public float arrowRadius;
            public FacingDirection arrowFacingDirection;
            [Header("Animation")]
            public bool isDesceteTransition;
            
        }


        private const int ItemsCountBase = 12;

        public enum FacingDirection
        {
            UP = 270,
            DOWN = 90,
            LEFT = 180,
            RIGHT = 0
        }

        [SerializeField] private SpiralSettings settings = new SpiralSettings
        {
            minRadius = 263.4f,
            maxRadius = 479.46f,
            minScale = .67f,
            maxScale = 1.33f,
            paddingDeg = 30,
            wingsSlotsShown = 7,
            wingsSlotsToFade = 3,
            minAlpha = .3f,
            slotFacingDirection = FacingDirection.UP,
            isDesceteTransition = true,
        };

        [SerializeField] private float zeroHourAngle = -60;
        [SerializeField] private Transform arrow;
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private SpiralPickerItem slotPrefab;
        [SerializeField] private SpiralPickerItem[] slots;
        [SerializeField] private float debug;

        private int selectedSlotIndex = 0;

        private RectTransform rectTransform => transform as RectTransform;

        //private void Start()
        //{

        //    //var radAngle = (-paddingGrad * Mathf.Deg2Rad) * 0 + startAngle;
        //    ////Debug.Log(radius * Mathf.Cos(radAngle));
        //    //radAngle = (-paddingGrad * Mathf.Deg2Rad) * 6 + startAngle;
        //    //Debug.Log(radius * Mathf.Cos(radAngle));
        //}

        private void Update()
        {
            //PlaceSlots(selectedHour); //to setup parameters
            UpdateInputDirection();
        }

        private void UpdateInputDirection()
        {
            Vector3 userInputDirection = (Input.mousePosition - rectTransform.position).normalized;
            var inputAngle = Mathf.Atan2(userInputDirection.x, userInputDirection.y) * Mathf.Rad2Deg;
            if (inputAngle < 0) inputAngle += 360f;
            debug = inputAngle;

            var halfPadding = settings.paddingDeg / 2f;
            for (int i = 0; i < ItemsCountBase; i++)
            {
                if(inputAngle < halfPadding + (i * settings.paddingDeg))
                {
                    PlaceSlots(i);
                    break;
                }
            }
        }

        private void PlaceSlots(float selectedHour)
        {
            if (selectedHour > ItemsCountBase || selectedHour < -ItemsCountBase) selectedHour %= ItemsCountBase;
            var startAngleDeg = zeroHourAngle + (selectedHour * settings.paddingDeg);

            int rightWingSlotsIndexStart = slots.Length - settings.wingsSlotsToFade - 1;
            for (int i = 0; i < slots.Length; i++)
            {
                float indexNormalised = ((float)i) / slots.Length;

                var slot = slots[i];

                float targetAngle = -settings.paddingDeg * i + -startAngleDeg - (float)settings.slotFacingDirection;

                float radius = Mathf.Lerp(settings.minRadius, settings.maxRadius, indexNormalised);
                float radAngle = targetAngle * Mathf.Deg2Rad;
                var posDirection = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
                slot.transform.localPosition = posDirection * radius;

                float rotationZ = FaceObjectAngleDeg(slotsContainer.localPosition, slot.transform.localPosition, settings.slotFacingDirection);
                slot.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
                slot.CompensateForParentRotation(rotationZ);

                
                float scaleIndex = Mathf.Lerp(settings.minScale, settings.maxScale, indexNormalised);
                slot.transform.localScale = Vector3.one * scaleIndex;

                // fading of left and right "wings"
                bool inLefFadetWing = i < settings.wingsSlotsToFade;
                bool inRightFadeWing = i > rightWingSlotsIndexStart;
                if (inLefFadetWing || inRightFadeWing)
                {
                    float t_left = (float)i / settings.wingsSlotsToFade;
                    float t_right = (slots.Length - i) / (float)settings.wingsSlotsToFade;

                    slot.SetAlpha(Mathf.Lerp(settings.minAlpha, 1, inLefFadetWing? t_left : t_right));
                }
            }

            if(arrow != null)
            {
                var radAngle = -(startAngleDeg + settings.arrowRotationOffset) * Mathf.Deg2Rad;
                var posDirection = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
                arrow.localPosition = posDirection * settings.arrowRadius;
                float rotationZ = FaceObjectAngleDeg(slotsContainer.localPosition, arrow.localPosition, settings.arrowFacingDirection);
                arrow.localRotation = Quaternion.Euler(0, 0, rotationZ);
            }
        }






        private void SpawnSlotsHolders(int wingsSlotsshown, System.Func<Transform, SpiralPickerItem> spawnCallback)
        {
            var totalSlotsCount = 1 + wingsSlotsshown + wingsSlotsshown;

            slots = new SpiralPickerItem[totalSlotsCount];
            for (int i = 0; i < totalSlotsCount; i++)
            {
                SpiralPickerItem item = spawnCallback(slotsContainer);
                slots[i] = item;
            }
        }

        private void ClearSpawnedSlots(System.Action<GameObject> destroyHandler)
        {
            //Undo.RecordObject(towerCreator, "Delete Created Floors");
            var childs = slotsContainer.Cast<Transform>().ToArray();

            for (int i = 0; i < childs.Length; i++)
            {
                destroyHandler(childs[i].gameObject);
            }
        }

        public static float FaceObjectAngleDeg(Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing)
        {
            Vector2 direction = targetPosition - startingPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= (float)facing;
            return angle;
        }


#if UNITY_EDITOR
        [CustomEditor(typeof(SpiralPicker)), CanEditMultipleObjects]
        public class SpiralPicker_Editor : Editor
        {
            private SpiralPicker[] ts;

            private void OnEnable()
            {
                ts = targets.Cast<SpiralPicker>().ToArray();
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (GUILayout.Button("Recreate slots"))
                {
                    for (int i = 0; i < ts.Length; i++)
                    {
                        Undo.IncrementCurrentGroup();
                        Undo.SetCurrentGroupName("Create radial slots");
                        var undoGroupIndex = Undo.GetCurrentGroup();

                        ts[i].ClearSpawnedSlots((go) => Destroy(go));

                        ts[i].SpawnSlotsHolders(
                            ts[i].settings.wingsSlotsShown,
                            (parent) => CreatePrefab(ts[i].slotPrefab, parent));
                        ts[i].PlaceSlots(0);
                        Undo.CollapseUndoOperations(undoGroupIndex);
                    }
                }
            }

            private SpiralPickerItem CreatePrefab(SpiralPickerItem prefab, Transform parent)
            {
                var go = PrefabUtility.InstantiatePrefab(prefab.gameObject, parent) as GameObject;
                Undo.RegisterCreatedObjectUndo(go, "Create radial menu slot");
                return go.GetComponent<SpiralPickerItem>();
            }

            private void Destroy(GameObject go)
            {
                Undo.DestroyObjectImmediate(go);
            }

        }
#endif
    }
}

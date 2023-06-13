﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpiralPicker
{
    public class SpiralPicker : MonoBehaviour
    {
        private const int ITEMS_COUNT_BASE = 12;

        public enum FacingDirection
        {
            Up = 270,
            Down = 90,
            Left = 180,
            Right = 0
        }

        [SerializeField] private SpiralSettings _spiralSettings = new()
        {
            MinRadius = 263.4f,
            MaxRadius = 479.46f,
            MinScale = .67f,
            MaxScale = 1.33f,
            PaddingDeg = 30,
            WingsSlotsShown = 7,
            WingsSlotsToFade = 3,
            MinAlpha = .3f,
            MaxAlpha = 1f,
            SlotFacingDirection = FacingDirection.Up,
            TimeToMove = 1f,
        };

        [SerializeField] private ArrowSettings _arrowSettings = new()
        {
            ArrowRadius = 235,
            ArrowRotationOffset = -30,
            ArrowFacingDirection = FacingDirection.Right
        };
        [SerializeField] private float _zeroHourAngle = -60;
        [SerializeField] private Transform _arrowTransform;
        [SerializeField] private Transform _slotsContainer;
        [SerializeField] private SpiralPickerItem _slotPrefab;
        [SerializeField] private SpiralPickerItem[] _slots;

        private RectTransform RectTransform => transform as RectTransform;

        private void OnValidate()
        {
            var targetPos = RectTransform.position;
            targetPos.y += 10;
            UpdateInputDirection(targetPos);
        }

        private void Update()
        {
            UpdateInputDirection(Input.mousePosition);
        }

        public void ShowItems(ISpiralPickerItem[] items, bool isCyclic)
        {
            
        }

        private void UpdateInputDirection(Vector3 mousePosition)
        {
            Vector3 userInputDirection = (mousePosition - RectTransform.position).normalized;
            var inputAngle = Mathf.Atan2(userInputDirection.x, userInputDirection.y) * Mathf.Rad2Deg;
            if (inputAngle < 0) inputAngle += 360f;

            var halfPadding = _spiralSettings.PaddingDeg / 2f;
            for (int i = 0; i < ITEMS_COUNT_BASE; i++)
            {
                if (inputAngle > halfPadding + (i * _spiralSettings.PaddingDeg)) continue;
                PlaceSlots(i);
                break;
            }
        }

        private void PlaceSlots(float selectedHour)
        {
            if (selectedHour is > ITEMS_COUNT_BASE or < -ITEMS_COUNT_BASE) selectedHour %= ITEMS_COUNT_BASE;
            var startAngleDeg = _zeroHourAngle + (selectedHour * _spiralSettings.PaddingDeg);

            int rightWingSlotsIndexStart = _slots.Length - _spiralSettings.WingsSlotsToFade - 1;
            for (int i = 0; i < _slots.Length; i++)
            {
                float indexNormalised = ((float)i) / _slots.Length;

                var slot = _slots[i];

                float targetAngle = -_spiralSettings.PaddingDeg * i + -startAngleDeg - (float)_spiralSettings.SlotFacingDirection;

                float radius = Mathf.Lerp(_spiralSettings.MinRadius, _spiralSettings.MaxRadius, indexNormalised);
                float radAngle = targetAngle * Mathf.Deg2Rad;
                var posDirection = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
                var targetPosition = posDirection * radius;
                slot.transform.DOLocalMove(targetPosition, _spiralSettings.TimeToMove);

                float rotationZ = FaceObjectAngleDeg(_slotsContainer.localPosition, targetPosition, _spiralSettings.SlotFacingDirection);
                slot.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, rotationZ), _spiralSettings.TimeToMove);
                slot.CompensateForParentRotation(rotationZ);

                
                float scaleIndex = Mathf.Lerp(_spiralSettings.MinScale, _spiralSettings.MaxScale, indexNormalised);
                slot.transform.DOScale(Vector3.one * scaleIndex, _spiralSettings.TimeToMove);

                // fading of left and right "wings"
                bool inLeftFadeWing = i < _spiralSettings.WingsSlotsToFade;
                bool inRightFadeWing = i > rightWingSlotsIndexStart;
                if (inLeftFadeWing || inRightFadeWing)
                {
                    float tLeft = (float)i / _spiralSettings.WingsSlotsToFade;
                    float tRight = (_slots.Length - i) / (float)_spiralSettings.WingsSlotsToFade;

                    slot.SetAlpha(Mathf.Lerp(_spiralSettings.MinAlpha, _spiralSettings.MaxAlpha, inLeftFadeWing? tLeft : tRight));
                }
            }

            HandleArrow(startAngleDeg);
        }

        private void HandleArrow(float startAngleDeg)
        {
            if (!_arrowTransform) return;
            
            var radAngle = -(startAngleDeg + _arrowSettings.ArrowRotationOffset) * Mathf.Deg2Rad;
            var posDirection = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
            _arrowTransform.localPosition = posDirection * _arrowSettings.ArrowRadius;
            float rotationZ = FaceObjectAngleDeg(_slotsContainer.localPosition, _arrowTransform.localPosition,
                _arrowSettings.ArrowFacingDirection);
            _arrowTransform.localRotation = Quaternion.Euler(0, 0, rotationZ);
        }


        private void SpawnSlotsHolders(int wingsSlotsShown, System.Func<Transform, SpiralPickerItem> spawnCallback)
        {
            var totalSlotsCount = 1 + wingsSlotsShown + wingsSlotsShown;

            _slots = new SpiralPickerItem[totalSlotsCount];
            for (int i = 0; i < totalSlotsCount; i++)
            {
                SpiralPickerItem item = spawnCallback(_slotsContainer);
                _slots[i] = item;
            }
        }

        private void ClearSpawnedSlots(System.Action<GameObject> destroyHandler)
        {
            //Undo.RecordObject(towerCreator, "Delete Created Floors");
            var childs = _slotsContainer.Cast<Transform>().ToArray();

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

        [System.Serializable]
        public struct SpiralSettings
        {
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

        [System.Serializable]
        public struct ArrowSettings
        {
            
            public float ArrowRotationOffset;
            public float ArrowRadius;
            public FacingDirection ArrowFacingDirection;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(SpiralPicker)), CanEditMultipleObjects]
        public class SpiralPickerEditor : Editor
        {
            private SpiralPicker[] _targets;

            private void OnEnable()
            {
                _targets = targets.Cast<SpiralPicker>().ToArray();
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!GUILayout.Button("Recreate slots")) return;
                for (int i = 0; i < _targets.Length; i++)
                {
                    Undo.IncrementCurrentGroup();
                    Undo.SetCurrentGroupName("Create radial slots");
                    var undoGroupIndex = Undo.GetCurrentGroup();

                    _targets[i].ClearSpawnedSlots(Destroy);

                    var capturedI = i;
                    _targets[i].SpawnSlotsHolders(
                        _targets[i]._spiralSettings.WingsSlotsShown,
                        (parent) => CreatePrefab(_targets[capturedI]._slotPrefab, parent));
                    _targets[i].PlaceSlots(0);
                    Undo.CollapseUndoOperations(undoGroupIndex);
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

    public interface ISpiralPickerItem
    {
    }
}

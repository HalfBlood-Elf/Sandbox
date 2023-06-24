﻿using System;
using UnityEngine;

namespace SpiralPicker
{
    public class CircleItemPicker : MonoBehaviour
    {
        [SerializeField] private float _zeroHourAngle = 90;
        [SerializeField] private SpiralSettings _spiralSettings = new()
        {
            CircleRadius = 335,
            PaddingDeg = 30,
            SlotFacingDirection = FacingDirection.Down,
        };
        [SerializeField] private ArrowSettings _arrowSettings = new()
        {
            ArrowRadius = 200,
            ArrowRotationOffset = 0,
            ArrowFacingDirection = FacingDirection.Right
        };
        [SerializeField] private SpiralPickerItem _itemPrefab;
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private SpiralPickerItem[] _slots;
        [SerializeField] private Transform _arrowTransform;
        private int _currentSelectedIndex;
        private Vector3 _lastMousePos;

        private int BaseSlotsCount => _slots.Length;
        
        private void OnValidate()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                var slot = _slots[i];
                
                float targetAngle = GetSlotAngleDegrees(i);
                var posDirection = UiDirectionFromAngle(targetAngle);

                float radius = _spiralSettings.CircleRadius;
                Vector2 targetLocalPosition = posDirection * radius;
                slot.transform.localPosition = targetLocalPosition;
                
                float rotationZ = FaceObjectAngleDeg(targetLocalPosition, _spiralSettings.SlotFacingDirection);
                slot.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
                slot.CompensateForParentRotation(rotationZ);
            }
            SelectSlot(0);
        }

        private static Vector2 UiDirectionFromAngle(float angleDegrees)
        {
            float radAngle = angleDegrees * Mathf.Deg2Rad;
            Vector2 posDirection = new(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
            return posDirection;
        }

        private void Start()
        {
            SelectSlot(0);
            _lastMousePos = Input.mousePosition;
        }

        private void Update()
        {
            HandleMousePos(Input.mousePosition);
            HandleInputDelta(Input.mouseScrollDelta.y);
        }

        private void HandleMousePos(Vector3 mousePos)
        {
            if (mousePos == _lastMousePos) return;
            Vector3 userInputDirection = (mousePos - _itemContainer.position).normalized;
            var targetSlotDirection = UiDirectionFromAngle(GetSlotAngleDegrees(_currentSelectedIndex));
            var delta = Vector2.SignedAngle(userInputDirection, targetSlotDirection);
            if (Mathf.Abs(delta) < _spiralSettings.PaddingDeg / 2f) return;
            HandleInputDelta(delta);
            _lastMousePos = mousePos;
        }

        private void HandleInputDelta(float delta)
        {
            IndexChangeDirection indexChange = delta switch
            {
                > 0 => IndexChangeDirection.Next,
                < 0 => IndexChangeDirection.Previous,
                _ => IndexChangeDirection.None
            };
            if(indexChange is not IndexChangeDirection.None) SelectSlot(_currentSelectedIndex + (int)indexChange);
        }

        private float GetSlotAngleDegrees(int slotIndex)
        {
            var indexCycled = slotIndex % BaseSlotsCount;
            return (_spiralSettings.PaddingDeg * indexCycled * (int)_spiralSettings.IndexIncrease) + _zeroHourAngle;
        }
        
        private void SelectSlot(int selectedIndex)
        {
            if (selectedIndex >= _slots.Length) selectedIndex = 0;
            if (selectedIndex < 0) selectedIndex = _slots.Length-1;
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].SetHovered(i==selectedIndex);
            }
            HandleArrow(GetSlotAngleDegrees(selectedIndex));
            _currentSelectedIndex = selectedIndex;
        }

        private void HandleArrow(float targetAngle)
        {
            if (!_arrowTransform) return;
            
            var radAngle = (targetAngle + _arrowSettings.ArrowRotationOffset) * Mathf.Deg2Rad;
            var posDirection = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
            _arrowTransform.localPosition = posDirection * _arrowSettings.ArrowRadius;
            
            float rotationZ = FaceObjectAngleDeg(_arrowTransform.localPosition, _arrowSettings.ArrowFacingDirection);
            _arrowTransform.localRotation = Quaternion.Euler(0, 0, rotationZ);
        }
        
        private static float FaceObjectAngleDeg(Vector2 targetPosition, FacingDirection facing)
        {
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
            angle -= (float)facing;
            return angle;
        }
        
        public enum IndexChangeDirection
        {
            None = 0,
            Next = 1,
            Previous = -1,
        }
    }
}
using System;
using System.Collections.Generic;
using LocalObjectPooler;
using UnityEngine;

namespace SpiralPicker
{
    public class SpiralPicker: MonoBehaviour
    {
        [SerializeField] private SpiralSettings _spiralSettings = new()
        {
            ZeroHourAngle = 90,
            CircleRadius = 335,
            PaddingDeg = 30,
            SlotFacingDirection = FacingDirection.Down,
            IndexIncrease = IndexIncreaseType.Clockwise,

            MinRadius = 263.4f,
            MaxRadius = 479.46f,
            MinScale = .67f,
            MaxScale = 1.33f,
            WingsSlotsShown = 7,
            WingsSlotsToFade = 3,
            MinAlpha = .3f,
            MaxAlpha = 1f,
            TimeToMove = 1f,
        };

        [SerializeField] private ArrowSettings _arrowSettings = new()
        {
            ArrowRadius = 235,
            ArrowRotationOffset = -30,
            ArrowFacingDirection = FacingDirection.Right
        };

        [SerializeField] private ushort _minSlotsCount = 12;
        [SerializeField] private Transform _arrowTransform;
        [SerializeField] private SpiralPickerItem _itemPrefab;
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private CanvasGroup _cycleStartGroup;
        [SerializeField] private List<SpiralPickerItem> _activeItems = new();

        private ShowSettings _showSettings;
        private int _currentSelectedIndex;
        private ComponentObjectPooler<SpiralPickerItem> _pooler;
        private SpiralPickerItem _currentSelectedItem;
        private SetupType _currentSetupType;

        public ushort MinSlotsCount => _minSlotsCount;
        public int CurrentSelectedIndex => _currentSelectedIndex;
        public SpiralSettings SpiralSettings => _spiralSettings;

        public void Setup(ShowSettings showSettings)
        {
            _showSettings = showSettings;
            SelectSlot(0);
        }

        public void SelectSlot(int selectedIndex)
        {
            if (selectedIndex >= _showSettings.ItemsToShow.Length)
            {
                if (!_showSettings.IsCycled) return;
                selectedIndex = 0;
            }

            if (selectedIndex < 0)
            {
                if (!_showSettings.IsCycled) return;
                selectedIndex = _showSettings.ItemsToShow.Length - 1;
            }

            UpdateSlotPositions();
            HandleArrow(_spiralSettings.GetSlotAngleDegrees(selectedIndex, _minSlotsCount));
            _currentSelectedIndex = selectedIndex;
        }

        private void UpdateSlotPositions()
        {
            if (_showSettings.ItemsToShow.Length <= _minSlotsCount)
            {
                UpdateAsCircle();
                return;
            }

            if (_showSettings.IsCycled) UpdateAsCycledSpiral();
            else UpdateAsNonCycledSpiral();
        }

        private void UpdateAsCircle()
        {
            SetSlotsCount(_minSlotsCount);
            SetupAsCircle();
            for (int i = 0; i < _activeItems.Count; i++)
            {
                var slot = _activeItems[i];

                slot.SetHovered(i == _currentSelectedIndex);
            }
        }

        private void UpdateAsCycledSpiral()
        {
            SetSlotsCount(1 + _spiralSettings.WingsSlotsShown + _spiralSettings.WingsSlotsShown);
            var infoToShow = new List<SpiralPickerItem.SetupParameters>();
            //left (of selected slot) wing of slots
            for (int i = _spiralSettings.WingsSlotsShown; i > 0; i--)
            {
                int slotId = _currentSelectedIndex - i;
                if (slotId < 0) slotId = _minSlotsCount - Math.Abs(slotId);
                var itemIndex = _showSettings.ItemsToShow.Length - Math.Abs(slotId);
                infoToShow.Add(new() { ItemIndex = itemIndex, HeadingIndex = slotId, ItemToShow = _showSettings.ItemsToShow[itemIndex], WingId = -i});
            }

            // currently selected item
            infoToShow.Add(new()
                { ItemIndex = _currentSelectedIndex, HeadingIndex = _currentSelectedIndex, ItemToShow = _showSettings.ItemsToShow[_currentSelectedIndex] });

            //left (of selected slot) wing of slots
            for (int i = 1; i <= _spiralSettings.WingsSlotsShown; i++)
            {
                int slotId = _currentSelectedIndex + i;
                var itemIndex = slotId > _showSettings.ItemsToShow.Length? slotId - _showSettings.ItemsToShow.Length: slotId;
                if (slotId > _minSlotsCount) slotId = slotId - _minSlotsCount;
                infoToShow.Add(new() { ItemIndex = itemIndex, HeadingIndex = slotId, ItemToShow = _showSettings.ItemsToShow[slotId], WingId = i});
            }

            for (int i = 0; i < _activeItems.Count; i++)
            {
                var slot = _activeItems[i];
                slot.Setup(infoToShow[i]);
                slot.SetHovered(infoToShow[i].ItemIndex == _currentSelectedIndex);
                float inverseIndexNormalised = 1 - (float)i / _activeItems.Count;
                float inverseWingIndexNormalized = 1 - Mathf.Abs((float)infoToShow[i].WingId) / _spiralSettings.WingsSlotsShown;
                SetSlotPositionInSpiral(
                    infoToShow[i].HeadingIndex,
                    slot,
                    Mathf.Lerp(_spiralSettings.MinRadius, _spiralSettings.MaxRadius, inverseIndexNormalised),
                    Mathf.Lerp(_spiralSettings.MinScale, _spiralSettings.MaxScale, inverseIndexNormalised),
                    Mathf.Lerp(_spiralSettings.MinAlpha, _spiralSettings.MaxAlpha, inverseWingIndexNormalized)
                );
            }

            _currentSetupType = SetupType.SpiralCycled;
        }

        private void UpdateAsNonCycledSpiral()
        {
        }

        private void SetSlotsCount(int count)
        {
            _pooler ??= new(_itemPrefab, _itemContainer);
            var difference = count - _activeItems.Count;
            if (difference == 0) return;
            var needToAdd = difference > 0;
            var needToRemove = difference < 0;
            for (int i = 0; i < Mathf.Abs(difference); i++)
            {
                if (needToAdd)
                {
                    var item = _pooler.GetFreeObject();
                    _activeItems.Add(item);
                }
                else if (needToRemove)
                {
                    _pooler.ReturnToPool(_activeItems[0]);
                    _activeItems.RemoveAt(0);
                }
            }
        }

        private void SetupAsCircle()
        {
            if (_currentSetupType == SetupType.Circle) return;
            for (int i = 0; i < _activeItems.Count; i++)
            {
                var slot = _activeItems[i];

                SetSlotPositionInSpiral(i, slot, _spiralSettings.CircleRadius, 1, 1);

                slot.Setup(new() { HeadingIndex = i, ItemToShow = _showSettings.ItemsToShow[i] });
            }

            _currentSetupType = SetupType.Circle;
        }

        private void SetSlotPositionInSpiral(int index, SpiralPickerItem slot, float radius, float scaleFactor, float alpha)
        {
            float targetAngle = _spiralSettings.GetSlotAngleDegrees(index, _minSlotsCount);
            var posDirection = SpiralPickerUtils.UiDirectionFromAngle(targetAngle);
            Debug.Log($"index: {index} targetAngle: {targetAngle}");
            slot.transform.localPosition = posDirection * radius;

            float rotationZ = SpiralPickerUtils.FaceObjectAngleDeg(
                posDirection,
                _spiralSettings.SlotFacingDirection);
            slot.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
            slot.CompensateForParentRotation(rotationZ);

            slot.transform.localScale = Vector3.one * scaleFactor;
            slot.SetAlpha(Mathf.Clamp01(alpha));
        }

        private void HandleArrow(float targetAngle)
        {
            if (!_arrowTransform) return;

            var radAngle = (targetAngle + _arrowSettings.ArrowRotationOffset) * Mathf.Deg2Rad;
            var posDirection = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
            _arrowTransform.localPosition = posDirection * _arrowSettings.ArrowRadius;

            float rotationZ =
                SpiralPickerUtils.FaceObjectAngleDeg(_arrowTransform.localPosition,
                    _arrowSettings.ArrowFacingDirection);
            _arrowTransform.localRotation = Quaternion.Euler(0, 0, rotationZ);
        }


        [System.Serializable]
        public struct ShowSettings
        {
            public ISpiralPickerItemToShow[] ItemsToShow;
            public bool IsCycled;
        }

        private enum SetupType
        {
            None,
            Circle,
            SpiralCycled,
            SpiralNonCycled,
        }
    }
}
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

        [SerializeField] private DebugShowSettings _debugShowSettings;

        [SerializeField] private ushort _minSlotsCount = 12;
        [SerializeField] private Transform _arrowTransform;
        [SerializeField] private SpiralPickerItem _itemPrefab;
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private CanvasGroup _cycleStartGroup;
        [SerializeField] private List<SpiralPickerItem> _activeItems = new();

        private ShowSettings _showSettings;
        private int _currentSelectedIndex;
        private ComponentObjectPooler<SpiralPickerItem> _pooler;
        private SetupType _currentSetupType;
        private int _leastCommonMultiple;

        public ushort MinSlotsCount => _minSlotsCount;
        public int CurrentSelectedIndex => _currentSelectedIndex;
        public SpiralSettings SpiralSettings => _spiralSettings;

        public void Setup(ShowSettings showSettings)
        {
            _showSettings = showSettings;
            _leastCommonMultiple = SpiralPickerUtils.Lcm(showSettings.ItemsToShow.Length, _minSlotsCount);
            SelectSlot(showSettings.StartIndex);
        }

        public void SelectSlot(int selectedIndex)
        {
            selectedIndex %= _leastCommonMultiple;
            if (selectedIndex >= _showSettings.ItemsToShow.Length && !_showSettings.IsCycled)
            {
                selectedIndex = _showSettings.ItemsToShow.Length;
            }

            if (selectedIndex < 0 && !_showSettings.IsCycled)
            {
                selectedIndex = 0;
            }
            _currentSelectedIndex = selectedIndex;
            
            UpdateSlotPositions();
            HandleArrow(_spiralSettings.GetSlotAngleDegrees(selectedIndex, _minSlotsCount));
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

                slot.SetHovered(i == GetItemIndex(_currentSelectedIndex));
            }
        }

        private void UpdateAsCycledSpiral()
        {
            SetSlotsCount(1 + _spiralSettings.WingsSlotsShown + _spiralSettings.WingsSlotsShown);
            var infoToShow = new List<SpiralPickerItem.SetupParameters>();
            //left (of selected slot) wing of slots
            for (int i = _spiralSettings.WingsSlotsShown; i > 0; i--)
            {
                int index = _currentSelectedIndex - i;
                int itemIndex = GetItemIndex(index);
                infoToShow.Add(new()
                {
                    ItemIndex = itemIndex,
                    HeadingIndex = GetHeadingIndex(index),
                    ItemToShow = _showSettings.ItemsToShow[itemIndex],
                    WingId = -i
                });
            }

            // currently selected item
            infoToShow.Add(new()
            {
                ItemIndex = GetItemIndex(_currentSelectedIndex),
                HeadingIndex = GetHeadingIndex(_currentSelectedIndex),
                ItemToShow = _showSettings.ItemsToShow[GetItemIndex(_currentSelectedIndex)]
            });

            //left (of selected slot) wing of slots
            for (int i = 1; i <= _spiralSettings.WingsSlotsShown; i++)
            {
                int index = _currentSelectedIndex + i;
                int itemIndex = GetItemIndex(index);
                infoToShow.Add(new()
                {
                    ItemIndex = itemIndex,
                    HeadingIndex = GetHeadingIndex(index),
                    ItemToShow = _showSettings.ItemsToShow[itemIndex],
                    WingId = i
                });}

            for (int i = 0; i < _activeItems.Count; i++)
            {
                var slot = _activeItems[i];
                slot.Setup(infoToShow[i]);
                slot.SetHovered(infoToShow[i].ItemIndex == GetItemIndex(_currentSelectedIndex));
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

        private int GetHeadingIndex(int index)
        {
            index %= _minSlotsCount;
            if (index < 0) return _minSlotsCount - Mathf.Abs(index);
            if (index > _minSlotsCount) return index - _minSlotsCount;
            return index;
        }
        
        private int GetItemIndex(int index)
        {
            index %= _showSettings.ItemsToShow.Length;
            if (index < 0) return _showSettings.ItemsToShow.Length - Mathf.Abs(index);
            if (index > _showSettings.ItemsToShow.Length) return index - _showSettings.ItemsToShow.Length;
            return index;
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
            public int StartIndex;
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
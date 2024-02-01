using System;
using UnityEngine;

namespace SpiralPicker
{
    public class InputManagerProvider: MonoInputProvider
    {
        [SerializeField] private bool _useMousePos;
        [SerializeField] private bool _useScrollWheel;
        [SerializeField] private bool _useDiscreteButtons;
        private void Update()
        {
            if(_useMousePos) HandleMousePos(Input.mousePosition);
            if(_useScrollWheel) HandleInputDelta(Input.mouseScrollDelta.y);
            if (_useDiscreteButtons) HandleDiscreteButtons();
        }

        private void HandleDiscreteButtons()
        {
            var indexChange = IInputProvider.IndexChangeDirection.None;
            if (Input.GetMouseButtonDown(0)) indexChange = IInputProvider.IndexChangeDirection.Previous;
            if (Input.GetMouseButtonDown(1)) indexChange = IInputProvider.IndexChangeDirection.Next;
            var currentIndex = _indexGetter();
            if(indexChange is not IInputProvider.IndexChangeDirection.None) _slotSelector(currentIndex + (int)indexChange);
        }

        private void HandleMousePos(Vector3 mousePos)
        {
            if (mousePos == _lastMousePos) return;
            Vector3 userInputDirection = (mousePos - _container.position).normalized;
            var spiralSettings = _spiralSettingsGetter();
            var currentIndex = _indexGetter();
            var baseCount = _baseCountGetter();
            var targetSlotDirection = SpiralPickerUtils.UiDirectionFromAngle(spiralSettings.GetSlotAngleDegrees(currentIndex, baseCount));
            var delta = Vector2.SignedAngle(userInputDirection, targetSlotDirection);
            if (Mathf.Abs(delta) < spiralSettings.SpacingDegrees / 2f) return;
            HandleInputDelta(delta);
            _lastMousePos = mousePos;
        }

        private void HandleInputDelta(float delta)
        {
            IInputProvider.IndexChangeDirection indexChange = delta switch
            {
                > 0 => IInputProvider.IndexChangeDirection.Next,
                < 0 => IInputProvider.IndexChangeDirection.Previous,
                _ => IInputProvider.IndexChangeDirection.None
            };
            var currentIndex = _indexGetter();
            if(indexChange is not IInputProvider.IndexChangeDirection.None) _slotSelector(currentIndex + (int)indexChange);
        }
    }
}
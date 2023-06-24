using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.WindowSystem
{
    public class MenuTabButton : MonoBehaviour
    {
        [SerializeField] private bool _isSelected;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private ButtonState _activeState;
        [SerializeField] private ButtonState _inactiveState;

        private MenuTabs _windowWithTabs;
        private MenuTabs.Tab _targetTab;

        public MenuTabs.Tab TargetTab => _targetTab;

        public System.Type TargetTabType => _targetTab.Window.GetType();

        public void Setup(MenuTabs windowWithTabs, MenuTabs.Tab item)
        {
            this._windowWithTabs = windowWithTabs;
            _targetTab = item;
            _title.text = item.Name;
        }

        public void OnButtonClick()
        {
            if (!_isSelected)
                _windowWithTabs.OpenTab(_targetTab.Window);
        }

        [System.Serializable]
        public class ButtonState
        {
            public Color TextColor = Color.black;
            public Color ButtonColor = Color.white;
            public GameObjectState[] GameObjectStates;
        }

        [System.Serializable]
        public class GameObjectState
        {
            public GameObject GameObject;
            public bool IsActive;
        }

        public void SetState(bool isActive)
        {
            _isSelected = isActive;
            SetVisualState(_isSelected);
        }

        public void SetVisualState(bool isActive)
        {
            var targetState = isActive ? _activeState : _inactiveState;
            _buttonImage.color = targetState.ButtonColor;
            _title.color = targetState.TextColor;
            foreach (var item in targetState.GameObjectStates)
            {
                item.GameObject.SetActive(item.IsActive);
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            SetVisualState(_isSelected);
        }
#endif
    }
}
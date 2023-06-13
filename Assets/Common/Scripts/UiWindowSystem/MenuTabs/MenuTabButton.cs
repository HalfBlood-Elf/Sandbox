using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.WindowSystem
{
	public class MenuTabButton : MonoBehaviour
    {
        [SerializeField] private bool isSelected;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Image buttonImage;
        [SerializeField] private ButtonState activeState;
        [SerializeField] private ButtonState inactiveState;

        private MenuTabs windowWithTabs;
        private MenuTabs.Tab targetTab;

        public MenuTabs.Tab TargetTab => targetTab;

        public System.Type TargetTabType => targetTab.window.GetType();

        public void Setup(MenuTabs windowWithTabs, MenuTabs.Tab item)
        {
            this.windowWithTabs = windowWithTabs;
            targetTab = item;
            title.text = item.name;
        }

        public void OnButtonClick()
        {
            if (!isSelected)
                windowWithTabs.OpenTab(targetTab.window);
        }

        [System.Serializable]
        public class ButtonState
        {
            public Color textColor = Color.black;
            public Color buttonColor = Color.white;
            public GameObjectState[] gameObjectStates;
        }

        [System.Serializable]
        public class GameObjectState
        {
            public GameObject gameObject;
            public bool isActive;
        }

        public void SetState(bool isActive)
        {
            isSelected = isActive;
            SetVisualState(isSelected);
        }

        public void SetVisualState(bool isActive)
        {
            var targetState = isActive ? activeState : inactiveState;
            buttonImage.color = targetState.buttonColor;
            title.color = targetState.textColor;
            foreach (var item in targetState.gameObjectStates)
            {
                item.gameObject.SetActive(item.isActive);
            }

        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            SetVisualState(isSelected);
        }
#endif
    }
}

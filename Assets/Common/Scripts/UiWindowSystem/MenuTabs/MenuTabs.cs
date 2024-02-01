using System;
using LocalObjectPooler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.WindowSystem
{
    public class MenuTabs : MonoBehaviour
    {
        [SerializeField] private Transform _tabButtonsContainer;
        [SerializeField] private MenuTabButton _tabButtonPrefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Tab[] _tabs = Array.Empty<Tab>();

        private RouterCloseAllPrevious _windowRouter;
        private ComponentObjectPooler<MenuTabButton> _tabsPooler;
        protected readonly List<MenuTabButton> ActiveTabButtons = new();

        private void Awake()
        {
            _windowRouter = new(_tabs.Select(x => x.Window));
            _tabsPooler = new(new Factory<MenuTabButton>(_tabButtonPrefab, _tabButtonsContainer));

            foreach (var item in _tabs)
            {
                var button = _tabsPooler.GetFreeObject();
                button.Setup(this, item);
                button.gameObject.SetActive(true);
                button.transform.SetAsLastSibling();
                ActiveTabButtons.Add(button);
            }
        }

        private void OnEnable()
        {
            _scrollRect.viewport.localPosition = Vector3.zero;

            if (_tabs?.Length > 0)
            {
                OpenTab(_tabs[0].Window);
            }
        }

        public void OpenTab(Window tab)
        {
            var targetTabType = tab.GetType();
            _windowRouter.Show(targetTabType.Name);
            foreach (var item in ActiveTabButtons)
            {
                item.SetState(targetTabType == item.TargetTabType);
            }
        }

        [System.Serializable]
        public class Tab
        {
            [Tooltip("Tab name")] public string Name;
            public Window Window;

            //todo: editor OnValidate for buttons with target names
        }
    }
}
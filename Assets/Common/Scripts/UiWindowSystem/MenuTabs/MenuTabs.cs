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
        [SerializeField] private Transform tabButtonsContainer;
        [SerializeField] private MenuTabButton tabButtonPrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Tab[] tabs = new Tab[0];

        private RouterCloseAllPrevious windowRouter;
        private ComponentObjectPooler<MenuTabButton> tabsPooler;
        protected List<MenuTabButton> activeTabButtons = new();

        private void Awake()
        {
            windowRouter = new(tabs.Select(x => x.window));
            tabsPooler = new(tabButtonPrefab, tabButtonsContainer);

            foreach (var item in tabs)
            {
                var button = tabsPooler.GetFreeObject();
                button.Setup(this, item);
                button.gameObject.SetActive(true);
                button.transform.SetAsLastSibling();
                activeTabButtons.Add(button);
            }
        }

        private void OnEnable()
        {
            scrollRect.viewport.localPosition = Vector3.zero;

            if (tabs?.Length > 0)
            {
                OpenTab(tabs[0].window);
            }
        }

        public void OpenTab(Window tab)
        {
            var targetTabType = tab.GetType();
            windowRouter.Show(targetTabType.Name);
            foreach (var item in activeTabButtons)
            {
                item.SetState(targetTabType == item.TargetTabType);
            }
        }

        [System.Serializable]
        public class Tab
        {
            [Tooltip("Tab name")] public string name;
            public Window window;

            //todo: editor onvalidate for buttons with target names
        }
    }
}
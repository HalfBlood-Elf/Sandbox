using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Projects.IncomeGame {
    public class TowerCreation : MonoBehaviour, IPointerClickHandler {
        
        public GameObject tower;
        public int towerCost = 200;
        
        [Inject] private IInstantiator _instantiator;
        private Money _money;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount != 2) return;
            
            if (_money.money >= towerCost)
            {
                var t = _instantiator.InstantiatePrefab(tower, eventData.position, Quaternion.identity, transform);
                t.GetComponent<Image>().color = Random.ColorHSV();
                t.GetComponent<IncomeTower>().price = towerCost;
            }
            else
            {
                Debug.Log("Not enough money!");
            }

        }

        // Use this for initialization
        void Start() {
            _money = GameObject.Find("Money").GetComponent<Money>();
        }
    }
}
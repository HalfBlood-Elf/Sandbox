using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IncomeGame {
    public class TowerCreation : MonoBehaviour, IPointerClickHandler {
        public GameObject tower;
        public int towerCost = 200;
        private Money _money;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2)
            {
                if (_money.money >= towerCost)
                {
                    var t = Instantiate(tower, eventData.position, Quaternion.identity, transform);
                    t.GetComponent<Image>().color = Random.ColorHSV();
                    t.GetComponent<IncomeTower>().price = towerCost;
                }
                else
                {
                    Debug.Log("Not enough money!");
                }
            }

        }

        // Use this for initialization
        void Start() {
            _money = GameObject.Find("Money").GetComponent<Money>();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
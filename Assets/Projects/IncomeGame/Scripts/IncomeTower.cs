using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace IncomeGame
{
    [RequireComponent(typeof(Animator))]
    public class IncomeTower : MonoBehaviour, IDropHandler
    {
        public int level = 1;
        public int maxLevel = 4;
        public List<Sprite> levelSprites;

        public int price = 200;
        public int income;
        public float frequency;
        public int additionalIncome;
        public float additionalFrequency;

        private Money _money;
        private float _timer;
        private Animator _anim;
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            _money = GameObject.Find("Money").GetComponent<Money>();
            _anim = GetComponent<Animator>();
        }

        void Update()
        {
            if (_timer < frequency - additionalFrequency)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                Action();
            }
        }
        public void OnDrop(PointerEventData eventData)
        {
            IncomeTower droppedTower = eventData.pointerDrag.GetComponent<IncomeTower>();
            Debug.Log(this.name + " " + droppedTower.name);
            int combineCost = Mathf.RoundToInt((droppedTower.price + this.price) * 2.5f);
            if (droppedTower && _money.money >= combineCost)
            {
                _money.Buy(combineCost);
                CombineTowers(droppedTower);
            }
        }

        private void CombineTowers(IncomeTower droppedTower)
        {
            if (droppedTower.level == this.level)
            {
                this.level++;
                _image.sprite = levelSprites[level - 1];
                price += droppedTower.price;
                income = Mathf.RoundToInt(income * 2.5f);
                frequency /= 1.1f;
                additionalIncome += droppedTower.additionalIncome;
                additionalFrequency += droppedTower.additionalFrequency;

                Destroy(droppedTower.gameObject);
            }
            else if (droppedTower.level < this.level)
            {
                price += droppedTower.price;

                additionalIncome += Mathf.RoundToInt((droppedTower.additionalIncome + droppedTower.income) * 0.1f); 
                additionalFrequency += (droppedTower.frequency - droppedTower.additionalFrequency) * 0.01f;

                Destroy(droppedTower.gameObject);
            }
        }

        private void Action()
        {
            _money.AddMoney(income + additionalIncome);
            _anim.SetTrigger("Pulse");
            _timer = 0;
        }
    }
}

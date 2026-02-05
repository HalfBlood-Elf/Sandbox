using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

namespace Projects.IncomeGame
{
    [RequireComponent(typeof(Animator))]
    public class IncomeTower : MonoBehaviour, IDropHandler
    {
        public int level = 0;

        public int price = 200;
        public int income;
        public float frequency;
        public int additionalIncome;
        public float additionalFrequency;

        private Money _money;
        private float _timer;
        private Animator _anim;
        private Image _image;
        private GameSettings _gameSettings;

        [Inject]
        private void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
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
            if (droppedTower.level == this.level && this.level < _gameSettings.MaxLevel)
            {
                this.level++;
                _image.sprite = _gameSettings.GetLevelSprite(level - 1);
                price += droppedTower.price;
                income = Mathf.RoundToInt(income * 2.5f);
                frequency /= 1.1f;
                additionalIncome += droppedTower.additionalIncome;
                additionalFrequency += droppedTower.additionalFrequency;

                Destroy(droppedTower.gameObject);
            }
            else if (droppedTower.level < this.level && _gameSettings.CanCombineDifferentLevel)
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

using System;
using System.Collections;
using System.Collections.Generic;
using LocalObjectPooler;
using Projects.ColorSort;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Projects.ColorSort
{
    interface IGameManager
    {
        void BottleClicked(Bottle bottle);
    }
    
    public class ColorSortManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private Bottle _bottlePrefab;
        [SerializeField] private Transform _bottleContainer;
        private Bottle _selectedBottle;

        private ComponentObjectPooler<Bottle> _bottlePool;
        protected readonly List<Bottle> _activeBottles = new();
        private LevelData _currentLevel;


        private void Start()
        {
            _bottlePool = new(new Factory<Bottle>(_bottlePrefab, _bottleContainer));
            _currentLevel = _levelData;
            ResetLevel();
        }

        public void ResetLevel()
        {
            if (_currentLevel is null)
            {
                Debug.LogError("No Level was selected");
                return;
            }
            foreach (var bottle in _activeBottles)
            {
                _bottlePool.ReturnToPool(bottle);
            }
            _activeBottles.Clear();
            foreach (var dataBottle in _currentLevel.Bottles)
            {
                var bottle = _bottlePool.GetFreeObject();
                bottle.Setup(dataBottle);
                _activeBottles.Add(bottle);
            }
        }

        public void BottleClicked(Bottle bottle)
        {
            if (_selectedBottle is null)
            {
                _selectedBottle = bottle;
                _selectedBottle.Selected();
                return;
            }
            else if (bottle.CanGetTransferFrom(_selectedBottle))
            {
                _selectedBottle.TransferContentsTo(toBottle: bottle);
            }
            
            _selectedBottle.Deselected();
            _selectedBottle = null;
        }
    }
}
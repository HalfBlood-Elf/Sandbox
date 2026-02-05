using System.Collections.Generic;
using UnityEngine;

namespace Projects.IncomeGame
{
    [CreateAssetMenu(fileName = nameof(GameSettings), menuName = "ScriptableObject/Projects/IncomeGame/"+nameof(GameSettings), order = 0)]
    public class GameSettings : ScriptableObject
    {
        public int MaxLevel = 4;
        
        [SerializeField] private bool canCanCombineDifferentLevel;
        [SerializeField] private Sprite[] _levelSprites;

        public bool CanCombineDifferentLevel
        {
            get => canCanCombineDifferentLevel;
            protected set => canCanCombineDifferentLevel = value;
        }

        public Sprite GetLevelSprite(int level)
        {
            return _levelSprites[level - 0];
        }
    }
}
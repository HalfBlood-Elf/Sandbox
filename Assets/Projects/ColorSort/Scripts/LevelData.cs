using System.Collections.Generic;
using UnityEngine;

namespace Projects.ColorSort
{
    [CreateAssetMenu(fileName = nameof(LevelData), menuName = "ScriptableObject/Projects/ColorSort/"+nameof(LevelData), order = 0)]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private Bottle.SetupParameters[] _bottles;
        
        public IReadOnlyList<Bottle.SetupParameters> Bottles => _bottles;
    }
}
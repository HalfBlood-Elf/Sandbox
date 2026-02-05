using UnityEngine;
using Zenject;

namespace Projects.IncomeGame.DI
{
    public class IncomeGameInstaller : MonoInstaller
    {
        [SerializeField] private GameSettings _gameSettings;
    
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSettings>().FromInstance(_gameSettings);
        }
    }
}
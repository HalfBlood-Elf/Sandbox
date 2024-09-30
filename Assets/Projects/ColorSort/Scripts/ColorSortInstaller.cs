using UnityEngine;
using Zenject;

namespace Projects.ColorSort.DI
{
    public class ColorSortInstaller : MonoInstaller
    {
        [SerializeField] private ColorSortManager _colorSortManager;
    
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ColorSortManager>().FromInstance(_colorSortManager);
        }
    }
}
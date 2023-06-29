using System;
using UnityEngine;

namespace SpiralPicker
{
    public abstract class MonoInputProvider: MonoBehaviour, IInputProvider
    {
        public event System.Action<IInputProvider.IndexChangeDirection> InputChanged
        {
            add => _inputChanged += value;
            remove => _inputChanged -= value;
        }

        protected System.Action<IInputProvider.IndexChangeDirection> _inputChanged { get; set; }
        
        protected Transform _container;
        protected Vector3 _lastMousePos;
        protected Func<int> _indexGetter;
        protected Func<SpiralSettings> _spiralSettingsGetter;
        protected Func<ushort> _baseCountGetter;
        protected Action<int> _slotSelector;

        public void Initialize(
            Transform container,
            Func<int> indexGetter,
            Func<SpiralSettings> spiralSettingsGetter,
            Func<ushort> baseCountGetter,
            Action<int> slotSelector)
        {
            _container = container;
            _indexGetter = indexGetter;
            _spiralSettingsGetter = spiralSettingsGetter;
            _baseCountGetter = baseCountGetter;
            _slotSelector = slotSelector;
        }

    }

    public interface IInputProvider
    {
        public enum IndexChangeDirection
        {
            None = 0,
            Next = 1,
            Previous = -1,
        }

        public event System.Action<IndexChangeDirection> InputChanged;

        public void Initialize(
            Transform container,
            Func<int> indexGetter,
            Func<SpiralSettings> spiralSettingsGetter,
            Func<ushort> baseCountGetter,
            Action<int> slotSelector);
    }
}
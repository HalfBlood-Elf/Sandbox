using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestingSandbox
{
    public class TestPrefabSpawn : MonoBehaviour
    {
        [SerializeField] private TestPrefabItem _prefabItem;

        private void Start()
        {
            var item = Instantiate(_prefabItem, transform);
            Debug.Log($"Test value: {item.TestValue}");
        }
    }
}

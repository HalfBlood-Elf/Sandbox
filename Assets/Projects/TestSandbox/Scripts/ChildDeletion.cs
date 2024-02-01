using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestingSandbox
{
    public class ChildDeletion : MonoBehaviour
    {
        private void Start()
        {
            // this deletes all objects without TestPrefabItem and doesn't skip any objects. This is good
            foreach (Transform child in transform)
            {
                if(!child.TryGetComponent<TestPrefabItem>(out _)) Destroy(child.gameObject);
            }
        }

        private void BadDeletion()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if(!child.TryGetComponent<TestPrefabItem>(out _)) Destroy(child.gameObject);
            }
        }
        
        private void SlightlyBetterDeletion()
        {
            var toDelete = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.TryGetComponent<TestPrefabItem>(out _))
                {
                    toDelete.Add(child.gameObject);
                }
            }

            foreach (var child in toDelete)
            {
                Destroy(child);
            }
        }
    }
}

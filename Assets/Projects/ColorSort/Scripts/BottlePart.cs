using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.ColorSort
{
    public class BottlePart : MonoBehaviour
    {
        public const int EMPTY_ID = -1;
        
        [SerializeField] private Image _image;
        [SerializeField] private Color[] _colors;
        
        public int CurrentId { get; protected set; }

        public void SetContentId(int i)
        {
            CurrentId = i;
            _image.color = i != EMPTY_ID ? _colors[i] : Color.clear;
        }
    }
}
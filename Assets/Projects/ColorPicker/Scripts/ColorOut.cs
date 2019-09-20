using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorOut : MonoBehaviour {
    float h, s, v;
    public List<Image> images = new List<Image>();

    public void SetColor(float h, float s, float v)
    {
        for (int i = 0; i < images.Count; i++)
        {
            images[i].color = Color.HSVToRGB(h, s, v - 0.2f*i);
        }
    }
}

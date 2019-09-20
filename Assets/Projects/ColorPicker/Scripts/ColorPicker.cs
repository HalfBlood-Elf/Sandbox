using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace ColorPicker
{
    public class ColorPicker : MonoBehaviour
    {
        public List<GameObject> outImages = new List<GameObject>();
        public int colors;

        [SerializeField]
        private float H, S =1, V =1;

        private ColorPrint cprint;

        private HPick hPick;
        private S_V_pick svPick;
        public void SetColorHSV(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
            hPick.SetColor(h);
            svPick.SetColor(s, v);
            OutColor();
        }
        /// <param name="r">[0..1]</param>
        /// <param name="g">[0..1]</param>
        ///  <param name="b">[0..1]</param>
        public void SetColorRGB(float r,float g,float b)
        {
            //Debug.Log(string.Format("{0},{1},{2}", r, g, b));
            var cMax = Mathf.Max(r, g, b);
            var cMin = Mathf.Min(r, g, b);
            var d = cMax - cMin;
            //Debug.Log(cMax + " " + cMin);
            float h = 0;
            if (d != 0)
            {
                if (cMax == r)
                    h = 60 * ((g - b / d) % 6);
                else if(cMax == g)
                    h = 60 * ((b - r / d) + 2);
                else if (cMax == b)
                    h = 60 * ((r - g / d) + 4);
            }
            float s = 0;
            if (cMax != 0)
                s = d / cMax;
            var v = cMax;
            //Debug.Log(h);
            SetColorHSV(h, s, v);
            //out 0-360,0-1,0-1
        }
        public void SetH(int h)
        {
            H = h;
            OutColor();
        }
        void OutColor()
        {
            outImages[0].GetComponent<ColorOut>().SetColor(H / 360, S, V);
            if (colors == 2)
                outImages[1].GetComponent<ColorOut>().SetColor(((H + 180) % 360) / 360, S, V);
            else if(colors == 3)
                outImages[1].GetComponent<ColorOut>().SetColor(((H + 160) % 360) / 360, S, V);
            outImages[2].GetComponent<ColorOut>().SetColor(Mathf.Abs((H - 160) % 360)/ 360, S, V);
            cprint.PrintColor(H/360, S, V);
        }
        public void SetSV(float s, float v)
        {
            S = Mathf.Clamp01(s);
            V = Mathf.Clamp01(v);
            OutColor();
        }
        private void Start()
        {
            cprint = GameObject.Find("Panel (1)").GetComponent<ColorPrint>();
            hPick = GameObject.Find("Color").GetComponent<HPick>();
            svPick = GameObject.Find("S_And_V").GetComponent<S_V_pick>();
        }
        public void SetScheme(int colors)
        {
            this.colors = colors+1;
            for (int i = 0; i < 3; i++)
            {
                if (i > colors)
                    outImages[i].SetActive(false);
                else outImages[i].SetActive(true);
            }
            hPick.SetSchema(colors);
        }
    }
}

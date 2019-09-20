using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace ColorPicker
{
    public class ColorPrint : MonoBehaviour
    {
        public List<InputField> colorPrints = new List<InputField>(5);
        /*  0-rgb;
            1-hex;
            2-cmyk;
            3-hsv;
            4-hsl;*/

        /// <param name="h">[0..1]</param>
        /// <param name="s">[0..1]</param>
        ///  <param name="v">[0..1]</param>
        public void PrintColor(float h, float s, float v)
        {
            colorPrints[0].text = HSVtoRGB(h,s,v);
            colorPrints[1].text = HSVtoHEX(h, s, v);
            colorPrints[2].text = HSVtoCMYK(h, s, v);
            colorPrints[3].text = string.Format(
                "hsv({0}, {1}, {2})",h*360,Mathf.RoundToInt(s*100), Mathf.RoundToInt(v*100));
            colorPrints[4].text = HSVtoHSL(h, s, v);
        }
        /// <param name="h">[0..1]</param>
        /// <param name="s">[0..1]</param>
        ///  <param name="v">[0..1]</param>
        string HSVtoRGB(float h, float s, float v)
        {
            Color color = Color.HSVToRGB(h, s, v);
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);
            return string.Format("rgb({0}, {1}, {2})", r,g,b);
        }
        string HSVtoHEX(float h, float s, float v)
        {
            Color color = Color.HSVToRGB(h, s, v);
            int r = Mathf.RoundToInt(color.r * 255);
            int g = Mathf.RoundToInt(color.g * 255);
            int b = Mathf.RoundToInt(color.b * 255);
            return string.Format("#{0:x}{1:x}{2:x}", r, g, b);
        }
        string HSVtoCMYK(float h, float s, float v)
        {
            Color color = Color.HSVToRGB(h, s, v);
            float k = Mathf.Min(1 - color.r, 1 - color.g, 1 - color.b);
            float c = 0, m = 0, y = 0;
            if (k != 1)
            {

                c = (1 - color.r - k) / (1 - k);
                m = (1 - color.g - k) / (1 - k);
                y = (1 - color.b - k) / (1 - k);
                
            }
            return string.Format("CMYK({0}, {1}, {2}, {3})",
                Mathf.RoundToInt(c*100), Mathf.RoundToInt(m * 100), Mathf.RoundToInt(y * 100), Mathf.RoundToInt(k * 100));
        }
        string HSVtoHSL(float h, float s, float v)
        {
            int outH = Mathf.RoundToInt(h*360);
            h *= 360;
            float outS, outL;
            outL = (2-s)*v/2;
            outS = (outL!=0 && outL < 1) ? s * v /(outL<0.5 ? outL*2:2-outL*2 ): 0;
            return string.Format("hsl({0}, {1}, {2})",
                outH, Mathf.RoundToInt(outS*100), Mathf.RoundToInt(outL * 100));
        }

        void ParseInput(string s)
        {
            if (s[0] != '#')
            {


                char[] delimiterChars = { ' ', ',', '(', ')' };

                string[] values = s.Split(delimiterChars);
            }
            else
            {
                /*byte r, g, b;
                if(int.TryParse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber,out r)
                g = Convert.ToByte(s.Substring(4, 2), 16);
                b = Convert.ToByte(s.Substring(6, 2), 16);*/

            }
        }




        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

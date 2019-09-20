using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MemoryTraining
{
    public enum InputType
    {
        rows,
        columns
    }
    public class Input : MonoBehaviour
    {
        public InputType type;
        public PanelBehavior panel;

        byte value;
        string prevString;
        // Use this for initialization
        void Start()
        {
            prevString = GetComponent<InputField>().text;

        }

        public void TryInputParse(string s)
        {
            if (byte.TryParse(s, out value) && value <= 10 && value>0)
            {
                prevString = value.ToString();
                if(type ==InputType.columns)
                {
                    panel.SetColomns(value);
                }
                else panel.SetRows(value);
            }
            else GetComponent<InputField>().text = prevString;

        }
    }
}

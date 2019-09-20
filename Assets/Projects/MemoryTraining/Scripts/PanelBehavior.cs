using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MemoryTraining
{
    public class PanelBehavior : MonoBehaviour
    {
        public GameObject CellPrefab;
        public Transform answerPannel;


        private FlexibleGrid flexGrid;
        private int childCount;
        [SerializeField]
        List<byte> answers;
        List<byte> zeroHundred = new List<byte>(100);
        void Start()
        {
            if(transform.childCount>0)
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
            flexGrid = GetComponent<FlexibleGrid>();
            childCount = flexGrid.columns * flexGrid.rows;
            answers= new List<byte>(childCount);
            for (int i = 0; i < childCount; i++)
            {
                var c = Instantiate(CellPrefab, transform);
                c.GetComponent<Cell>().dragable = false;
            }
            flexGrid.SetGrid();
            NewAnswers();
        }
        void NewAnswers()
        {
            zeroHundred = new List<byte>(100);
            answers = new List<byte>(childCount);
            for (int i = 1; i <= zeroHundred.Capacity; i++)
            {
                zeroHundred.Add((byte)i);
            }
            Debug.Log("zeroHundred initialised");
            for (int i = 0; i < answers.Capacity; i++)
            {
                int pos = Random.Range(0, zeroHundred.Count);
                answers.Add(zeroHundred[pos]);
                transform.GetChild(i).SendMessage("SetNumber", zeroHundred[pos], SendMessageOptions.DontRequireReceiver);

                zeroHundred.RemoveAt(pos);

            }
        }
        public void StartGame()
        {
            SliderText slider = GameObject.Find("Slider").GetComponent<SliderText>();
            NewAnswers();
            ShowCells();


            slider.StartCountDown();
            var answersSorted = new List<byte>(answers);
            answersSorted.Sort();
            if(answerPannel.childCount>0)
            {
                for (int i = 0; i < answerPannel.childCount; i++)
                {
                    Destroy(answerPannel.GetChild(i).gameObject);
                }
            }
            for (int i = 0; i < childCount; i++)
            {
                var c = Instantiate(CellPrefab, answerPannel);
                c.GetComponent<Cell>().SetNumber(answersSorted[i]);
                c.GetComponent<Cell>().dragable = true;
                c.GetComponentInChildren<Text>(true).enabled = true;
            }
        }
        void ShowCells()
        {
            for (int i = 0; i < childCount; i++)
            {
                var c = transform.GetChild(i);
                c.GetComponentInChildren<Text>(true).enabled = true;
            }
        }
        public void HideCells()
        {
            for (int i = 0; i < childCount; i++)
            {
                var c = transform.GetChild(i);
                    c.GetComponentInChildren<Text>().enabled = false;
                c.GetComponent<Cell>().number = 0;
            }
        }
        public void CheckAnswers()
        {
            for (int i = 0; i < childCount; i++)
            {
                var c = transform.GetChild(i).GetComponent<Cell>();
                if (c.number == answers[i])
                    c.Right();
                else c.Wrong();
            }
        }
        public void SetColomns(byte colomns)
        {
            flexGrid.columns = colomns;
            Start();
        }
        public void SetRows(byte rows)
        {
            flexGrid.rows = rows;
            Start();
        }
    }
}
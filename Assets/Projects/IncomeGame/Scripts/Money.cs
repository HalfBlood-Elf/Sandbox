using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour {
    public int money
    { get { return _money;}
    }

    [SerializeField]
    private int _money;
    private Text uiText;

    public void AddMoney(int income)
    {
        _money += income;
        uiText.text = "Money: " + _money;
    }
    public void Buy(int price)
    {
        _money -= price;
        uiText.text = "Money: " + _money;
    }
    // Use this for initialization
    void Start () {
        uiText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

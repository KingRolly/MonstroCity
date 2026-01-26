using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public TextMeshProUGUI goblinCounter;

    [SerializeField]private int money;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // set current money to amt
    public void setMoney(int amt)
    {
        money = amt;
        goblinCounter.text = money.ToString();
    }

    // add money by amt (can be positive or negative)
    public void addMoney(int amt)
    {
        money += amt;
        goblinCounter.text = money.ToString();
    }
}

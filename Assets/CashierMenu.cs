using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CashierMenu : MonoBehaviour
{
    public GameObject historyItems;
    private bool isHistoryOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney()
    {

    }

    public void Withdraw()
    {

    }

    public void ScratchCard()
    {

    }

    public void ToggleHistory()
    {
        if (!isHistoryOpened)
        {
            historyItems.SetActive(true);
            isHistoryOpened = true;
        }
        else
        {
            historyItems.SetActive(false);
            isHistoryOpened = false;
        }
    }

    public void Purchase()
    {

    }

    public void Tds()
    {

    }

    public void WithdrawHistory()
    {

    }

    public void Bonus()
    {

    }

    public void Back()
    {
        SceneManager.LoadScene("Poker Menu");
    }
}

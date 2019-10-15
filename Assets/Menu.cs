using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject freeRollItems;
    public GameObject realItems;
    public GameObject accountItems;
    private bool isFreeRollOpened = false;
    private bool isRealOpened = false;
    private bool isAccountOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleFreeRoll()
    {
        if (!isFreeRollOpened)
        {
            freeRollItems.SetActive(true);
            isFreeRollOpened = true;
        }
        else
        {
            freeRollItems.SetActive(false);
            isFreeRollOpened = false;
        }
    }

    public void toggleReal()
    {
        if (!isRealOpened)
        {
            realItems.SetActive(true);
            isRealOpened = true;
        }
        else
        {
            realItems.SetActive(false);
            isRealOpened = false;
        }
    }

    public void toggleAccount()
    {
        if (!isAccountOpened)
        {
            accountItems.SetActive(true);
            isAccountOpened = true;
        }
        else
        {
            accountItems.SetActive(false);
            isAccountOpened = false;
        }
    }

    public void Cashier()
    {
        SceneManager.LoadScene("Cashier");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Company : Building
{
    public Company()
    {
        LOCAdded = 50000;
        BuyCost = 100000;
        SellCost = 50000;
        AppearNextMinimum = 100000000;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("AppearNext", 0, 1);
        InvokeRepeating("SendLOC", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

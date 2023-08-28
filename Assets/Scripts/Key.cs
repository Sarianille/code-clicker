using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Building
{
    public Key()
    {
        LOCAdded = 1;
        BuyCost = 10;
        SellCost = 5;
        AppearNextMinimum = 100;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URandom : Building
{
    public URandom()
    {
        LOCAdded = 10;
        BuyCost = 100;
        SellCost = 50;
        AppearNextMinimum = 1000;
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

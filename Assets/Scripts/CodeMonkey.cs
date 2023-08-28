using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeMonkey : Building
{
    public CodeMonkey()
    {
        LOCAdded = 100;
        BuyCost = 500;
        SellCost = 250;
        AppearNextMinimum = 10000;
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

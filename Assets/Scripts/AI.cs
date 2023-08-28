using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Building
{
    public AI()
    {
        LOCAdded = 100000;
        BuyCost = 150000;
        SellCost = 75000;
        AppearNextMinimum = 1000000000;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SendLOC", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

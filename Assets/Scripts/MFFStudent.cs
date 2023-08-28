using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFFStudent : Building
{
    public MFFStudent()
    {
        LOCAdded = 1000;
        BuyCost = 10000;
        SellCost = 5000;
        AppearNextMinimum = 1000000;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftedChild : Building
{
    public GiftedChild()
    {
        LOCAdded = 500;
        BuyCost = 2000;
        SellCost = 1000;
        AppearNextMinimum = 100000;
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

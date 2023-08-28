using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractorTeam : Building
{
    public ContractorTeam()
    {
        LOCAdded = 10000;
        BuyCost = 50000;
        SellCost = 25000;
        AppearNextMinimum = 10000000;
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

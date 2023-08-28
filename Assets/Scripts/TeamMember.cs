using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMember : Building
{
    public TeamMember()
    {
        LOCAdded = 5000;
        BuyCost = 15000;
        SellCost = 7500;
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

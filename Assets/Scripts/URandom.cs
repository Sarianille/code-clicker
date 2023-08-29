using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class URandom : Building
{
    [SerializeField]
    private GameObject upgradeHigherThroughput;
    private Condition upgradeHigherThroughputCondition;

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
        SetupUpgrades();
        SetupConditions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetupUpgrades()
    {
        upgradeHigherThroughput.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeHigherThroughput, 20, 200));

        upgrades = new GameObject[] { upgradeHigherThroughput };
    }

    public override void SetupConditions()
    {
        upgradeHigherThroughputCondition = new Condition(this, 3, 1000, null);

        conditions = new Condition[] { upgradeHigherThroughputCondition };

        InvokeRepeating("ShowUpgrade", 0, 1);
    }
}

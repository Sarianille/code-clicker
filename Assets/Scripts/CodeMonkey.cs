using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeMonkey : Building
{
    [SerializeField]
    private GameObject upgradeDvorak;
    private Condition upgradeDvorakCondition;
    [SerializeField]
    private GameObject upgradeStyleGuide;
    private Condition upgradeStyleGuideCondition;

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
        SetupUpgrades();
        SetupConditions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetupUpgrades()
    {
        upgradeDvorak.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeDvorak, 10, 100));
        upgradeStyleGuide.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeStyleGuide, 50, 1000));

        upgrades = new GameObject[] { upgradeDvorak, upgradeStyleGuide };
    }

    public override void SetupConditions()
    {
        upgradeDvorakCondition = new Condition(this, 5, 50000, null);
        upgradeStyleGuideCondition = new Condition(this, 10, 100000, null);

        conditions = new Condition[] { upgradeDvorakCondition, upgradeStyleGuideCondition };

        InvokeRepeating("ShowUpgrade", 0, 1);
    }
}

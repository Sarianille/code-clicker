using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : Building
{
    [SerializeField]
    private GameObject upgradeHumanEmotions;
    private Condition upgradeHumanEmotionsCondition;

    public AI()
    {
        LOCAdded = 100000;
        BuyCost = 150000;
        SellCost = 75000;
        AppearNextMinimum = 10000000000;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupUpgrades();
        SetupConditions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetupUpgrades()
    {
        upgradeHumanEmotions.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeHumanEmotions, 50000, 10000000));

        upgrades = new GameObject[] { upgradeHumanEmotions };
    }

    public override void SetupConditions()
    {
        upgradeHumanEmotionsCondition = new Condition(this, 15, 10000000000, null);

        conditions = new Condition[] { upgradeHumanEmotionsCondition };

        InvokeRepeating("ShowUpgrade", 0, 1);
    }
}

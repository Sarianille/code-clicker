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
    }

    void Start()
    {
        SetupDefaults();
        SetupUpgrades();
        SetupConditions();
    }

    public override void SetupUpgrades()
    {
        upgradeHigherThroughput.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeHigherThroughput, 20, 200));

        upgrades = new GameObject[] { upgradeHigherThroughput };
    }

    public override void SetupConditions()
    {
        upgradeHigherThroughputCondition = new Condition(this, 3, 1000, null);

        conditions = new Condition[] { upgradeHigherThroughputCondition };

        InvokeRepeating(nameof(ShowUpgrade), 0, 1);
    }
}

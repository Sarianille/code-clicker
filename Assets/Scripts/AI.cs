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
    }

    void Start()
    {
        SetupDefaults();
        SetupUpgrades();
        SetupConditions();
    }

    public override void SetupUpgrades()
    {
        upgradeHumanEmotions.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeHumanEmotions, 50000, 10000000));

        upgrades = new GameObject[] { upgradeHumanEmotions };
    }

    public override void SetupConditions()
    {
        upgradeHumanEmotionsCondition = new Condition(this, 15, 10000000000, null);

        conditions = new Condition[] { upgradeHumanEmotionsCondition };

        InvokeRepeating(nameof(ShowUpgrade), 0, 1);
    }
}

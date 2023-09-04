using UnityEngine;
using UnityEngine.UI;

public class GiftedChild : Building
{
    [SerializeField]
    private GameObject upgradeFormatter;
    private Condition upgradeFormatterCondition;
    [SerializeField]
    private GameObject upgradeAutomatic;
    private Condition upgradeAutomaticCondition;
    [SerializeField]
    private GameObject upgradeLinter;
    private Condition upgradeLinterCondition;
    [SerializeField]
    private GameObject upgradeCustomRules;
    private Condition upgradeCustomRulesCondition;

    public GiftedChild()
    {
        LOCAdded = 500;
        BuyCost = 2000;
        SellCost = 1000;
    }

    void Start()
    {
        SetupDefaults();
        SetupUpgrades();
        SetupConditions();
    }

    public override void SetupUpgrades()
    {
        upgradeFormatter.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeFormatter, 100, 1500));
        upgradeAutomatic.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeAutomatic, (ulong)(LOCAdded * 0.1), 1000));
        upgradeLinter.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeLinter, 200, 3000));
        upgradeCustomRules.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeCustomRules, (ulong)(LOCAdded * 0.15), 1500));

        upgrades = new GameObject[] { upgradeFormatter, upgradeAutomatic, upgradeLinter, upgradeCustomRules };
    }

    public override void SetupConditions()
    {
        upgradeFormatterCondition = new Condition(this, 5, 80000, null);
        upgradeAutomaticCondition = new Condition(this, 20, 500000, upgradeFormatterCondition);
        upgradeLinterCondition = new Condition(this, 40, 800000, null);
        upgradeCustomRulesCondition = new Condition(this, 60, 2000000, upgradeLinterCondition);

        conditions = new Condition[] { upgradeFormatterCondition, upgradeAutomaticCondition, upgradeLinterCondition, upgradeCustomRulesCondition };

        InvokeRepeating(nameof(ShowUpgrade), 0, 1);
    }
}

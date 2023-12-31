using UnityEngine;
using UnityEngine.UI;

public class Key : Building
{
    [SerializeField] private GameObject upgrade10Fingers;
    private Condition upgrade10FingersCondition;
    [SerializeField] private GameObject upgradeGoogleAnswers;
    private Condition upgradeGoogleAnswersCondition;
    [SerializeField] private GameObject upgradeCopilot;
    private Condition upgradeCopilotCondition;
    [SerializeField] private GameObject upgradeGPT;
    private Condition upgradeGPTCondition;
    [SerializeField] private GameObject upgradeRefactorCode;
    private Condition upgradeRefactorCodeCondition;

    public Key()
    {
        LOCAdded = 1;
        BuyCost = 10;
        SellCost = 5;
    }

    void Start()
    {
        SetupDefaults();
        SetupUpgrades();
        SetupConditions();
    }

    public override void SetupUpgrades()
    {
        upgrade10Fingers.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgrade10Fingers, 4, 10));
        upgradeGoogleAnswers.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeGoogleAnswers, 5, 20));
        upgradeCopilot.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeCopilot, 5, 20));
        upgradeGPT.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeGPT, 10, 50));
        upgradeRefactorCode.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeRefactorCode, 15, 100));

        upgrades = new GameObject[] { upgrade10Fingers, upgradeGoogleAnswers, upgradeCopilot, upgradeGPT, upgradeRefactorCode };
    }

    public override void SetupConditions()
    {
        upgrade10FingersCondition = new Condition(this, 2, 50, null);
        upgradeGoogleAnswersCondition = new Condition(this, 10, 150, null);
        upgradeCopilotCondition = new Condition(this, 20, 500, null);
        upgradeGPTCondition = new Condition(this, 30, 1000, null);
        upgradeRefactorCodeCondition = new Condition(this, 50, 10000, null);

        conditions = new Condition[] { upgrade10FingersCondition, upgradeGoogleAnswersCondition, upgradeCopilotCondition, upgradeGPTCondition, upgradeRefactorCodeCondition };

        InvokeRepeating(nameof(ShowUpgrade), 0, 1);
    }

    public ulong GetLOCAdded()
    {
        return LOCAdded;
    }
}

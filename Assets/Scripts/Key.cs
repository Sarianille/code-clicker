using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : Building
{
    [SerializeField]
    private GameObject upgrade10Fingers;
    private Condition upgrade10FingersCondition;
    [SerializeField]
    private GameObject upgradeGoogleAnswers;
    private Condition upgradeGoogleAnswersCondition;
    [SerializeField]
    private GameObject upgradeCopilot;
    private Condition upgradeCopilotCondition;
    [SerializeField]
    private GameObject upgradeGPT;
    private Condition upgradeGPTCondition;
    [SerializeField]
    private GameObject upgradeRefactorCode;
    private Condition upgradeRefactorCodeCondition;

    public Key()
    {
        LOCAdded = 1;
        LOCAddedDefault = 1;
        BuyCost = 10;
        BuyCostDefault = 10;
        SellCost = 5;
        SellCostDefault = 5;
        AppearNextMinimum = 100;
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
        upgrade10Fingers.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgrade10Fingers, 4, 10));
        upgradeGoogleAnswers.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeGoogleAnswers, 5, 20));
        upgradeCopilot.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeCopilot, 5, 20));
        upgradeGPT.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeGPT, 10, 50));
        upgradeRefactorCode.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeRefactorCode, 15, 100));

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

        InvokeRepeating("ShowUpgrade", 0, 1);
    }

    public ulong GetLOCAdded()
    {
        return LOCAdded;
    }
}

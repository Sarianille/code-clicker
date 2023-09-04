using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractorTeam : Building
{
    [SerializeField]
    private GameObject upgradeGitLecture;
    private Condition upgradeGitLectureCondition;
    [SerializeField]
    private GameObject upgradeHomeOffice;
    private Condition upgradeHomeOfficeCondition;
    [SerializeField]
    private GameObject upgradeRemote;
    private Condition upgradeRemoteCondition;

    public ContractorTeam()
    {
        LOCAdded = 10000;
        LOCAddedDefault = LOCAdded;
        BuyCost = 50000;
        BuyCostDefault = BuyCost;
        SellCost = 25000;
        SellCostDefault = SellCost;
        AppearNextMinimum = 100000000;
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
        upgradeGitLecture.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeGitLecture, 1000, 50000));
        upgradeHomeOffice.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeHomeOffice, 1000, 50000));
        upgradeRemote.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeRemote, (ulong)(LOCAdded * 0.1), 100000));

        upgrades = new GameObject[] { upgradeGitLecture, upgradeHomeOffice, upgradeRemote };
    }

    public override void SetupConditions()
    {
        upgradeGitLectureCondition = new Condition(this, 5, 5000000, null);
        upgradeHomeOfficeCondition = new Condition(this, 20, 5000000000, null);
        upgradeRemoteCondition = new Condition(this, 50, 1000000000000, null);

        conditions = new Condition[] { upgradeGitLectureCondition, upgradeHomeOfficeCondition, upgradeRemoteCondition };

        InvokeRepeating("ShowUpgrade", 0, 1);
    }
}

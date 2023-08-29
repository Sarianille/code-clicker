using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MFFStudent : Building
{
    [SerializeField]
    private GameObject upgradeCredits;
    private Condition upgradeCreditsCondition;
    [SerializeField]
    private GameObject upgradeFinalProject;
    private Condition upgradeFinalProjectCondition;

    public MFFStudent()
    {
        LOCAdded = 1000;
        BuyCost = 10000;
        SellCost = 5000;
        AppearNextMinimum = 1000000;
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
        upgradeCredits.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeCredits, 500, 5000));
        upgradeFinalProject.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeFinalProject, (ulong)(LOCAdded * 0.2), 10000));

        upgrades = new GameObject[] { upgradeCredits, upgradeFinalProject };
    }

    public override void SetupConditions()
    {
        upgradeCreditsCondition = new Condition(this, 10, 1000000, null);
        upgradeFinalProjectCondition = new Condition(this, 40, 10000000, null);

        conditions = new Condition[] { upgradeCreditsCondition, upgradeFinalProjectCondition };

        InvokeRepeating("ShowUpgrade", 0, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamMember : Building
{
    [SerializeField]
    private GameObject upgradePeerReview;
    private Condition upgradePeerReviewCondition;
    [SerializeField]
    private GameObject upgradeCodeReview;
    private Condition upgradeCodeReviewCondition;
    [SerializeField]
    private GameObject upgradeSocks;
    private Condition upgradeSocksCondition;

    public TeamMember()
    {
        LOCAdded = 5000;
        BuyCost = 15000;
        SellCost = 7500;
        AppearNextMinimum = 10000000;
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
        upgradePeerReview.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradePeerReview, 700, 25000));
        upgradeCodeReview.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeCodeReview, 1200, 50000));
        upgradeSocks.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(this, upgradeSocks, (ulong)(LOCAdded * 0.15), 100000));

        upgrades = new GameObject[] { upgradePeerReview, upgradeCodeReview, upgradeSocks };
    }

    public override void SetupConditions()
    {
        upgradePeerReviewCondition = new Condition(this, 5, 10000000, null);
        upgradeCodeReviewCondition = new Condition(this, 20, 50000000, upgradePeerReviewCondition);
        upgradeSocksCondition = new Condition(this, 50, 1000000000, null);

        conditions = new Condition[] { upgradePeerReviewCondition, upgradeCodeReviewCondition, upgradeSocksCondition };

        InvokeRepeating("ShowUpgrade", 0, 1);
    }
}

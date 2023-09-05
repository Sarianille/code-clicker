using UnityEngine;
using UnityEngine.UI;

public class TeamMember : Building
{
    [SerializeField] private GameObject upgradePeerReview;
    private Condition upgradePeerReviewCondition;
    [SerializeField] private GameObject upgradeCodeReview;
    private Condition upgradeCodeReviewCondition;
    [SerializeField] private GameObject upgradeSocks;
    private Condition upgradeSocksCondition;

    public TeamMember()
    {
        LOCAdded = 5000;
        BuyCost = 15000;
        SellCost = 7500;
    }

    void Start()
    {
        SetupDefaults();
        SetupUpgrades();
        SetupConditions();
    }

    public override void SetupUpgrades()
    {
        upgradePeerReview.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradePeerReview, 700, 25000));
        upgradeCodeReview.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeCodeReview, 1200, 50000));
        upgradeSocks.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeSocks, (ulong)(LOCAdded * 0.15), 100000));

        upgrades = new GameObject[] { upgradePeerReview, upgradeCodeReview, upgradeSocks };
    }

    public override void SetupConditions()
    {
        upgradePeerReviewCondition = new Condition(this, 5, 10000000, null);
        upgradeCodeReviewCondition = new Condition(this, 20, 50000000, upgradePeerReviewCondition);
        upgradeSocksCondition = new Condition(this, 50, 1000000000, null);

        conditions = new Condition[] { upgradePeerReviewCondition, upgradeCodeReviewCondition, upgradeSocksCondition };

        InvokeRepeating(nameof(ShowUpgrade), 0, 1);
    }
}

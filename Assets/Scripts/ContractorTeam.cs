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
        BuyCost = 50000;
        SellCost = 25000;
    }

    void Start()
    {
        SetupDefaults();
        SetupUpgrades();
        SetupConditions();
    }

    public override void SetupUpgrades()
    {
        upgradeGitLecture.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeGitLecture, 1000, 50000));
        upgradeHomeOffice.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeHomeOffice, 1000, 50000));
        upgradeRemote.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeRemote, (ulong)(LOCAdded * 0.1), 100000));

        upgrades = new GameObject[] { upgradeGitLecture, upgradeHomeOffice, upgradeRemote };
    }

    public override void SetupConditions()
    {
        upgradeGitLectureCondition = new Condition(this, 5, 5000000, null);
        upgradeHomeOfficeCondition = new Condition(this, 20, 5000000000, null);
        upgradeRemoteCondition = new Condition(this, 50, 1000000000000, null);

        conditions = new Condition[] { upgradeGitLectureCondition, upgradeHomeOfficeCondition, upgradeRemoteCondition };

        InvokeRepeating(nameof(ShowUpgrade), 0, 1);
    }
}

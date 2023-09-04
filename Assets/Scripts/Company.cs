using UnityEngine;
using UnityEngine.UI;

public class Company : Building
{
    [SerializeField]
    private GameObject upgradeQuittingVim;
    private Condition upgradeQuittingVimCondition;
    [SerializeField]
    private GameObject upgradeCoffeeMachine;
    private Condition upgradeCoffeeMachineCondition;
    [SerializeField]
    private GameObject upgradeSnacks;
    private Condition upgradeSnacksCondition;
    [SerializeField]
    private GameObject upgradeFreeLunch;
    private Condition upgradeFreeLunchCondition;
    [SerializeField]
    private GameObject upgradeGym;
    private Condition upgradeGymCondition;
    [SerializeField]
    private GameObject upgradeUgh;
    private Condition upgradeUghCondition;
    [SerializeField]
    private GameObject upgradeBars;
    private Condition upgradeBarsCondition;

    public Company()
    {
        LOCAdded = 50000;
        BuyCost = 100000;
        SellCost = 50000;
    }

    void Start()
    {
        SetupDefaults();
        SetupUpgrades();
        SetupConditions();
    }

    public override void SetupUpgrades()
    {
        upgradeQuittingVim.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeQuittingVim, 5000, 500000));
        upgradeCoffeeMachine.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeCoffeeMachine, (ulong)(LOCAdded * 0.2), 1000000));
        upgradeSnacks.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeSnacks, 5000, 550000));
        upgradeFreeLunch.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeFreeLunch, 7000, 1000000));
        upgradeGym.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeGym, 4000, 500000));
        upgradeUgh.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeUgh, (ulong)(LOCAdded * 0.2), 5000000));
        upgradeBars.GetComponentInChildren<Button>().onClick.AddListener(() => AddUpgrade(upgradeBars, (ulong)(LOCAdded * 0.2), 3000000));

        upgrades = new GameObject[] { upgradeQuittingVim, upgradeCoffeeMachine, upgradeSnacks, upgradeFreeLunch, upgradeGym, upgradeUgh, upgradeBars };
    }

    public override void SetupConditions()
    {
        upgradeQuittingVimCondition = new Condition(this, 5, 1000000000, null);
        upgradeCoffeeMachineCondition = new Condition(this, 10, 5000000000, null);
        upgradeSnacksCondition = new Condition(this, 15, 10000000000, null);
        upgradeFreeLunchCondition = new Condition(this, 20, 50000000000, null);
        upgradeGymCondition = new Condition(this, 25, 100000000000, null);
        upgradeUghCondition = new Condition(this, 45, 1000000000000, null);
        upgradeBarsCondition = new Condition(this, 50, 5000000000000, upgradeUghCondition);

        conditions = new Condition[] { upgradeQuittingVimCondition, upgradeCoffeeMachineCondition, upgradeSnacksCondition, upgradeFreeLunchCondition, upgradeGymCondition, upgradeUghCondition, upgradeBarsCondition };

        InvokeRepeating(nameof(ShowUpgrade), 0, 1);
    }
}

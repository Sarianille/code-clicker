using System;
using TMPro;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    protected ulong LOCAdded;
    protected ulong LOCAddedDefault;
    protected ulong BuyCost;
    protected ulong BuyCostDefault;
    protected ulong SellCost;
    protected ulong SellCostDefault;
    protected ulong Amount = 0;

    public float Multiplier = 1;

    protected int UpgradeAndConditionCounter = 0;
    protected GameObject[] upgrades;
    protected Condition[] conditions;

    public Clicker clicker;

    [SerializeField] protected TMP_Text BuyCostText;
    [SerializeField] protected TMP_Text SellCostText;
    [SerializeField] protected TMP_Text AmountText;

    public abstract void SetupUpgrades();
    public abstract void SetupConditions();
    public static ulong ToTwosComplement(ulong input) => ~input + 1;
    public ulong GetAmount() => Amount;
    public bool EnoughMoney(ulong cost) => clicker.currentLOCCount >= cost;
    public ulong SendLOC() => (ulong)((Amount * LOCAdded) * Multiplier);

    public void Buy()
    {
        if (EnoughMoney(BuyCost))
        {
            clicker.currentLOCCount -= BuyCost;
            clicker.ManageLOCServerRpc(ToTwosComplement(BuyCost), clicker.isServer);

            AdjustCosts((x, y) => (ulong)(x * y));
            Amount++;
            RefreshText();
        }
    }

    public void Sell()
    {
        if (Amount > 0)
        {
            clicker.currentLOCCount += SellCost;
            clicker.ManageLOCServerRpc(SellCost, clicker.isServer);
            Amount--;

            AdjustCosts((x, y) => (ulong)(x / y));
            RefreshText();
        }
    }

    private void AdjustCosts(Func<double, double, ulong> operation)
    {
        if (Amount != 0)
        {

            SellCost = operation(SellCost, 1.5);
        }

        BuyCost = operation(BuyCost, 1.5);
    }

    private void RefreshText()
    {
        BuyCostText.text = "Buy: " + clicker.numberSuffixes.FormatNumber(BuyCost);
        SellCostText.text = "Sell: " + clicker.numberSuffixes.FormatNumber(SellCost);
        AmountText.text = Amount.ToString();
    }

    protected void AddUpgrade(GameObject upgrade, ulong amountAdded, ulong cost)
    {
        if (EnoughMoney(cost))
        {
            LOCAdded += amountAdded;
            clicker.currentLOCCount -= cost;
            clicker.ManageLOCServerRpc(ToTwosComplement(cost), clicker.isServer);
            upgrade.SetActive(false);
        }
    }

    protected void ShowUpgrade()
    {
        if (UpgradeAndConditionCounter >= upgrades.Length)
        {
            CancelInvoke(nameof(ShowUpgrade));
        }

        if (conditions[UpgradeAndConditionCounter].IsMet())
        {
            upgrades[UpgradeAndConditionCounter].SetActive(true);
            UpgradeAndConditionCounter++;
        }
    }

    protected void SetupDefaults()
    {
        LOCAddedDefault = LOCAdded;
        BuyCostDefault = BuyCost;
        SellCostDefault = SellCost;
    }

    public void ResetToDefault()
    {
        LOCAdded = LOCAddedDefault;
        BuyCost = BuyCostDefault;
        SellCost = SellCostDefault;
        Amount = 0;
        UpgradeAndConditionCounter = 0;

        RefreshText();

        foreach (var upgrade in upgrades)
        {
            upgrade.SetActive(false);
        }

        if (!IsInvoking(nameof(ShowUpgrade)))
        {
            InvokeRepeating(nameof(ShowUpgrade), 0, 1);
        }
    }
}
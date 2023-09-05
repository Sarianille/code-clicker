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

    /// <summary>
    /// Assigns the correct values to upgrades.
    /// </summary>
    public abstract void SetupUpgrades();
    /// <summary>
    /// Assigns the correct values to conditions.
    /// </summary>
    public abstract void SetupConditions();
    /// <summary>
    /// Converts a number to two's complement.
    /// </summary>
    /// <param name="input">The number to be converted.</param>
    /// <returns>The converted number.</returns>
    private static ulong ToTwosComplement(ulong input) => ~input + 1;
    /// <summary>
    /// Returns how many buildings are owned.
    /// </summary>
    /// <returns>Building amount.</returns>
    public ulong GetAmount() => Amount;
    /// <summary>
    /// Checks whether the player has enough LOC.
    /// </summary>
    /// <param name="cost">The cost of what is bought.</param>
    /// <returns>Whether the player has enough LOC.</returns>
    public bool EnoughMoney(ulong cost) => clicker.currentLOCCount >= cost;
    /// <summary>
    /// Calculates how many LOC the buildings of this type are producing per second, including possible boosts.
    /// </summary>
    /// <returns>LOC amount.</returns>
    public ulong SendLOC() => (ulong)((Amount * LOCAdded) * Multiplier);

    /// <summary>
    /// Buys a building.
    /// </summary>
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

    /// <summary>
    /// Sells a building.
    /// </summary>
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

    /// <summary>
    /// Adjusts the costs of buying and selling a building.
    /// </summary>
    /// <param name="operation">The operation to be executed based on whether the player is buying or selling.</param>
    private void AdjustCosts(Func<double, double, ulong> operation)
    {
        // Do not increase the sell cost for the first building
        if (Amount != 0)
        {

            SellCost = operation(SellCost, 1.5);
        }

        BuyCost = operation(BuyCost, 1.5);
    }

    /// <summary>
    /// Ensures that the text is up to date.
    /// </summary>
    private void RefreshText()
    {
        BuyCostText.text = "Buy: " + clicker.numberSuffixes.FormatNumber(BuyCost);
        SellCostText.text = "Sell: " + clicker.numberSuffixes.FormatNumber(SellCost);
        AmountText.text = Amount.ToString();
    }

    /// <summary>
    /// Adds an upgrade to the building.
    /// </summary>
    /// <param name="upgrade">Which upgrade is being added.</param>
    /// <param name="amountAdded">How much LOC the upgrade adds.</param>
    /// <param name="cost">How much the upgrade costs.</param>
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

    /// <summary>
    /// Changes the visibility of upgrades based on whether their conditions are met.
    /// Stops invoking itself when all upgrades have been shown.
    /// </summary>
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

    /// <summary>
    /// Sets the default values of the building.
    /// </summary>
    protected void SetupDefaults()
    {
        LOCAddedDefault = LOCAdded;
        BuyCostDefault = BuyCost;
        SellCostDefault = SellCost;
    }

    /// <summary>
    /// Resets the building to its default state.
    /// </summary>
    public void ResetToDefault()
    {
        LOCAdded = LOCAddedDefault;
        BuyCost = BuyCostDefault;
        SellCost = SellCostDefault;
        Amount = 0;
        UpgradeAndConditionCounter = 0;

        RefreshText();

        // In case unbought upgrades are visible
        foreach (var upgrade in upgrades)
        {
            upgrade.SetActive(false);
        }

        // In case we have bought all upgrades in the previous session
        if (!IsInvoking(nameof(ShowUpgrade)))
        {
            InvokeRepeating(nameof(ShowUpgrade), 0, 1);
        }
    }
}
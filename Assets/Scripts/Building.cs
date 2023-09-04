using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Building : MonoBehaviour
{
    protected ulong LOCAdded;
    protected ulong LOCAddedDefault;
    public ulong Amount = 0;
    public ulong BuyCost;
    protected ulong BuyCostDefault;
    protected ulong SellCost;
    protected ulong SellCostDefault;
    protected ulong AppearNextMinimum;
    public float Multiplier = 1;

    protected int UpgradeAndConditionCounter = 0;
    protected GameObject[] upgrades;
    protected Condition[] conditions;

    public Clicker clicker;
    [SerializeField]
    protected GameObject building;

    [SerializeField]
    protected TMP_Text BuyCostText;
    [SerializeField]
    protected TMP_Text SellCostText;
    [SerializeField]
    protected TMP_Text AmountText;

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

    public void RefreshText()
    {
        BuyCostText.text = "Buy: " + clicker.numberSuffixes.FormatNumber(BuyCost);
        SellCostText.text = "Sell: " + clicker.numberSuffixes.FormatNumber(SellCost);
        AmountText.text = Amount.ToString();
    }

    public ulong SendLOC()
    {
        ulong LOC = (ulong)((Amount * LOCAdded) * Multiplier);

        return LOC;
    }

    public void AppearNext()
    {
        if (clicker.overallLOCCount > AppearNextMinimum)
        {
            building.SetActive(true);
            CancelInvoke("AppearNext");
        }
        else
        {
            building.SetActive(false);
        }
    }

    public void AddUpgrade(Building building, GameObject upgrade, ulong amountAdded, ulong cost)
    {
        if (EnoughMoney(cost))
        {
            building.LOCAdded += amountAdded;
            clicker.currentLOCCount -= cost;
            clicker.ManageLOCServerRpc(ToTwosComplement(cost), clicker.isServer);
            upgrade.SetActive(false);
        }
    }

    public bool EnoughMoney(ulong cost)
    {
        return clicker.currentLOCCount >= cost;
    }

    public abstract void SetupUpgrades();
    public abstract void SetupConditions();
    public void ShowUpgrade()
    {
        if (UpgradeAndConditionCounter >= upgrades.Length)
        {
            CancelInvoke("ShowUpgrade");
        }

        if (conditions[UpgradeAndConditionCounter].IsMet())
        {
            upgrades[UpgradeAndConditionCounter].SetActive(true);
            UpgradeAndConditionCounter++;
        }
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

        if (!IsInvoking("ShowUpgrade"))
        {
            InvokeRepeating("ShowUpgrade", 0, 1);
        }

        if (!IsInvoking("AppearNext"))
        {
            InvokeRepeating("AppearNext", 0, 1);
        }
    }

    public static ulong ToTwosComplement(ulong input) => ~input + 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
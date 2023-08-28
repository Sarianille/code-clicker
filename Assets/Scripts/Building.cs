using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    protected ulong LOCAdded;
    protected int Amount = 0;
    protected ulong BuyCost;
    protected ulong SellCost;
    protected ulong AppearNextMinimum;

    [SerializeField]
    protected Clicker clicker = new Clicker();
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
        if (clicker.currentLOCCount >= BuyCost)
        {
            clicker.currentLOCCount -= BuyCost;

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
        clicker.LOCCount.text = clicker.numberSuffixes.FormatNumber(clicker.currentLOCCount);
        BuyCostText.text = "Buy: " + clicker.numberSuffixes.FormatNumber(BuyCost);
        SellCostText.text = "Sell: " + clicker.numberSuffixes.FormatNumber(SellCost);
        AmountText.text = Amount.ToString();
    }

    public void SendLOC()
    {
        ulong LOC = (ulong)Amount * LOCAdded;
        clicker.LOCPerSecond += LOC;
    }

    public void AppearNext()
    {
        if (clicker.overallLOCCount > AppearNextMinimum)
        {
            building.SetActive(true);
            CancelInvoke("AppearNext");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
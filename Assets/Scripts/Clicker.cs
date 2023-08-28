using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public ulong overallLOCCount = 0;
    public ulong currentLOCCount = 0;
    private ulong LOCFromClicking = 0;
    public TMP_Text LOCCount;

    public ulong LOCPerSecond = 0;
    public TMP_Text LOCPErSecondText;

    public NumberSuffixes numberSuffixes = new NumberSuffixes();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BuildingsOwned())
        {
            if (!IsInvoking("AddFromBuildings"))
            {
                InvokeRepeating("AddFromBuildings", 0, 1);
            }
        }
        else
        {
            CancelInvoke("AddFromBuildings");
        }
    }

    public void AddFromClick()
    {
        IncrementLOC(1);
        LOCFromClicking++;
    }

    public void IncrementLOC(ulong LOCAdded)
    {
        overallLOCCount += LOCAdded;
        currentLOCCount += LOCAdded;
        LOCCount.text = numberSuffixes.FormatNumber(currentLOCCount);
    }

    private bool BuildingsOwned()
    {
        return LOCPerSecond > 0;
    }

    private void AddFromBuildings()
    {
        LOCPErSecondText.text = "+" + numberSuffixes.FormatNumber(LOCPerSecond);
        LOCPErSecondText.CrossFadeAlpha(1, 0, false);
        IncrementLOC(LOCPerSecond);
        LOCPerSecond = 0;
        LOCPErSecondText.CrossFadeAlpha(0, 0.8f, false);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Clicker : NetworkBehaviour
{
    public ulong overallLOCCount = 0;
    public ulong currentLOCCount = 0;
    public ulong clicks = 0;
    public TMP_Text LOCCount;

    public ulong LOCPerSecond = 0;
    public TMP_Text LOCPErSecondText;

    public NumberSuffixes numberSuffixes;
    public ulong ClickMultiplier = 1;
    public ulong ProductionMultiplier = 1;

    public Key key;
    public URandom urandom;
    public CodeMonkey codeMonkey;
    public GiftedChild giftedChild;
    public MFFStudent MFFStudent;
    public TeamMember teamMember;
    public ContractorTeam contractorTeam;
    public Company company;
    public AI ai;
    public Building[] buildings;

    public Notification notification;

    // Start is called before the first frame update
    void Start()
    {
        buildings = new Building[] { key, urandom, codeMonkey, giftedChild, MFFStudent, teamMember, contractorTeam, company, ai };
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

        ChangeTextOfClientClientRpc();
    }

    public void AddFromClick()
    {
        IncrementLOC(key.GetLOCAdded() * ClickMultiplier);
        clicks++;
    }

    public void IncrementLOC(ulong LOCAdded)
    {
        try
        {
            checked 
            {
                overallLOCCount += LOCAdded;
                currentLOCCount += LOCAdded;
            }
        }
        catch (OverflowException ex)
        {
            //show congrats u broke the game screen
        }
    }

    private bool BuildingsOwned()
    {
        return buildings.Any(building => building.Amount > 0);
    }

    private void AddFromBuildings()
    {
        CollectLOCFromBuildings();
        ulong LOCAdded = LOCPerSecond * ProductionMultiplier;
        LOCPErSecondText.text = "+" + numberSuffixes.FormatNumber(LOCAdded);
        LOCPErSecondText.CrossFadeAlpha(1, 0, false);
        IncrementLOC(LOCAdded);
        SendLOCToServerServerRpc(LOCAdded);
        LOCPerSecond = 0;
        LOCPErSecondText.CrossFadeAlpha(0, 0.8f, false);
    }

    private void CollectLOCFromBuildings()
    {
        foreach (var building in buildings)
        {
            LOCPerSecond += building.SendLOC();
        }
    }

    [ServerRpc]
    private void SendLOCToServerServerRpc(ulong LOCAdded)
    {
        IncrementLOC(LOCAdded);
    }

    [ClientRpc]
    private void ChangeTextOfClientClientRpc()
    {
        LOCCount.text = numberSuffixes.FormatNumber(currentLOCCount);
    }
}
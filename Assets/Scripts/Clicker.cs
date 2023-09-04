using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class Clicker : NetworkBehaviour
{
    public ulong overallLOCCount = 0;
    public ulong currentLOCCount = 0;
    public ulong clicks = 0;
    public TMP_Text LOCCount;

    public ulong LOCPerSecond = 0;
    public TMP_Text LOCPErSecondText;
    private ulong LOCFromOthers = 0;
    public TMP_Text LOCFromOthersText;

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

    public bool isServer;

    private ulong[] appearNextMinimum = { 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        buildings = new Building[] { key, urandom, codeMonkey, giftedChild, MFFStudent, teamMember, contractorTeam, company, ai };
        InvokeRepeating("ManageBuildingVisibility", 0, 0.5f);
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

        LOCCount.text = numberSuffixes.FormatNumber(currentLOCCount);
    }

    public void AddFromClick()
    {
        IncrementLOC(key.GetLOCAdded() * ClickMultiplier);
        SendLOCToServerServerRpc(key.GetLOCAdded() * ClickMultiplier, isServer);
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

                ChangeTextOfClientClientRpc(overallLOCCount, currentLOCCount);
            }
        }
        catch (OverflowException ex)
        {
            //show congrats u broke the game screen
        }
    }

    private bool BuildingsOwned()
    {
        return buildings.Any(building => building.GetAmount() > 0);
    }

    private void AddFromBuildings()
    {
        CollectLOCFromBuildings();
        ulong LOCAdded = LOCPerSecond * ProductionMultiplier;

        IncrementLOC(LOCAdded);
        SendLOCToServerServerRpc(LOCAdded, isServer);
        ChangeLOCFromOthersClientRpc(LOCFromOthers);

        LOCPErSecondText.text = "+" + numberSuffixes.FormatNumber(LOCAdded);
        LOCPErSecondText.CrossFadeAlpha(1, 0, false);
        LOCPErSecondText.CrossFadeAlpha(0, 0.8f, false);

        if (LOCFromOthers != 0)
        {
            LOCFromOthersText.text = "Teammates: +" + numberSuffixes.FormatNumber(LOCFromOthers);
            LOCFromOthersText.CrossFadeAlpha(1, 0, false);
            LOCFromOthersText.CrossFadeAlpha(0, 0.8f, false);
        }

        LOCPerSecond = 0;
        LOCFromOthers = 0;
    }

    private void CollectLOCFromBuildings()
    {
        foreach (var building in buildings)
        {
            LOCPerSecond += building.SendLOC();
        }
    }

    public void Restart()
    {
        overallLOCCount = 0;
        currentLOCCount = 0;
        clicks = 0;

        foreach (var building in buildings)
        {
            if (building.gameObject.activeSelf)
            {
                building.ResetToDefault();
            }
        }
    }

    private void ManageBuildingVisibility()
    {
        if (counter >= appearNextMinimum.Length)
        {
            CancelInvoke("ManageBuildingVisibility");
        }

        if (overallLOCCount > appearNextMinimum[counter])
        {
            buildings[counter + 1].gameObject.SetActive(true);
            counter++;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendLOCToServerServerRpc(ulong LOCAdded, bool isServer)
    {
        if (!isServer)
        {
            IncrementLOC(LOCAdded);
            LOCFromOthers += LOCAdded;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ManageLOCServerRpc(ulong LOC, bool isServer)
    {
        if (!isServer)
        {
            currentLOCCount += LOC;
            ChangeTextOfClientClientRpc(overallLOCCount, currentLOCCount);
        }
    }

    [ClientRpc]
    private void ChangeTextOfClientClientRpc(ulong overallLOC, ulong currentLOC)
    {
        overallLOCCount = overallLOC;
        currentLOCCount = currentLOC;
    }

    [ClientRpc]
    private void ChangeLOCFromOthersClientRpc(ulong currentLOCFromOthers)
    {
        LOCFromOthers = currentLOCFromOthers;
    }
}
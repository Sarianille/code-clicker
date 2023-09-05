using System;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Clicker : NetworkBehaviour
{
    public ulong overallLOCCount = 0;
    public ulong currentLOCCount = 0;
    public ulong clicks = 0;
    [SerializeField] private TMP_Text LOCCount;

    private ulong LOCPerSecond = 0;
    private ulong LOCFromOthers = 0;
    [SerializeField] private TMP_Text LOCPerSecondText;
    [SerializeField] private TMP_Text LOCFromOthersText;

    public ulong ClickMultiplier = 1;
    public ulong ProductionMultiplier = 1;

    [SerializeField] private Key key;
    [SerializeField] private URandom urandom;
    [SerializeField] private CodeMonkey codeMonkey;
    [SerializeField] private GiftedChild giftedChild;
    [SerializeField] private MFFStudent MFFStudent;
    [SerializeField] private TeamMember teamMember;
    [SerializeField] private ContractorTeam contractorTeam;
    [SerializeField] private Company company;
    [SerializeField] private AI ai;
    public Building[] buildings;

    public NumberSuffixes numberSuffixes;
    public Notification notification;

    public bool isServer;

    private ulong[] appearNextMinimum = { 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000 };
    private int counter = 0;

    void Start()
    {
        buildings = new Building[] { key, urandom, codeMonkey, giftedChild, MFFStudent, teamMember, contractorTeam, company, ai };
        InvokeRepeating(nameof(ManageBuildingVisibility), 0, 0.5f);
    }

    void Update()
    {
        if (BuildingsOwned())
        {
            if (!IsInvoking(nameof(AddFromBuildings)))
            {
                InvokeRepeating(nameof(AddFromBuildings), 0, 1);
            }
        }
        else
        {
            CancelInvoke(nameof(AddFromBuildings));
        }

        LOCCount.text = numberSuffixes.FormatNumber(currentLOCCount);
    }

    private bool BuildingsOwned() => buildings.Any(building => building.GetAmount() > 0);

    public void AddFromClick()
    {
        AddToLOC(key.GetLOCAdded() * ClickMultiplier);
        SendLOCToServerServerRpc(key.GetLOCAdded() * ClickMultiplier, isServer);
        clicks++;
    }

    private void AddToLOC(ulong LOCAdded)
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
            GameWon();
        }
    }

    private void AddFromBuildings()
    {
        CollectLOCFromBuildings();

        ulong LOCAdded = LOCPerSecond * ProductionMultiplier;

        AddToLOC(LOCAdded);
        SendLOCToServerServerRpc(LOCAdded, isServer);
        ChangeLOCFromOthersClientRpc(LOCFromOthers);

        LOCPerSecondText.text = "+" + numberSuffixes.FormatNumber(LOCAdded);
        LOCPerSecondText.CrossFadeAlpha(1, 0, false);
        LOCPerSecondText.CrossFadeAlpha(0, 0.8f, false);

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

    private void ManageBuildingVisibility()
    {
        if (counter >= appearNextMinimum.Length)
        {
            CancelInvoke(nameof(ManageBuildingVisibility));
        }

        if (overallLOCCount > appearNextMinimum[counter])
        {
            buildings[counter + 1].gameObject.SetActive(true);
            counter++;
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

                if (building is not Key)
                {
                    building.gameObject.SetActive(false);
                }
            }
        }
    }

    private void GameWon()
    {
        notification.ShowMessage("Too many LOC! You won! Restarting...");
        Restart();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendLOCToServerServerRpc(ulong LOCAdded, bool isServer)
    {
        if (!isServer)
        {
            AddToLOC(LOCAdded);
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
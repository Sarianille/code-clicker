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
        // Ensures that the LOC from buildings is only calculated when the player owns buildings
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

    /// <summary>
    /// Checks if the player owns any buildings.
    /// </summary>
    /// <returns>Whether the player owns any buildings.</returns>
    private bool BuildingsOwned() => buildings.Any(building => building.GetAmount() > 0);

    /// <summary>
    /// Upon clicking, adds the correct LOC amount.
    /// </summary>
    public void AddFromClick()
    {
        // Synchronize how many LOC clicks add with the Key building
        AddToLOC(key.GetLOCAdded() * ClickMultiplier);
        SendLOCToServerServerRpc(key.GetLOCAdded() * ClickMultiplier, isServer, isFromClick: true);
        clicks++;
    }

    /// <summary>
    /// Add the amount of LOC.
    /// If the amount of LOC exceeds the maximum value of ulong, the session is won.
    /// </summary>
    /// <param name="LOCAdded"></param>
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
        catch (OverflowException)
        {
            GameWon();
        }
    }

    /// <summary>
    /// Adds the LOC gained from buildings.
    /// Showcases how many LOC were gained this way.
    /// </summary>
    private void AddFromBuildings()
    {
        CollectLOCFromBuildings();

        ulong LOCAdded = LOCPerSecond * ProductionMultiplier;

        AddToLOC(LOCAdded);
        SendLOCToServerServerRpc(LOCAdded, isServer, isFromClick: false);
        ChangeLOCFromOthersClientRpc(LOCFromOthers);
        
        LOCFromOthers -= LOCAdded;

        LOCPerSecondText.text = "+" + numberSuffixes.FormatNumber(LOCAdded);
        LOCPerSecondText.CrossFadeAlpha(1, 0, false);
        LOCPerSecondText.CrossFadeAlpha(0, 0.8f, false);

        // In multiplayer, show how much LOC was added by teammates
        if (LOCFromOthers != 0)
        {
            LOCFromOthersText.text = "Teammates: +" + numberSuffixes.FormatNumber(LOCFromOthers);
            LOCFromOthersText.CrossFadeAlpha(1, 0, false);
            LOCFromOthersText.CrossFadeAlpha(0, 0.8f, false);
        }

        LOCPerSecond = 0;
        LOCFromOthers = 0;
    }

    /// <summary>
    /// Adds up the LOC gained from each buildings into one number.
    /// </summary>
    private void CollectLOCFromBuildings()
    {
        foreach (var building in buildings)
        {
            LOCPerSecond += building.SendLOC();
        }
    }

    /// <summary>
    /// Shows the next building when the LOC count is high enough.
    /// Stops invoking when all buildings are shown.
    /// </summary>
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

    /// <summary>
    /// Restart the game. Reset all values to default, apart from achievements.
    /// </summary>
    public void Restart()
    {
        overallLOCCount = 0;
        currentLOCCount = 0;
        clicks = 0;

        foreach (var building in buildings)
        {
            // Inactive buildings still have dafault values
            if (building.gameObject.activeSelf)
            {
                building.ResetToDefault();

                // The Key building is always active
                if (building is not Key)
                {
                    building.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// If the player has too many LOC, they win and the game restarts in case the player wants to play again.
    /// </summary>
    private void GameWon()
    {
        notification.ShowMessage("Too many LOC! You won! Restarting...");

        Restart();
    }


    /// <summary>
    /// Updates the host's LOC count based on the player's addition.
    /// </summary>
    /// <param name="LOCAdded">How many LOC the player added.</param>
    /// <param name="isServer">Whether the player is the host.</param>
    [ServerRpc(RequireOwnership = false)]
    private void SendLOCToServerServerRpc(ulong LOCAdded, bool isServer, bool isFromClick)
    {
        // The host already has the correct value
        if (!isServer)
        {
            AddToLOC(LOCAdded);
        }

        if (!isFromClick)
        {
            LOCFromOthers += LOCAdded;
        }
    }

    /// <summary>
    /// Manages current LOC count without it affecting the overall count (ex. when buying buildings).
    /// </summary>
    /// <param name="LOC">LOC added or subtracted.</param>
    /// <param name="isServer">Whether the player is the host.</param>
    [ServerRpc(RequireOwnership = false)]
    public void ManageLOCServerRpc(ulong LOC, bool isServer)
    {
        // The host already has the correct value
        if (!isServer)
        {
            currentLOCCount += LOC;
            ChangeTextOfClientClientRpc(overallLOCCount, currentLOCCount);
        }
    }

    /// <summary>
    /// Sends the correct LOC counts from the host to the other players for synchronization.
    /// </summary>
    /// <param name="overallLOC">New overallLOCCount.</param>
    /// <param name="currentLOC">New currentLOCCount.</param>
    [ClientRpc]
    private void ChangeTextOfClientClientRpc(ulong overallLOC, ulong currentLOC)
    {
        overallLOCCount = overallLOC;
        currentLOCCount = currentLOC;
    }

    /// <summary>
    /// Updates the LOC gained from teammates.
    /// </summary>
    /// <param name="currentLOCFromOthers">LOC gained from teammates.</param>
    [ClientRpc]
    private void ChangeLOCFromOthersClientRpc(ulong currentLOCFromOthers)
    {
        LOCFromOthers = currentLOCFromOthers;
    }
}
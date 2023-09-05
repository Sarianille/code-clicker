using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    private Lobby hostLobby;
    private Lobby joinedLobby;

    [SerializeField] private Button hostPublic;
    [SerializeField] private Button hostPrivate;
    [SerializeField] private Button joinPublic;
    [SerializeField] private TMP_InputField joinPrivate;
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private TMP_Text playerAmount;
    [SerializeField] private Notification notification;

    private LobbyEventCallbacks callbacks;
    private bool isHost = false;
    private static readonly string RelayCodeKey = "StartGame_RelayCode";

    private void Start()
    {
        SetupButtons();
        callbacks = new LobbyEventCallbacks();
        callbacks.LobbyChanged += OnLobbyChanged;
        InvokeRepeating(nameof(UpdateLobby), 0, 1.1f);
    }

    /// <summary>
    /// Checks if the player is the lobby host.
    /// </summary>
    /// <returns>Whether the player is the lobby host.</returns>
    private bool IsLobbyHost() => isHost;

    /// <summary>
    /// Updates the player amount based on the current lobby.
    /// </summary>
    public void ChangePlayerAmount() => playerAmount.text = "Players: " + joinedLobby.Players.Count + "/" + joinedLobby.MaxPlayers;

    /// <summary>
    /// Assigns the correct functions to the multiplayer buttons.
    /// </summary>
    private void SetupButtons()
    {
        hostPublic.onClick.AddListener(() => CreateLobby(isPrivate: false));
        hostPrivate.onClick.AddListener(() => CreateLobby(isPrivate: true));
        joinPublic.onClick.AddListener(() => QuickJoinLobby());
        joinPrivate.onEndEdit.AddListener((string lobbyCode) => JoinLobbyByCode(lobbyCode));
    }

    /// <summary>
    /// Creates a new lobby with the given privacy setting.
    /// Updates the corresponding values.
    /// </summary>
    /// <param name="isPrivate">Privacy setting of the lobby.</param>
    private async void CreateLobby(bool isPrivate)
    {
        try
        {
            string lobbyName = "Lobby";
            int maxPlayers = 4;
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions { 
                IsPrivate = isPrivate,
                Data = new Dictionary<string, DataObject>
                {
                    // Set the relay code to 0 to indicate that the game has not started
                    // Ensure it is visisble only to the lobby members
                    { RelayCodeKey, new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, lobbyOptions);
            hostLobby = lobby;
            joinedLobby = hostLobby;

            await Lobbies.Instance.SubscribeToLobbyEventsAsync(joinedLobby.Id, callbacks);

            if (isPrivate)
            {
                codeText.text = "Code: " + lobby.LobbyCode;
            }

            isHost = true;
            notification.ShowMessage("Lobby created.");
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError($"LobbyServiceException: {ex.Message}");
        }
    }

    /// <summary>
    /// Adds the player to a public lobby and updates the corresponding values.
    /// </summary>
    private async void QuickJoinLobby()
    {
        try
        {
            // If no public lobbies are found, send a notification and return
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            if (queryResponse.Results.Count == 0)
            {
                notification.ShowMessage("No lobbies found.");
                return;
            }

            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            joinedLobby = lobby;

            await Lobbies.Instance.SubscribeToLobbyEventsAsync(joinedLobby.Id, callbacks);

            notification.ShowMessage("You joined a lobby.");
            isHost = false;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError($"LobbyServiceException: {ex.Message}");
        }
    }

    /// <summary>
    /// Adds the player to a private lobby with the given code and updates the corresponding values.
    /// </summary>
    /// <param name="lobbyCode">Code of the lobby the players wants to join.</param>
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
            joinedLobby = lobby;

            await Lobbies.Instance.SubscribeToLobbyEventsAsync(joinedLobby.Id, callbacks);

            notification.ShowMessage("You joined the lobby.");
            isHost = false;
        }
        catch (LobbyServiceException ex)
        {
            switch (ex.Reason)
            {
                case LobbyExceptionReason.AlreadySubscribedToLobby:
                    notification.ShowMessage("Already subscribed to lobby.");
                    break;
            }

            Debug.LogError($"LobbyServiceException: {ex.Message}");
        }
    }

    /// <summary>
    /// If in a lobby, checks if the lobby has been updated.
    /// If the lobby host has started the game, joins the relay and leaves the lobby.
    /// </summary>
    private async void UpdateLobby()
    {
        if (joinedLobby is null)
        {
            return;
        }

        Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
        joinedLobby = lobby;
        ChangePlayerAmount();

        if (joinedLobby.Data[RelayCodeKey].Value == "0")
        {
            return;
        }

        // If the lobby host has started the game, join the relay
        // The lobby host joins the relay upon creation
        if (!IsLobbyHost())
        {
            Relay.Instance.JoinRelay(joinedLobby.Data[RelayCodeKey].Value);
        }

        notification.ShowMessage("Game started.");
        joinedLobby = null;
    }

    /// <summary>
    /// Upon clicking, starts the game if the player is the lobby host.
    /// </summary>
    public async void StartGame()
    {
        if (!IsLobbyHost())
        {
            notification.ShowMessage("You are not the lobby host.");
            return;
        }

        if (joinedLobby is null)
        {
            notification.ShowMessage("You are not in a lobby.");
            return;
        }

        try
        {
            string relayCode = await Relay.Instance.CreateRelay(3);

            Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                    {
                        // Set the relay code to the relay code returned by the Relay API so that other players can join
                        { RelayCodeKey, new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
            });
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError($"LobbyServiceException: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends players updates about whether someone joined or left.
    /// </summary>
    /// <param name="changes">Changes made to the lobby.</param>
    private void OnLobbyChanged(ILobbyChanges changes)
    {
        if (changes.PlayerJoined.Changed)
        {
            notification.ShowMessage("Player joined.");
        }
        if (changes.PlayerLeft.Changed) 
        {
            notification.ShowMessage("Player left.");
        }
    }
}

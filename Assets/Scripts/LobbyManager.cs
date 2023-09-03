using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    Lobby hostLobby;
    Lobby joinedLobby;
    float lobbyUpdateTimer = 1.1f;

    public Button hostPublic;
    public Button hostPrivate;
    public Button joinPublic;
    public TMP_InputField joinPrivate;
    public TMP_Text codeText;
    public Notification notification;
    bool isHost = false;

    public TMP_Text playerAmount;
    private LobbyEventCallbacks callbacks;

    private void Start()
    {
        //await UnityServices.InitializeAsync();
        //await AuthenticationService.Instance.SignInAnonymouslyAsync();

        SetupButtons();
        callbacks = new LobbyEventCallbacks();
        callbacks.LobbyChanged += OnLobbyChanged;
    }

    private void Update()
    {
        UpdateLobby();
    }

    private void SetupButtons()
    {
        hostPublic.onClick.AddListener(() => CreateLobby(false));
        hostPrivate.onClick.AddListener(() => CreateLobby(true));
        joinPublic.onClick.AddListener(() => QuickJoinLobby());
        joinPrivate.onEndEdit.AddListener((string lobbyCode) => JoinLobbyByCode(lobbyCode));
    }

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
                    { "StartGame_RelayCode", new DataObject(DataObject.VisibilityOptions.Member, "0") }
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
            Debug.LogError(ex.Message);
        }
    }

    private async void QuickJoinLobby()
    {
        try
        {
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
            Debug.LogError(ex.Message);
        }
    }

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
            Debug.LogError(ex.Message);
        }
    }

    private async void UpdateLobby()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0)
            {
                lobbyUpdateTimer = 1.1f;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
                ChangePlayerAmount();

                if (joinedLobby.Data["StartGame_RelayCode"].Value != "0")
                {
                    if (!IsLobbyHost())
                    {
                        Relay.Instance.JoinRelay(joinedLobby.Data["StartGame_RelayCode"].Value);
                    }

                    joinedLobby = null;
                }
            }
        }
    }

    public async void StartGame()
    {
        if (IsLobbyHost())
        {
            if (joinedLobby == null)
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
                        { "StartGame_RelayCode", new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        else
        {
            notification.ShowMessage("You are not the lobby host.");
        }
    }

    private bool IsLobbyHost()
    {
        return isHost;
        
    }

    private void OnLobbyChanged(ILobbyChanges changes)
    {
        if (changes.PlayerJoined.Changed)
        {
            notification.ShowMessage("Player joined.");
        }
        else if (changes.PlayerLeft.Changed) 
        {
            notification.ShowMessage("Player left.");
        }
    }

    public void ChangePlayerAmount()
    {
        playerAmount.text = "Players: " + joinedLobby.Players.Count + "/" + joinedLobby.MaxPlayers;
    }
}

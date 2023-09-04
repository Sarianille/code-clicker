using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public static Relay Instance { get; private set; }
    private bool isRunning = false;
    public Clicker clicker;
    private async void Start()
    {
        Instance = this;
        await UnityServices.InitializeAsync();

#if UNITY_EDITOR
        if (ParrelSync.ClonesManager.IsClone())
        {
            // When using a ParrelSync clone, switch to a different authentication profile to force the clone
            // to sign in as a different anonymous user account.
            string customArgument = ParrelSync.ClonesManager.GetArgument();
            AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
        }
#endif

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async Task<string> CreateRelay(int numberOfPlayers)
    {
        try
        {
            if (isRunning)
            {
                NetworkManager.Singleton.Shutdown();
                clicker.Restart();
            }

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(numberOfPlayers);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            clicker.isServer = true;

            NetworkManager.Singleton.StartHost();

            isRunning = true;

            return joinCode;
        }
        catch (RelayServiceException ex) 
        {
            Debug.LogError($"RelayServiceException: {ex.Message}");
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            if (isRunning)
            {
                NetworkManager.Singleton.Shutdown();
                clicker.Restart();
            }

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            clicker.isServer = false;

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException ex) 
        {
            Debug.LogError($"RelayServiceException: {ex.Message}");
        }
    }
}

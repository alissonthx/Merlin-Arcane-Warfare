using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour
{
    public static TestRelay Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // await UnityServices.InitializeAsync();

        // AuthenticationService.Instance.SignedIn += () =>
        // {
        //     print("Signed in " + AuthenticationService.Instance.PlayerId);
        // };

        // await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            print(joinCode);

            RelayServerData relayServerData = new RelayServerData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            print(e);
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            print("Joining Relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            // joinAllocation.RelayServer.IpV4,
            // (ushort)joinAllocation.RelayServer.Port,
            // joinAllocation.AllocationIdBytes,
            // joinAllocation.Key,
            // joinAllocation.ConnectionData,
            // joinAllocation.HostConnectionData
            // );

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            print(e);
        }
    }
}
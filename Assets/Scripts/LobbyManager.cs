using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    private Lobby hostLobby;
    private float heartbeatTimer;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            print("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            hostLobby = lobby;

            print("Create Lobby! " + lobby.Name + " " + lobby.MaxPlayers);
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }

    // public async void ListLobbies()
    // {
    //     try
    //     {
    //         QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
    //         {
    //             Count = 25,
    //             Filters = new List<QueryFilter>{
    //                 new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0", QueryFilter.OpOptions.GT)
    //             },
    //         };

    //         QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

    //         print("Lobbies found: " + queryResponse.Results.Count);
    //         foreach (Lobby lobby in queryResponse.Results)
    //         {
    //             // print(lobby.Name + "" + lobby.MaxPlayers);

    //         }
    //     }
    //     catch (LobbyServiceException e)
    //     {
    //         print(e);
    //     }
    // }

    public async Task<List<Lobby>> ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>{
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0", QueryFilter.OpOptions.GT)
            },
            };

            QueryResponse queryResponse = await Unity.Services.Lobbies.Lobbies.Instance.QueryLobbiesAsync();

            print("Lobbies found: " + queryResponse.Results.Count);

            return queryResponse.Results;
        }
        catch (LobbyServiceException e)
        {
            print(e);
            return new List<Lobby>();
        }
    }


    private async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Unity.Services.Lobbies.Lobbies.Instance.QueryLobbiesAsync();
            // print("Lobbies found: " + queryResponse.Results.Count);
            // foreach (Lobby lobby in queryResponse.Results)
            // {
            //     print(lobby.Name + "" + lobby.MaxPlayers);
            // }

            await Unity.Services.Lobbies.Lobbies.Instance.JoinLobbyByCodeAsync(queryResponse.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }

    }

    public async Task<string> StartHostWithRelay(int maxConnections = 5)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
}

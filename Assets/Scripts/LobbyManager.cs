using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    private Lobby joinnedLobby;
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

    public async void ListLobbies()
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

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            print("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                print(lobby.Name + "" + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }
    }

    public async void JoinLobby()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            // print("Lobbies found: " + queryResponse.Results.Count);
            // foreach (Lobby lobby in queryResponse.Results)
            // {
            //     print(lobby.Name + "" + lobby.MaxPlayers);
            // }

            await Lobbies.Instance.JoinLobbyByCodeAsync(queryResponse.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            print(e);
        }

    }

    // async Task<string> CreateRelay()
    // {
    //     Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);
    //     string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

    //     RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

    //     NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
    //     NetworkManager.Singleton.StartHost();

    //     return joinCode;
    // }

    // private async void JoinRelay(string joinCode)
    // {
    //     print("Creating relay");
    //     JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
    //     RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

    //     NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

    //     NetworkManager.Singleton.StartClient();
    // }

    // public async void StartGame()
    // {
    //     string relayCode = await CreateRelay();

    //     Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinnedLobby.Id, new UpdateLobbyOptions
    //     {
    //         Data = new Dictionary<string, DataObject>
    //         {
    //             {"StartGame", new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
    //         }
    //     });

    //     joinnedLobby = lobby;
    // }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Unity.Services.Core;
using TMPro;

public class Lobbies : MonoBehaviour
{
    [SerializeField] private Button update;
    [SerializeField] private GameObject buttonPrefab;

    private void Awake()
    {
        update.onClick.AddListener(async () =>
        {
            await UnityServices.InitializeAsync();
            await UpdateLobbyButtons();
        });
    }

    private async Task UpdateLobbyButtons()
    {
        List<Lobby> lobbies = await LobbyManager.Instance.ListLobbies();

        // Clear existing buttons
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Create buttons for each lobby
        foreach (Lobby lobby in lobbies)
        {
            GameObject buttonGO = Instantiate(buttonPrefab, transform);
            Button button = buttonGO.GetComponent<Button>();
            button.GetComponentInChildren<TMP_Text>().text = lobby.Name;
        }
    }
}

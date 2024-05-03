using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    [SerializeField] private Button create;
    private void Awake()
    {
        create.onClick.AddListener(() =>
       {
           LobbyManager.Instance.CreateLobby();
           _ = LobbyManager.Instance.StartHostWithRelay();
       });
    }
}

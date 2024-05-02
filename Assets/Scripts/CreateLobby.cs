using UnityEngine;
using UnityEngine.UI;

public class CreateLobby : MonoBehaviour
{
    [SerializeField] private Button create;
    private void Awake()
    {
        create.onClick.AddListener(() =>
       {
           LobbyManager.Instance.CreateLobby();
           TestRelay.Instance.CreateRelay();
       });
    }
}

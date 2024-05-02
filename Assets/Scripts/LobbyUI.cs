using UnityEngine;
using UnityEngine.UI;
public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button update;

    private void Awake()
    {
        update.onClick.AddListener(() =>
        {
            LobbyManager.Instance.ListLobbies();
        });
    }

}

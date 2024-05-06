using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyUI : MonoBehaviour
{
    [SerializeField] private Button okButton;
    [SerializeField] private TMP_InputField input;

    private void Awake()
    {
        okButton.onClick.AddListener(() =>
       {
           string input = this.input.text;
           _ = LobbyManager.Instance.StartClientWithRelay(input);
       });
    }
}

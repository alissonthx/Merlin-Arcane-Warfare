using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyUI : MonoBehaviour
{
    [SerializeField] private Button okButton;
    [SerializeField] private TMP_InputField input;

    private void Awake()
    {
        string input = this.input.text;
        print(input);

        okButton.onClick.AddListener(() =>
       {
           _ = LobbyManager.Instance.StartClientWithRelay(input);
       });
    }
}

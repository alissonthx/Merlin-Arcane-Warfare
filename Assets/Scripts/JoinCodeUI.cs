using System;
using TMPro;
using UnityEngine;

public class JoinCodeUI : MonoBehaviour
{
    [SerializeField] TMP_Text joinCodeText;

    private void OnEnable()
    {
        LobbyManager.OnJoinCodeCreated += UpdateJoinCodeText;
    }

    private void OnDisable()
    {
        LobbyManager.OnJoinCodeCreated -= UpdateJoinCodeText;
    }

    private void UpdateJoinCodeText(object sender, EventArgs e)
    {
        string joinCode = LobbyManager.Instance.GetJoinCode();
        if (!string.IsNullOrEmpty(joinCode))
        {
            joinCodeText.text = joinCode;
        }
    }

}

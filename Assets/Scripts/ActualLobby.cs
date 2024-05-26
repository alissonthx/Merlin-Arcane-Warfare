using System;
using UnityEngine;
using UnityEngine.UI;

public class ActualLobby : MonoBehaviour
{
    public static event EventHandler OnLobbyClick;
    private Button actualButton;

    private void Awake()
    {
        actualButton = GetComponent<Button>();

        actualButton.onClick.AddListener(() =>
        {
            OnLobbyClick?.Invoke(this, EventArgs.Empty);
        });
    }
}

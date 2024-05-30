using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator anim;
    private int previousCountdownNumber;
    private string NUMBER_POPUP = "NumberPopup";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
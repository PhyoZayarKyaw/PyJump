using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI powerText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        stageText.text = $"WORLD {GameManager.Instance.world}-{GameManager.Instance.stage}";
        coinText.text = $"x{GameManager.Instance.coins:00}";
        livesText.text = $"LIVES x{GameManager.Instance.lives}";
        timeText.text = $"TIME {Mathf.FloorToInt(GameManager.Instance.timeElapsed):000}";

        if (PowerManager.Instance != null && powerText != null)
        {
            powerText.text = $"POWER: {PowerManager.Instance.power}";
        }
    }
}
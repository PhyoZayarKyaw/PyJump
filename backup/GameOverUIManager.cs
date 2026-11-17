using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private PlayerMovement playerMovement;

    private void Awake()
    {
        Instance = this;
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    public void OnRestartPressed()
    {
        gameOverPanel.SetActive(false);
        GameManager.Instance.RestartLevel();
    }

    public void OnMainMenuPressed()
    {
        gameOverPanel.SetActive(false);
        GameManager.Instance.GoToMainMenu();
    }
}
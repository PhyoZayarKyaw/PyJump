using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject difficultyPanel; // holds EasyButton + HardButton
    public GameObject stageGridPanel;  // holds StageGrid
    public StageSelectManager stageSelectManager;

    private void Awake()
    {
        stageGridPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void SelectEasy()
    {
        EnsureGameManagerExists();
        GameManager.Instance.SetDifficulty(false);
        ShowStageGrid();
    }

    public void SelectHard()
    {
        EnsureGameManagerExists();
        GameManager.Instance.SetDifficulty(true);
        ShowStageGrid();
    }

    private void ShowStageGrid()
    {
        difficultyPanel.SetActive(false);
        stageGridPanel.SetActive(true);
        stageSelectManager.PopulateGrid();
    }

    private void EnsureGameManagerExists()
    {
        if (GameManager.Instance == null)
        {
            GameObject go = new GameObject("GameManager");
            go.AddComponent<GameManager>();
        }
    }

    public void BackToDifficulty()
    {
        stageGridPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }
}
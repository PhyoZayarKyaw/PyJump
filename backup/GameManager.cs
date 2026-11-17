using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }
    public int coinsThisStage { get; private set; }
    public int totalCoinsThisStage { get; private set; }
    public bool diedThisStage { get; private set; }
    public float timeElapsed { get; private set; }
    public int startingLives { get; private set; } = 2;
    public int totalStagesInWorld = 20;
    public string mainMenuSceneName = "1-0";
    public bool isHard { get; private set; } = true;

    public void SetDifficulty(bool isHard)
    {
        this.isHard = isHard;
        startingLives = isHard ? 2 : 99;
        lives = startingLives;
    }

    public string GetStarKey(int world, int stage)
    {
        string difficultyLabel = isHard ? "Hard" : "Easy";
        return $"Stars_{difficultyLabel}_{world}-{stage}";
    }
    

    public void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        if (lives <= 0)
        {
            InitializeGameState();
        }

        if (world <= 0 || stage <= 0)
        {
            SyncWorldStageFromActiveScene();
        }

        if (totalCoinsThisStage <= 0)
        {
            totalCoinsThisStage = FindObjectsOfType<PowerUp>().Count(p => p.type == PowerUp.Type.Coin)
                + FindObjectsOfType<BlockHit>().Count(b => b.item != null && b.item.GetComponent<BlockCoin>() != null);
        }
    }

    private void SyncWorldStageFromActiveScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string[] parts = sceneName.Split('-');

        if (parts.Length == 2 && int.TryParse(parts[0], out int w) && int.TryParse(parts[1], out int s))
        {
            world = w;
            stage = s;
        }
    }

    private void InitializeGameState()
    {
        lives = startingLives;
        coins = 0;
        timeElapsed = 0f;
        diedThisStage = false;
    }

    public void NewGame()
    {
        InitializeGameState();
        LoadLevel(1, 1);
    }

    public void LoadLevel(int world, int stage)
    {
        bool isNewStage = (world != this.world || stage != this.stage);

        if (isNewStage)
        {
            coins = 0;
            diedThisStage = false;
        }

        this.world = world;
        this.stage = stage;
        timeElapsed = 0f;
        coinsThisStage = 0;

        string sceneName = $"{world}-{stage}";
        SceneManager.LoadScene(sceneName);

        totalCoinsThisStage = FindObjectsOfType<PowerUp>().Count(p => p.type == PowerUp.Type.Coin)
    + FindObjectsOfType<BlockHit>().Count(b => b.item != null && b.item.GetComponent<BlockCoin>() != null);
    }

    public void NextLevel()
    {
        bool isFinalStage = stage >= totalStagesInWorld;

        System.Action onContinue = isFinalStage
            ? (System.Action)(() => GoToMainMenu())
            : (() => LoadLevel(world, stage + 1));

        CompleteLevelWithResults(onContinue, isFinalStage);
    }

    private void CompleteLevelWithResults(System.Action onContinue, bool isFinalStage = false)
    {
        bool star1 = timeElapsed < 300f;
        bool star2 = totalCoinsThisStage <= 0 || coinsThisStage >= totalCoinsThisStage;
        bool star3 = !diedThisStage;

        //string key = $"Stars_{world}-{stage}";
        string key = GetStarKey(world, stage);
        int mask = (star1 ? 1 : 0) | (star2 ? 2 : 0) | (star3 ? 4 : 0);
        PlayerPrefs.SetInt(key, mask);
        PlayerPrefs.Save();

        if (ResultsUIManager.Instance != null)
        {
            ResultsUIManager.Instance.ShowResults(star1, star2, star3, onContinue, isFinalStage);
        }
        else
        {
            onContinue?.Invoke();
        }
    }

    public void ResetLevel()
    {
        lives--;
        coins = 0;
        diedThisStage = true;
        if (lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    public void LoseLife()
    {
        lives--;
        diedThisStage = true;
        if (lives <= 0)
        {
            GameOver();
        }
    }


    private void GameOver()
    {
        lives = startingLives;

        if (GameOverUIManager.Instance != null)
        {
            GameOverUIManager.Instance.ShowGameOver();
        }
        else
        {
            LoadLevel(world, stage);
        }
    }

    public void AddCoin()
    {
        coins++;
        coinsThisStage++;
        if (coins >= 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++; // Increase the number of lives
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
    }

    private void TimeUp()
    {
        LoseLife(); // reuse your existing life-loss logic
    }

    public void RestartLevel()
    {
        coins -= coinsThisStage;
        LoadLevel(world, stage);
    }

    public void ResetLevelDelayed(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

  public void ClearAllProgress()
    {
        for (int i = 1; i <= 20; i++)
        {
            PlayerPrefs.DeleteKey($"Stars_Easy_1-{i}");
            PlayerPrefs.DeleteKey($"Stars_Hard_1-{i}");
        }
        PlayerPrefs.Save();
    }

}

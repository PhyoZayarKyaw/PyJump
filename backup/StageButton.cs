using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageButton : MonoBehaviour
{
    public int world = 1;
    public int stage;

    [SerializeField] private TextMeshProUGUI stageLabel;
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;
    [SerializeField] private Button button;
    public Sprite filledStar;
    public Sprite emptyStar;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        stageLabel.text = $"{world}-{stage}";

        int mask = PlayerPrefs.GetInt(GameManager.Instance.GetStarKey(world, stage), 0);

        star1.sprite = (mask & 1) != 0 ? filledStar : emptyStar;
        star2.sprite = (mask & 2) != 0 ? filledStar : emptyStar;
        star3.sprite = (mask & 4) != 0 ? filledStar : emptyStar;

        bool unlocked = IsUnlocked();

        if (button != null)
        {
            button.interactable = unlocked;
        }

        stageLabel.text = unlocked ? $"{world}-{stage}" : "LOCKED";
    }


    private bool IsUnlocked()
    {
        if (stage <= 1) return true;

        int previousMask = PlayerPrefs.GetInt(GameManager.Instance.GetStarKey(world, stage - 1), 0);
        return previousMask > 0;
    }

    public void PlayStage()
    {
        if (!IsUnlocked()) return;
        GameManager.Instance.LoadLevel(world, stage);
    }
}
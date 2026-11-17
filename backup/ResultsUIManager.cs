using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsUIManager : MonoBehaviour
{
    public static ResultsUIManager Instance { get; private set; }

    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private Image star1Image;
    [SerializeField] private Image star2Image;
    [SerializeField] private Image star3Image;
    [SerializeField] private TextMeshProUGUI continueButtonLabel;
    public Sprite filledStar;
    public Sprite emptyStar;

    private System.Action onContinue;

    private void Awake()
    {
        Instance = this;
        resultsPanel.SetActive(false);
    }

    public void ShowResults(bool star1, bool star2, bool star3, System.Action continueCallback, bool isFinalStage = false)
    {
        onContinue = continueCallback;

        star1Image.sprite = star1 ? filledStar : emptyStar;
        star2Image.sprite = star2 ? filledStar : emptyStar;
        star3Image.sprite = star3 ? filledStar : emptyStar;

        if (continueButtonLabel != null)
        {
            continueButtonLabel.text = isFinalStage ? "MAIN MENU" : "CONTINUE";
        }

        resultsPanel.SetActive(true);
    }

    public void OnContinuePressed()
    {
        resultsPanel.SetActive(false);
        onContinue?.Invoke();
        onContinue = null;
    }
}
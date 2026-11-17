using UnityEngine;
using TMPro;

public class QuestionUIManager : MonoBehaviour
{
    public static QuestionUIManager Instance { get; private set; }

    [SerializeField] private GameObject questionPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TMP_InputField answerInput;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private PlayerMovement playerMovement;

    private string currentCorrectAnswer;
    private bool currentCheckAnswer = true;
    private System.Action<string> currentOnSubmit;
    private System.Action currentOnClosed;
    private System.Action currentOnCorrect;

    private void Awake()
    {
        Instance = this;
        questionPanel.SetActive(false);
    }

    public void ShowQuestion(string question, string correctAnswer, bool checkAnswer = true, System.Action<string> onSubmit = null, System.Action onClosed = null, System.Action onCorrect = null)
    {
        currentCorrectAnswer = correctAnswer;
        currentCheckAnswer = checkAnswer;
        currentOnSubmit = onSubmit;
        currentOnClosed = onClosed;
        currentOnCorrect = onCorrect;
        questionText.text = question;
        answerInput.text = "";
        feedbackText.text = "";
        questionPanel.SetActive(true);

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            Rigidbody2D rb = playerMovement.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        answerInput.ActivateInputField();
    }

    public void HideQuestion()
    {
        StartCoroutine(HideQuestionNextFrame());
    }

    private System.Collections.IEnumerator HideQuestionNextFrame()
    {
        yield return null;
        questionPanel.SetActive(false);
        if (playerMovement != null) playerMovement.enabled = true;
        currentOnClosed?.Invoke();
    }

    public void SubmitAnswer()
    {
        string playerAnswer = answerInput.text.Trim();
        currentOnSubmit?.Invoke(playerAnswer);

        questionPanel.SetActive(false);
        if (playerMovement != null) playerMovement.enabled = true;

        if (!currentCheckAnswer)
        {
            feedbackText.text = "Got it!";
            currentOnClosed?.Invoke();
            return;
        }

        if (string.Equals(playerAnswer, currentCorrectAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            feedbackText.text = "Correct!";
            currentOnClosed?.Invoke();
            currentOnCorrect?.Invoke();
            GameManager.Instance.NextLevel();
        }
        else
        {
            feedbackText.text = "Not quite, try again!";
            currentOnClosed?.Invoke();
            GameManager.Instance.LoseLife();
        }
    }
}
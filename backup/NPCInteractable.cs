using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [Header("Question Setup")]
    public string question = "What keyword declares a whole number in C#?";
    public string correctAnswer = "int";

    [Tooltip("If checked, the answer is graded against Correct Answer (right = advance level, wrong = lose life). If unchecked, whatever the player types is just stored — no grading, no life loss, no level advance.")]
    public bool checkAnswer = true;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Optional Wall Barrier")]
    [Tooltip("If set, this wall disappears when the player answers correctly.")]
    public GameObject barrier;

    [HideInInspector] public string lastAnswer = "";

    private bool playerInRange = false;
    private bool questionOpen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && !questionOpen && Input.GetKeyDown(interactKey))
        {
            questionOpen = true;
            QuestionUIManager.Instance.ShowQuestion(question, correctAnswer, checkAnswer, OnAnswerSubmitted, OnQuestionClosed, OnCorrectAnswer);
        }
    }

    private void OnAnswerSubmitted(string answer)
    {
        lastAnswer = answer;
    }

    private void OnQuestionClosed()
    {
        questionOpen = false;
    }

    private void OnCorrectAnswer()
    {
        if (barrier != null)
        {
            barrier.SetActive(false);
        }
    }
}
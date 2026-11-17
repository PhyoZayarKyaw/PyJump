using UnityEngine;
using TMPro;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance { get; private set; }

    [TextArea(3, 10)]
    [Tooltip("Set this per-level in the Inspector — the explanation shown when the player presses Hint")]
    public string hintText;

    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TextMeshProUGUI hintTextDisplay;

    private void Awake()
    {
        Instance = this;
        hintPanel.SetActive(false);
    }

    public void ShowHint()
    {
        hintTextDisplay.text = hintText;
        hintPanel.SetActive(true);
    }

    public void HideHint()
    {
        hintPanel.SetActive(false);
    }
    
}
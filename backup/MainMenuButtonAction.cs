using UnityEngine;

public class MainMenuButtonAction : MonoBehaviour
{
    public void OnMainMenuPressed()
    {
        GameManager.Instance.GoToMainMenu();
    }
}
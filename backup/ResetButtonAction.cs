using UnityEngine;

public class ResetButtonAction : MonoBehaviour
{
    public void OnResetPressed()
    {
        GameManager.Instance.RestartLevel();
    }
}
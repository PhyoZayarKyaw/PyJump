using UnityEngine;

public class DeathBarrier : MonoBehaviour 
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false); // Deactivate the player object
            GameManager.Instance.ResetLevelDelayed(3f); // Reset the level after 3 seconds

        }
        else
        {
            Destroy(other.gameObject); // Destroy the enemy object
        }
    }
}

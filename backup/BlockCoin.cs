using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.AddCoin(); // Increment the coin count in the GameManager when the block is created
        StartCoroutine(Animate()); // Start the animation coroutine
    }

    private IEnumerator Animate()
    {
        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 2f; // Define the animated position
        
        yield return Move(restingPosition, animatedPosition); // Move to the animated position
        yield return Move(animatedPosition, restingPosition); // Move back to the resting position    
        Destroy(gameObject); // Destroy the coin object after the animation is complete
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.25f; // Duration of the movement

        while (elapsed < duration)
        {
            float t = elapsed / duration; // Calculate the interpolation factor
            transform.localPosition = Vector3.Lerp(from, to, t); // Interpolate the position
            elapsed += Time.deltaTime; // Increment the elapsed time
            yield return null; // Wait for the next frame
        }

        transform.localPosition = to; // Ensure the final position is set
    }
}

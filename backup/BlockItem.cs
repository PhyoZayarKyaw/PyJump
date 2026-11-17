using System.Collections;
using UnityEngine;

public class Blocked : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animate()); // Start the animation coroutine
    }

    private IEnumerator Animate()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();   
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody.isKinematic = true; // Make the rigidbody kinematic to prevent physics interactions
        physicsCollider.enabled = false; // Disable the physics collider to prevent collisions
        triggerCollider.enabled = false; // Disable the trigger collider to prevent interactions
        spriteRenderer.enabled = false; // Hide the sprite renderer to make the block invisible

        yield return new WaitForSeconds(0.25f); // Wait for 0.5 seconds before starting the animation 
        spriteRenderer.enabled = true; // Show the sprite renderer to make the block visible

        float elapsed = 0f;
        float duration = 0.5f; // Duration of the movement

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition + Vector3.up; // Move the block up by 0.5 units

        while (elapsed < duration)
        {
            float t = elapsed / duration; // Calculate the interpolation factor
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t); // Interpolate the position
            elapsed += Time.deltaTime; // Increment the elapsed time
            yield return null; // Wait for the next frame
        }

        transform.localPosition = endPosition; // Ensure the final position is set
        rigidbody.isKinematic = false; // Make the rigidbody non-kinematic to allow physics interactions
        physicsCollider.enabled = true; // Enable the physics collider to allow collisions
        triggerCollider.enabled = true; // Enable the trigger collider to allow interactions 
    }
}

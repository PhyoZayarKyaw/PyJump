using System.Collections;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{   
    public SpriteRenderer spriteRenderer;
    public Sprite deadSprite;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }

    private void UpdateSprite()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10; // Ensure the sprite is rendered above other elements
        if (deadSprite != null)
        {
            spriteRenderer.sprite = deadSprite;
        }
    }

    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false; // Disable all colliders to prevent further interactions
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // ← this line, not rb.isKinematic
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        EntityMovement entityMovement = GetComponent<EntityMovement>();

        if (playerMovement != null)
        {
            playerMovement.enabled = false; // Disable player movement
        }

        if (entityMovement != null)
        {
            entityMovement.enabled = false; // Disable entity movement
        }
    }

    private IEnumerator Animate()
    {
        float elapsed = 0f;
        float duration = 3f; // Duration of the death animation
        float jumpVelocity = 10f;
        float gravity = -36f;

        Vector3 velocity = Vector3.up * jumpVelocity;
        while (elapsed < duration)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime; // Apply gravity to the vertical velocity
            elapsed += Time.deltaTime;
            yield return null;
        }

         // Gravity value
    }
}

using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;
    public Sprite emptyBlock;
    public int maxHits = -1; // Maximum number of hits the block can take
    private bool animating;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Hit();
            }
        }
    }

    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); 
        spriteRenderer.enabled = true; 
        maxHits--; // Decrease the number of hits remaining
        
        if (maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock; // Change the sprite to the empty block sprite
        }

        if (item != null)
        {
            Instantiate(item, transform.position, Quaternion.identity); // Spawn the item above the block
        }
 
        StartCoroutine(Animate()); // Start the animation coroutine
    }

    private IEnumerator Animate()
    {
        animating = true; // Set animating to true to indicate that the animation is in progress
        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f; // Define the animated position
        
        yield return Move(restingPosition, animatedPosition); // Move to the animated position
        yield return Move(animatedPosition, restingPosition); // Move back to the resting position
        
        animating = false;


    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f; // Duration of the movement

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

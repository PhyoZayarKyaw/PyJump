using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{   
    public Sprite[] sprites;
    public float framerate = 1f / 6f; 
    
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private int frame;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Animate), framerate, framerate); // Start the animation
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        frame++; // Increment the frame index
        if(frame >= sprites.Length) // If the frame index exceeds the number of sprites
        {
            frame = 0; // Reset to the first frame
        }

        if(frame >= 0 && frame < sprites.Length) // Ensure the frame index is valid
        {
            spriteRenderer.sprite = sprites[frame]; // Update the sprite to the current frame
        }
    }
}

using UnityEngine;

public class RandomJump : MonoBehaviour
{
    public float minInterval = 1f;
    public float maxInterval = 3f;
    public float jumpForce = 8f;

    private EntityMovement movement;
    private float timer;

    private void Awake()
    {
        movement = GetComponent<EntityMovement>();
        SetNextJumpTime();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (movement != null && movement.enabled)
            {
                movement.Jump(jumpForce);
            }
            SetNextJumpTime();
        }
    }

    private void SetNextJumpTime()
    {
        timer = Random.Range(minInterval, maxInterval);
    }
}
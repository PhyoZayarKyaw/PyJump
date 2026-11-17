using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Carrying")]
    public Transform carryPoint;
    public LayerMask carryableLayer;
    public float grabRadius = 0.75f;
    public KeyCode grabKey = KeyCode.LeftControl;
    

    private Carryable carriedObject;
    public bool carrying => carriedObject != null;

    private Camera mainCamera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private float inputAxis;
    private Vector2 velocity;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

    [Header("Slide Stage")]
    [Tooltip("Enable for slide-down stages: player can't move up, falls at a constant speed, but can still jump.")]
    public bool slideMode = false;
    public float slideSpeed = 3f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void Update()
    {
        HorizontalMovement();
        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded && !slideMode)
        {
            GroundedMovement();
        }

        if (slideMode)
    {
        if (grounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
        else if (grounded)
        {
            jumping = false;
        }
    }

        ApplyGravity();
        HandleCarrying();
    }

    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxisRaw("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0f;
        }

        if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;

        float terminalVelocity = slideMode ? -slideSpeed : gravity / 2f;
        velocity.y = Mathf.Max(velocity.y, terminalVelocity);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = mainCamera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }

    private void HandleCarrying()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (carrying)
            {
                DropCarriedObject();
            }
            else
            {
                TryPickUp();
            }
        }
    }

    private void TryPickUp()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, grabRadius, carryableLayer);
        if (hit != null)
        {
            Carryable carryable = hit.GetComponentInParent<Carryable>();
            if (carryable != null)
            {
                carriedObject = carryable;
                carriedObject.PickUp(carryPoint);
            }
        }
    }

    public void DropCarriedObject()
    {
        carriedObject.Drop();
        carriedObject = null;
    }

    public void Bounce(float force)
    {
        Vector2 direction = velocity.normalized;

        if (direction == Vector2.zero)
        {
            direction = Vector2.down;
        }

        velocity = -direction * force;
        jumping = true;
    }

    public void StompBounce()
    {
        velocity.y = jumpForce / 2f;
        jumping = true;
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Carryable : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;

    public bool beingCarried { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void PickUp(Transform carryPoint)
    {
        beingCarried = true;

        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
        col.enabled = false; // stop colliding with player/enemies while held

        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;
    }

    public void Drop()
    {
        beingCarried = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        col.enabled = true;
    }

    public void Bounce(Vector2 force)
{
    if (rb != null)
    {
        rb.linearVelocity = force;
    }
}
}
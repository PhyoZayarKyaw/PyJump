using UnityEngine;

public static class Extensions
{
    public static LayerMask layerMask = LayerMask.GetMask("Default");

    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
{
    float radius = 0.25f;
    float distance = 0.375f;

    Collider2D collider = rigidbody.GetComponent<Collider2D>();
    Vector2 origin = collider != null ? rigidbody.position + collider.offset : rigidbody.position;

    // Only adjust origin for downward raycasts
    if (collider != null && direction == Vector2.down)
    {
        origin.y -= collider.bounds.extents.y;
    }

    RaycastHit2D hit = Physics2D.CircleCast(origin, radius, direction.normalized, distance, layerMask);
    return hit.collider != null && hit.rigidbody != rigidbody;
}

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }
}
using UnityEngine;

public class Slide : MonoBehaviour
{
    private Transform player;
    public float height = 4.45f;
    public float undergroundHeight = -10f;

    public bool followVertical = false;
    public Vector2 verticalOffset = Vector2.zero;

    private bool underground = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        position.x = player.position.x;

        if (followVertical)
        {
            position.y = player.position.y + verticalOffset.y;
        }
        else
        {
            position.y = underground ? undergroundHeight : height;
        }

        transform.position = position;
    }

    public void SetUnderground(bool isUnderground)
    {
        underground = isUnderground;

        if (!followVertical)
        {
            Vector3 cameraPosition = transform.position;
            cameraPosition.y = isUnderground ? undergroundHeight : height;
            transform.position = cameraPosition;
        }
    }
}
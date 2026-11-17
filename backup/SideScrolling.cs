using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;
    public float height = 4.45f;
    public float undergroundHeight = -10f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        position.x = player.position.x;
        transform.position = position;
    }

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        transform.position = cameraPosition;
    }
}
 
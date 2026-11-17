using UnityEngine;

public class ListItem : MonoBehaviour
{
    [Tooltip("Must match exactly what's used in ListMatchManager's Target List, e.g. \"apple\", \"banana\", \"cherry\".")]
    public string itemName = "apple";

    [Header("Bounce on Wrong Pickup")]
    [Tooltip("If true, wrong pickups bounce the player back instead of destroying this item.")]
    public bool bounceOnWrong = false;
    public float bounceForce = 5f;

    [Header("Label (auto-created if left empty)")]
    public TextMesh label;

    private void Awake()
    {
        SetupLabel();
    }

    private void SetupLabel()
    {
        if (label == null)
        {
            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(transform);
            labelObject.transform.localPosition = new Vector3(0f, 0.6f, -0.1f);

            Vector3 parentScale = transform.localScale;
            labelObject.transform.localScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );

            label = labelObject.AddComponent<TextMesh>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.GetComponent<MeshRenderer>().sortingOrder = 10;
        }

        label.text = itemName;
        label.characterSize = 0.1f;
        label.fontSize = 48;
        label.anchor = TextAnchor.MiddleCenter;
        label.alignment = TextAlignment.Center;
        label.color = Color.white;
    }

    private void LateUpdate()
    {
        if (label != null)
        {
            label.transform.rotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool correct = ListMatchManager.Instance.TryCollect(itemName);

            if (correct)
            {
                Destroy(gameObject);
            }
            else if (bounceOnWrong)
            {
                PlayerMovement movement = other.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    bool landedFromAbove = transform.DotTest(other.transform, Vector2.up);

                    if (landedFromAbove)
                    {
                        movement.StompBounce();
                    }
                    else
                    {
                        movement.Bounce(bounceForce);
                    }
                }
            }
        }
    }
}
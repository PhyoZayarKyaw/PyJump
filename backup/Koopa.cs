using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 12f;

    [Header("Boss Mode (optional)")]
    public bool isBoss = false;
    public int hitsRequired = 5;

    [Header("Requires Starpower")]
    [Tooltip("If true, touching this Koopa without starpower active kills the player instantly.")]
    public bool requiresStarpower = false;

    [Tooltip("Auto-created if left empty — floats above the Koopa and follows it.")]
    public TextMesh counterText;

    public GameObject barrier;

    [Header("Ordered Sequence Mode (optional)")]
    [Tooltip("If true, this Koopa is part of an ordered-stomp puzzle instead of normal enemy behavior.")]
    public bool isSequenceItem = false;
    public Vector2Int coordinate = Vector2Int.zero;

    [Tooltip("If true, Hits Required is calculated dynamically from PowerManager (if/elif/else stage). If false, Hits Required uses whatever fixed number you set in the Inspector.")]
    public bool useDynamicHitsRequired = false;

    [Tooltip("Auto-created if left empty — shows the coordinate above the Koopa.")]
    public TextMesh coordinateLabel;

    private int currentHits = 0;
    private bool shelled;
    private bool pushed;

    private void Awake()
    {
        if (isSequenceItem)
        {
            SetupCoordinateLabel();
        }

        if (isBoss)
        {
            SetupCounterLabel(); // no longer calculate here
        }
    }

    private void CalculateHitsRequired()
    {
        int power = PowerManager.Instance != null ? PowerManager.Instance.power : 0;

        if (power < 50)
        {
            hitsRequired = 10;
        }
        else if (power < 100)
        {
            hitsRequired = 3;
        }
        else
        {
            hitsRequired = 1;
        }
    }

    private void Start()
    {
        UpdateCounterText();
    }

    private void SetupCounterLabel()
    {
        if (counterText == null)
        {
            GameObject labelObject = new GameObject("CounterLabel");
            labelObject.transform.SetParent(transform);
            labelObject.transform.localPosition = new Vector3(0f, 1.2f, -0.1f);

            Vector3 parentScale = transform.localScale;
            labelObject.transform.localScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );

            counterText = labelObject.AddComponent<TextMesh>();
            counterText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            counterText.GetComponent<MeshRenderer>().sortingOrder = 10;
        }

        counterText.characterSize = 0.08f;
        counterText.fontSize = 40;
        counterText.anchor = TextAnchor.MiddleCenter;
        counterText.alignment = TextAlignment.Center;
        counterText.color = Color.white;
    }

    private void SetupCoordinateLabel()
    {
        if (coordinateLabel == null)
        {
            GameObject labelObject = new GameObject("CoordinateLabel");
            labelObject.transform.SetParent(transform);
            labelObject.transform.localPosition = new Vector3(0f, 1.2f, -0.1f);

            Vector3 parentScale = transform.localScale;
            labelObject.transform.localScale = new Vector3(
                1f / parentScale.x,
                1f / parentScale.y,
                1f / parentScale.z
            );

            coordinateLabel = labelObject.AddComponent<TextMesh>();
            coordinateLabel.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            coordinateLabel.GetComponent<MeshRenderer>().sortingOrder = 10;
        }

        coordinateLabel.text = $"({coordinate.x},{coordinate.y})";
        coordinateLabel.characterSize = 0.08f;
        coordinateLabel.fontSize = 40;
        coordinateLabel.anchor = TextAnchor.MiddleCenter;
        coordinateLabel.alignment = TextAlignment.Center;
        coordinateLabel.color = Color.white;
    }

    private void LateUpdate()
    {
        if (counterText != null)
        {
            counterText.transform.rotation = Quaternion.identity;
        }

        if (coordinateLabel != null)
        {
            coordinateLabel.transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (requiresStarpower && !player.starpower)
        {
            player.ForceDeath();
            return;
        }

        if (player.starpower)
        {
            Hit();
            return;
        }

        bool stompedFromAbove = collision.transform.DotTest(transform, Vector2.down);

        if (isSequenceItem)
        {
            if (stompedFromAbove)
            {
                RegisterSequenceStomp();
            }
            else
            {
                player.Hit();
            }
            return;
        }

        if (isBoss)
        {
            if (stompedFromAbove)
            {
                RegisterBossHit();
            }
            else
            {
                player.Hit();
            }
            return;
        }

        if (!shelled)
        {
            if (stompedFromAbove)
            {
                EnterShell();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void RegisterSequenceStomp()
    {
        if (OrderedStompManager.Instance.IsNext(coordinate))
        {
            OrderedStompManager.Instance.RegisterCorrectStomp();
            Hit();
        }
        else
        {
            OrderedStompManager.Instance.RegisterWrongStomp();
        }
    }

    private void RegisterBossHit()
    {
        if (currentHits == 0 && useDynamicHitsRequired)
        {
            CalculateHitsRequired();
            UpdateCounterText();
        }

        currentHits++;
        UpdateCounterText();

        if (currentHits >= hitsRequired)
        {
            if (barrier != null)
            {
                barrier.SetActive(false);
            }
            Hit();
        }
    }

    private void UpdateCounterText()
    {
        if (counterText != null)
        {
            counterText.text = $"{currentHits}/{hitsRequired}";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isBoss || isSequenceItem) return;

        if (shelled && other.CompareTag("Player"))
        {
            if (!pushed)
            {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            }
            else
            {
                Player player = other.GetComponent<Player>();
                if (player.starpower)
                {
                    Hit();
                }
                else
                {
                    player.Hit();
                }
            }
        }
        else if (!shelled && other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    private void EnterShell()
    {
        if (!isBoss && !isSequenceItem && PowerManager.Instance != null)
        {
            PowerManager.Instance.AddPower(10);
        }

        shelled = true;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
    }

    private void PushShell(Vector2 direction)
    {
        pushed = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }

    private void OnBecameInvisible()
    {
        if (pushed)
        {
            Destroy(gameObject);
        }
    }
}
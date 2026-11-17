using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

    [Header("Ordered Sequence Mode (optional)")]
    public bool isSequenceItem = false;
    public Vector2Int coordinate = Vector2Int.zero;
    public TextMesh coordinateLabel;

    [Header("While-Condition Glow Mode (optional)")]
    [Tooltip("If true, this enemy toggles between glowing (safe to attack) and not glowing (attacking kills the player) on its own timer.")]
    public bool glowMode = false;
    public float glowDuration = 2f;
    public float darkDuration = 2f;
    public Color glowColor = Color.yellow;
    public Color normalColor = Color.white;

    private bool isGlowing = false;
    private float glowTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isSequenceItem)
        {
            SetupCoordinateLabel();
        }

        if (glowMode)
        {
            isGlowing = true; // start glowing (safe) so the player isn't punished immediately on sight
            glowTimer = glowDuration;
            UpdateGlowVisual();
        }
    }

    private void Update()
    {
        if (!glowMode) return;

        glowTimer -= Time.deltaTime;

        if (glowTimer <= 0f)
        {
            isGlowing = !isGlowing;
            glowTimer = isGlowing ? glowDuration : darkDuration;
            UpdateGlowVisual();
        }
    }

    private void UpdateGlowVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isGlowing ? glowColor : normalColor;
        }
    }

    private void SetupCoordinateLabel()
    {
        if (coordinateLabel == null)
        {
            GameObject labelObject = new GameObject("CoordinateLabel");
            labelObject.transform.SetParent(transform);
            labelObject.transform.localPosition = new Vector3(0f, 1f, -0.1f);

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
        if (coordinateLabel != null)
        {
            coordinateLabel.transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (glowMode)
        {
            if (!isGlowing)
            {
                player.ForceDeath();
                return;
            }

            bool stompedFromAbove = collision.transform.DotTest(transform, Vector2.down);
            if (stompedFromAbove)
            {
                Flatten();
            }
            else
            {
                player.Hit();
            }
            return;
        }

        if (player.starpower)
        {
            Hit();
            return;
        }

        bool stompedFromAboveNormal = collision.transform.DotTest(transform, Vector2.down);

        if (isSequenceItem)
        {
            if (stompedFromAboveNormal)
            {
                RegisterSequenceStomp();
            }
            else
            {
                player.Hit();
            }
            return;
        }

        if (stompedFromAboveNormal)
        {
            Flatten();
        }
        else
        {
            player.Hit();
        }
    }

    private void RegisterSequenceStomp()
    {
        if (OrderedStompManager.Instance.IsNext(coordinate))
        {
            OrderedStompManager.Instance.RegisterCorrectStomp();
            Flatten();
        }
        else
        {
            OrderedStompManager.Instance.RegisterWrongStomp();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell"))
        {
            Hit();
        }
    }

    private void Flatten()
    {
        if (!isSequenceItem && !glowMode && PowerManager.Instance != null)
        {
            PowerManager.Instance.AddPower(10);
        }

        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        Destroy(gameObject, 0.5f);
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }
}
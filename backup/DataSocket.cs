using UnityEngine;
using System.Linq;

public class DataSocket : MonoBehaviour
{
    public DataType requiredType;
    public Transform snapPoint; // where each collected box briefly shows up, defaults to this transform if not set

    [Header("Optional Label Override")]
    [Tooltip("Leave empty to use the default type name (int/float/string/bool). Set this to override with custom text, e.g. 'Box X'.")]
    public string customLabel = "";

    [Tooltip("If checked, no label text is shown at all (overrides Custom Label too).")]
    public bool hideLabel = false;

    [Tooltip("How many boxes are needed to complete this socket. Leave at 0 to auto-count every matching box currently in the scene.")]
    public int requiredCount = 0;

    [Tooltip("Leave empty to accept any value of the required type. Set this to require an exact value match too, e.g. '5' or 'true'.")]
    public string requiredValue = "";

    [Header("Print Output (optional)")]
    [Tooltip("If set, this socket will show the linked NPC's stored answer instead of 'Complete!' once filled.")]
    public NPCInteractable sourceNPC;

    [Tooltip("If set, this socket reports completion to this specific group instead of the scene-wide LevelObjectivesManager.")]
    public SocketGroup socketGroup;

    private int collectedCount = 0;

    [Header("Feedback")]
    public GameObject correctEffect;
    public GameObject wrongEffect;

    [Header("Visual Indicator (auto-created if left empty)")]
    public TextMesh label;
    public SpriteRenderer indicator;
    public Color glowColor = new Color(1f, 1f, 1f, 0.6f);
    public float pulseSpeed = 2f;
    public float minAlpha = 0.25f;
    public float maxAlpha = 0.9f;

    public bool filled { get; private set; }

    private void Awake()
    {
        if (requiredCount <= 0)
        {
            requiredCount = FindObjectsOfType<DataBox>().Count(b => b.dataType == requiredType);
        }

        SetupLabel();
        SetupIndicator();
    }

    private void Update()
    {
        if (filled || indicator == null) return;

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        Color c = glowColor;
        c.a = alpha;
        indicator.color = c;
    }

    private void RefreshLabelText()
    {
        if (label != null)
        {
            if (hideLabel)
            {
                label.text = "";
            }
            else if (!string.IsNullOrEmpty(customLabel))
            {
                label.text = customLabel;
            }
            else
            {
                label.text = $"Place {TypeLabel()} here ({collectedCount}/{requiredCount})";
            }
        }
    }

    private string TypeLabel()
    {
        return requiredType switch
        {
            DataType.Int => "int",
            DataType.Bool => "bool",
            DataType.Float => "float",
            DataType.String => "string",
            _ => ""
        };
    }

    private void SetupLabel()
    {
        if (label == null)
        {
            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(transform);
            labelObject.transform.localPosition = new Vector3(0f, 1f, -0.1f); // above socket
            labelObject.transform.localScale = Vector3.one;

            label = labelObject.AddComponent<TextMesh>();
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.GetComponent<MeshRenderer>().sortingOrder = 10;
        }

        RefreshLabelText();
        label.characterSize = 0.08f;
        label.fontSize = 40;
        label.anchor = TextAnchor.MiddleCenter;
        label.alignment = TextAlignment.Center;
        label.color = Color.white;
    }

    private void SetupIndicator()
    {
        if (indicator == null)
        {
            GameObject indicatorObject = new GameObject("Indicator");
            indicatorObject.transform.SetParent(transform);
            indicatorObject.transform.localPosition = new Vector3(0f, 0f, 0.1f); // slightly behind box
            indicatorObject.transform.localScale = Vector3.one;

            indicator = indicatorObject.AddComponent<SpriteRenderer>();
            indicator.sprite = GenerateBorderSprite();
            indicator.sortingOrder = -1;
        }

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            indicator.transform.localScale = new Vector3(box.size.x, box.size.y, 1f);
            indicator.transform.localPosition = new Vector3(box.offset.x, box.offset.y, 0.1f);
        }

        indicator.color = glowColor;
    }

    private Sprite GenerateBorderSprite()
    {
        int size = 32;
        int thickness = 3;
        Texture2D tex = new Texture2D(size, size);
        tex.filterMode = FilterMode.Point;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                bool border = x < thickness || x >= size - thickness || y < thickness || y >= size - thickness;
                tex.SetPixel(x, y, border ? Color.white : new Color(1f, 1f, 1f, 0f));
            }
        }
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (filled) return;

        DataBox box = other.GetComponentInParent<DataBox>();
        if (box == null || box.placed) return;

        bool typeMatches = box.dataType == requiredType;
        bool valueMatches = string.IsNullOrEmpty(requiredValue) || box.value == requiredValue;

        if (typeMatches && valueMatches)
        {
            PlaceCorrectly(box);
        }
        else
        {
            RejectBox(box);
        }
    }

    private void PlaceCorrectly(DataBox box)
    {
        collectedCount++;

        Transform target = snapPoint != null ? snapPoint : transform;

        if (correctEffect != null)
        {
            Instantiate(correctEffect, target.position, Quaternion.identity);
        }

        Destroy(box.gameObject);

        RefreshLabelText();

        if (collectedCount >= requiredCount)
        {
            filled = true;

            if (indicator != null) indicator.enabled = false;

            if (label != null)
            {
                if (sourceNPC != null && !string.IsNullOrEmpty(sourceNPC.lastAnswer))
                {
                    label.text = sourceNPC.lastAnswer;
                }
                else
                {
                    label.text = "Complete!";
                }
            }

            if (socketGroup != null)
                {
                    socketGroup.NotifySocketFilled(this);
                }
            else
                {
                    LevelObjectivesManager.Instance?.NotifySocketFilled(this);
                }
        }
    }

    [Header("Reject Bounce")]
    public float bounceForce = 4f;
    public float bounceUpForce = 3f;

    private void RejectBox(DataBox box)
    {
        if (wrongEffect != null)
        {
            Instantiate(wrongEffect, box.transform.position, Quaternion.identity);
        }

        Vector2 direction = (Vector2)box.transform.position - (Vector2)transform.position;
        if (direction.sqrMagnitude < 0.01f) direction = Vector2.up;
        direction.Normalize();

        Vector2 bounce = direction * bounceForce + Vector2.up * bounceUpForce;
        box.Bounce(bounce);

        bool loseLife = LevelObjectivesManager.Instance != null && LevelObjectivesManager.Instance.loseLifeOnMistake;
        if (loseLife)
        {
            GameManager.Instance.LoseLife();
        }
    }
}
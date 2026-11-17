using UnityEngine;

public class DataBox : Carryable
{
    [Tooltip("The actual data type this value belongs to - used for socket matching. Set this correctly for your literal value below.")]
    public DataType dataType;

    [Tooltip("The literal value shown to the player, e.g. \"1\", \"1.2\", \"Hello\". The player must figure out its type themselves.")]
    public string value = "1";

    [Header("Label (auto-created if left empty)")]
    public TextMesh label;

    public bool placed { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        SetupLabel();
    }

    private void SetupLabel()
    {
        if (label == null)
        {
            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(transform);
            labelObject.transform.localPosition = new Vector3(0f, 0.3f, -0.1f); // above the box, slightly in front for sorting

            // Counter-scale so the label stays a consistent visual size regardless of how big the parent sprite is
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

        // Strings get quotes for a visual hint, matching how strings are usually written in code -
        // remove this if you'd rather not hint the answer at all.
        label.text = dataType == DataType.String ? $"\"{value}\"" : value;

        label.characterSize = 0.1f;
        label.fontSize = 48;
        label.anchor = TextAnchor.MiddleCenter;
        label.alignment = TextAlignment.Center;
        label.color = Color.white;
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(value)) return;

        switch (dataType)
        {
            case DataType.Int:
                if (!int.TryParse(value, out _))
                    Debug.LogWarning($"[{name}] dataType is Int but value \"{value}\" is not a valid int.", this);
                break;
            case DataType.Float:
                if (!float.TryParse(value, out _))
                    Debug.LogWarning($"[{name}] dataType is Float but value \"{value}\" is not a valid float.", this);
                break;
            case DataType.Bool:
                if (!bool.TryParse(value, out _))
                    Debug.LogWarning($"[{name}] dataType is Bool but value \"{value}\" is not \"true\" or \"false\".", this);
                break;
        }
    }

    public void Lock()
    {
        placed = true;

        var col = GetComponent<Collider2D>();
        var rb = GetComponent<Rigidbody2D>();

        if (col != null) col.enabled = true;
        if (rb != null) rb.isKinematic = true; // freeze in place once correctly slotted
    }

    private void LateUpdate()
    {
        if (label != null)
        {
            label.transform.rotation = Quaternion.identity;
        }
    }
}
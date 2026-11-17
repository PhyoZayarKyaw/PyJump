using UnityEngine;

public class StaticLabelBox : MonoBehaviour
{
    [Tooltip("The text shown on this box.")]
    public string text = "apple";

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
            labelObject.transform.localPosition = new Vector3(0f, 0f, -0.1f);

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

        label.text = text;
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (label != null)
        {
            label.text = text;
        }
    }
#endif
}
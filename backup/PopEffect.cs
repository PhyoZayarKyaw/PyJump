using UnityEngine;
using System.Collections;

public class PopEffect : MonoBehaviour
{
    [Header("Center Pop")]
    public Color color = new Color(1f, 0.85f, 0.2f); // warm gold, change to taste
    public float duration = 0.4f;
    public float endScale = 1.5f;
    public float overshoot = 1.3f; // how far past endScale it punches before settling

    [Header("Sparkles")]
    public int sparkleCount = 8;
    public float sparkleSpeed = 3f;
    public float sparkleLifetime = 0.5f;
    public float sparkleSize = 0.08f;

    private void Start()
    {
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = GenerateDotSprite();
        sr.color = color;
        sr.sortingOrder = 20;

        SpawnSparkles();
        StartCoroutine(AnimateCenter(sr));
    }

    private IEnumerator AnimateCenter(SpriteRenderer sr)
    {
        float elapsed = 0f;
        transform.localScale = Vector3.zero;

        // Punch out past endScale, then settle back down while fading
        float punchDuration = duration * 0.3f;
        while (elapsed < punchDuration)
        {
            float t = elapsed / punchDuration;
            transform.localScale = Vector3.one * Mathf.Lerp(0f, endScale * overshoot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        float settleDuration = duration - punchDuration;
        while (elapsed < settleDuration)
        {
            float t = elapsed / settleDuration;
            transform.localScale = Vector3.one * Mathf.Lerp(endScale * overshoot, endScale, t);

            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void SpawnSparkles()
    {
        for (int i = 0; i < sparkleCount; i++)
        {
            float angle = (360f / sparkleCount) * i + Random.Range(-15f, 15f);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject sparkle = new GameObject("Sparkle");
            sparkle.transform.SetParent(transform.parent, true);
            sparkle.transform.position = transform.position;

            SpriteRenderer sr = sparkle.AddComponent<SpriteRenderer>();
            sr.sprite = GenerateDotSprite();
            sr.color = color;
            sr.sortingOrder = 19;

            StartCoroutine(AnimateSparkle(sparkle, sr, direction));
        }
    }

    private IEnumerator AnimateSparkle(GameObject sparkle, SpriteRenderer sr, Vector2 direction)
    {
        float elapsed = 0f;
        Vector3 startPos = sparkle.transform.position;
        float speed = sparkleSpeed * Random.Range(0.7f, 1.2f);

        sparkle.transform.localScale = Vector3.one * sparkleSize;

        while (elapsed < sparkleLifetime)
        {
            float t = elapsed / sparkleLifetime;

            // Fly outward, decelerating
            float ease = 1f - Mathf.Pow(1f - t, 2f);
            sparkle.transform.position = startPos + (Vector3)(direction * speed * ease * sparkleLifetime);

            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(sparkle);
    }

    private Sprite GenerateDotSprite()
    {
        int size = 16;
        Texture2D tex = new Texture2D(size, size);
        tex.filterMode = FilterMode.Point;

        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                tex.SetPixel(x, y, dist <= radius ? Color.white : new Color(1f, 1f, 1f, 0f));
            }
        }
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }
}
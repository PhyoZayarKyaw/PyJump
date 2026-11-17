using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRender smallRenderer;
    public PlayerSpriteRender bigRenderer;
    public Transform carryPoint; 

    private PlayerSpriteRender activeRenderer;

    private DeathAnimation deathAnimation;
    private CapsuleCollider2D capsuleCollider;

    private Coroutine starpowerCoroutine;
    private float starpowerEndTime;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower {get; private set; }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        activeRenderer = smallRenderer;
    }

    public void Hit()
    {
        if (!dead && !starpower)
        {
            if (big)
            {
                Shrink();
            }
            else
            {
                Death();
            }
        }
    }

    public void ForceDeath()
    {
        if (!dead)
        {
            Death();
        }
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        if (carryPoint != null)
        {
            carryPoint.localPosition = new Vector3(0f, 1.7f, 0f); // top of big Mario + a little padding
        }

        StartCoroutine(ScaleAnimation());
    }

    private void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        if (carryPoint != null)
        {
            carryPoint.localPosition = new Vector3(0f, 0.7f, 0f); // top of small Mario + a little padding
        }


        StartCoroutine(ScaleAnimation());
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;

        deathAnimation.enabled = true;

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null && movement.carrying)
        {
            movement.DropCarriedObject();
        }

        GameManager.Instance.ResetLevelDelayed(3f);
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void Starpower(float duration = 10f)
    {
        starpowerEndTime = Mathf.Max(starpowerEndTime, Time.time) + duration;

        if (starpowerCoroutine == null)
        {
            starpowerCoroutine = StartCoroutine(StarpowerAnimation());
        }
    } 

    private IEnumerator StarpowerAnimation()
    {
        starpower = true;

        while (Time.time < starpowerEndTime)
        {
            if (Time.frameCount % 4 == 0)
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
            yield return null;
            activeRenderer.spriteRenderer.color = Color.white;
        }

        starpower = false;
        starpowerCoroutine = null;
    }
}
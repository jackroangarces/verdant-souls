using UnityEngine;

public class HoleKillPlayer : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float requiredOverlapPercent = 0.5f; // 50%

    private Collider2D holeCollider;

    void Awake()
    {
        holeCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Collider2D playerCollider = other;

        float overlapArea = GetOverlapArea(holeCollider, playerCollider);
        float playerArea = GetColliderArea(playerCollider);

        if (playerArea <= 0f) return;

        float percentOverHole = overlapArea / playerArea;

        if (percentOverHole >= requiredOverlapPercent)
        {
            DeathPlayer death = other.GetComponentInParent<DeathPlayer>();
            if (death != null)
                death.Die();
        }
    }

    float GetOverlapArea(Collider2D a, Collider2D b)
    {
        Bounds overlap = GetOverlapBounds(a.bounds, b.bounds);
        if (overlap.size.x <= 0 || overlap.size.y <= 0)
            return 0f;

        return overlap.size.x * overlap.size.y;
    }

    Bounds GetOverlapBounds(Bounds a, Bounds b)
    {
        Vector3 min = Vector3.Max(a.min, b.min);
        Vector3 max = Vector3.Min(a.max, b.max);

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    float GetColliderArea(Collider2D col)
    {
        Bounds b = col.bounds;
        return b.size.x * b.size.y;
    }
}

using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        DeathPlayer health = other.GetComponentInParent<DeathPlayer>();
        if (health != null)
        {
            health.Die();
        }
    }
}

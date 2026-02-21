using UnityEngine;

public class HitboxPlayer : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private MovementPlayer movement;

    void Awake()
    {
        movement = GetComponentInParent<MovementPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyHealthKnockback enemy = other.GetComponent<EnemyHealthKnockback>();
        if (enemy == null) return;

        enemy.TakeDamage(damage, transform);
        movement.ApplyKnockback(other.transform);

        HitPause.Freeze(0.04f);
        ScreenShake.Shake(0.08f, 0.07f);
    }
}

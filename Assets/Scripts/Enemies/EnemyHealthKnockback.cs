using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealthKnockback : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth;

    [Header("Knockback")]
    public float knockbackForce = 4f;
    public float knockbackDuration = 0.1f;

    Rigidbody2D rb;
    bool isKnockedBack;
    float knockbackTimer;
    Vector2 knockbackVelocity;

    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        currentHealth -= damage;
        ApplyKnockback(attacker);

        if (currentHealth <= 0)
            Die();
    }

    void ApplyKnockback(Transform attacker)
    {
        Vector2 direction = (transform.position - attacker.position).normalized;
        knockbackVelocity = direction * knockbackForce;
        knockbackTimer = knockbackDuration;
        isKnockedBack = true;
    }

    void FixedUpdate()
    {
        if (!isKnockedBack) return;

        rb.linearVelocity = knockbackVelocity;
        knockbackTimer -= Time.fixedDeltaTime;

        if (knockbackTimer <= 0f)
        {
            isKnockedBack = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Die()
    {
        // Optional: HitPause/ScreenShake on kill here
        Destroy(gameObject);
    }
}
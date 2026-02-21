using UnityEngine;

public class ShootAtPlayerAttack : EnemyAttackBase
{
    public float shootCooldown = 2.5f;
    public float windupDelay = 0.3f;
    public GameObject projectilePrefab;

    float timer;
    bool isWindingUp;
    float windupTimer;

    public override void Tick()
    {
        if (core == null || core.player == null) return;

        if (isWindingUp)
        {
            windupTimer -= Time.deltaTime;
            if (windupTimer <= 0f)
            {
                Fire();
                isWindingUp = false;
                core.SetAttacking(false);
            }
            return;
        }

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        // Start windup
        isWindingUp = true;
        windupTimer = windupDelay;
        timer = shootCooldown;

        core.SetAttacking(true);
        core.LockMovement(windupDelay); // coordination point
    }

    void Fire()
    {
        Vector2 dir = (core.player.position - transform.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        var p = proj.GetComponent<EnemyProjectile>();
        if (p != null) p.Init(dir);
    }
}
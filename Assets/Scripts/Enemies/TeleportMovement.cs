using UnityEngine;

public class TeleportMovement : EnemyMovementBase
{
    public float teleportCooldown = 2.5f;
    public LayerMask obstacleMask;
    public float checkRadius = 0.3f;
    public int tries = 20;

    float timer;

    public override void Tick()
    {
        if (core == null || core.roomBounds == null) return;
        if (core.IsMovementLocked) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Teleport();
        timer = teleportCooldown;
    }

    void Teleport()
    {
        for (int i = 0; i < tries; i++)
        {
            Vector2 randomPos = core.roomBounds.GetRandomPointInside();

            if (!Physics2D.OverlapCircle(randomPos, checkRadius, obstacleMask))
            {
                transform.position = randomPos;
                return;
            }
        }
    }
}
using UnityEngine;
using System.Collections;

public class TeleporterEnemy : MonoBehaviour
{
    [Header("Teleport")]
    public float teleportCooldown = 2.5f;
    public LayerMask obstacleMask;

    [Header("Attack")]
    public float shootDelay = 0.3f;
    public GameObject projectilePrefab;

    private Transform player;
    private RoomBounds roomBounds;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        roomBounds = FindFirstObjectByType<RoomBounds>();

        StartCoroutine(BehaviorLoop());
    }

    IEnumerator BehaviorLoop()
    {
        while (true)
        {
            Teleport();
            yield return new WaitForSeconds(shootDelay);
            Shoot();
            yield return new WaitForSeconds(teleportCooldown);
        }
    }

    void Teleport()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector2 randomPos = roomBounds.GetRandomPointInside();

            // Check if tile is empty
            if (!Physics2D.OverlapCircle(randomPos, 0.3f, obstacleMask))
            {
                transform.position = randomPos;
                return;
            }
        }
    }

    void Shoot()
    {
        if (!player) return;

        Vector2 dir = (player.position - transform.position).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        proj.GetComponent<ProjectileEnemy>().Init(dir);
    }
}

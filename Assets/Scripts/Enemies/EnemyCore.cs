using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [Header("Shared References")]
    public Transform player;
    public RoomBounds roomBounds;

    [Header("State")]
    public bool IsMovementLocked { get; private set; }
    public bool IsAttacking { get; private set; }

    float movementLockTimer;

    void Awake()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        if (roomBounds == null)
            roomBounds = FindFirstObjectByType<RoomBounds>();
    }

    void Update()
    {
        if (movementLockTimer > 0f)
        {
            movementLockTimer -= Time.deltaTime;
            if (movementLockTimer <= 0f) IsMovementLocked = false;
        }
    }

    public void LockMovement(float seconds)
    {
        IsMovementLocked = true;
        movementLockTimer = Mathf.Max(movementLockTimer, seconds);
    }

    public void SetAttacking(bool value) => IsAttacking = value;
}
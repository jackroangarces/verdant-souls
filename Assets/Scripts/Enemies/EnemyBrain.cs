using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public EnemyMovementBase movement;
    public EnemyAttackBase attack;

    void Awake()
    {
        if (movement == null) movement = GetComponent<EnemyMovementBase>();
        if (attack == null) attack = GetComponent<EnemyAttackBase>();
    }

    void Update()
    {
        movement?.Tick();
        attack?.Tick();
    }
}
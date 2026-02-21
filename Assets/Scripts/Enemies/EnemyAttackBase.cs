using UnityEngine;

public abstract class EnemyAttackBase : MonoBehaviour
{
    protected EnemyCore core;

    protected virtual void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    public abstract void Tick();
}
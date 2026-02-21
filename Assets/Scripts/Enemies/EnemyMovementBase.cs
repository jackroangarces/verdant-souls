using UnityEngine;

public abstract class EnemyMovementBase : MonoBehaviour
{
    protected EnemyCore core;

    protected virtual void Awake()
    {
        core = GetComponent<EnemyCore>();
    }

    public abstract void Tick();
}
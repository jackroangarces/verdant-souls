using UnityEngine;

public class SpikeBlockMover : MonoBehaviour
{
    public Vector2[] localWaypoints;
    public float moveSpeed = 2f;
    public bool loop = true;

    private int currentIndex = 0;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (localWaypoints.Length == 0)
            return;

        Vector3 target = startPosition + (Vector3)localWaypoints[currentIndex];
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            currentIndex++;

            if (currentIndex >= localWaypoints.Length)
            {
                if (loop)
                    currentIndex = 0;
                else
                    currentIndex = localWaypoints.Length - 1;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 prev = Application.isPlaying ? startPosition : transform.position;

        foreach (Vector2 wp in localWaypoints)
        {
            Vector3 next = transform.position + (Vector3)wp;
            Gizmos.DrawLine(prev, next);
            Gizmos.DrawSphere(next, 0.1f);
            prev = next;
        }
    }
}

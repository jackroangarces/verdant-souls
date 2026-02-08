using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 5f;

    private Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: damage player
            Destroy(gameObject);
        }

        if (other.CompareTag("RoomBoundary"))
        {
            Destroy(gameObject);
        }
    }
}

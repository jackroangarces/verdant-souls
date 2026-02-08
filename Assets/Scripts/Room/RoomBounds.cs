using UnityEngine;

public class RoomBounds : MonoBehaviour
{
    public float width = 18f;
    public float height = 32f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(width, height, 0)
        );
    }

    public Vector2 GetRandomPointInside()
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        float x = Random.Range(-halfWidth, halfWidth);
        float y = Random.Range(-halfHeight, halfHeight);

        return (Vector2)transform.position + new Vector2(x, y);
    }

}

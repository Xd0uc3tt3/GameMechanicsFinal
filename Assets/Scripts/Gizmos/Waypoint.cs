using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float radius = 0.2f;
    public Color color = Color.yellow;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}

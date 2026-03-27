using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Transform target;
    private Vector3 lastPosition;
    private Rigidbody playerRb;

    void Start()
    {
        target = pointB;
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);

        Vector3 delta = transform.position - lastPosition;
        lastPosition = transform.position;

        if (playerRb != null)
        {
            playerRb.MovePosition(playerRb.position + delta);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerRb = collision.rigidbody;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerRb = null;
        }
    }
}

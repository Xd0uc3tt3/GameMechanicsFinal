using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    public float rotationSpeed = 90f;

    public float moveSpeed = 5f;
    public float shrinkSpeed = 4f;

    private bool isCollected = false;
    private Transform target;

    public void Collect(Transform player)
    {
        if (isCollected)
        {
            return;
        }

        isCollected = true;
        target = player;

        GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        if (!isCollected)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

            if (transform.localScale.magnitude < 0.05f || (target != null && Vector3.Distance(transform.position, target.position) < 0.2f))
            {
                Destroy(gameObject);
            }
        }
    }
}

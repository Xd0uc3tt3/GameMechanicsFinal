using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform door;
    public Vector3 openOffset;
    public float speed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;

    void Start()
    {
        closedPos = door.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        if (isOpen)
        {
            door.position = Vector3.Lerp(door.position, openPos, Time.deltaTime * speed);
        }
        else
        {
            door.position = Vector3.Lerp(door.position, closedPos, Time.deltaTime * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = false;
        }
    }
}

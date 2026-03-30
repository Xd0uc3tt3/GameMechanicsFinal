using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform door;
    public Vector3 openOffset;
    public float speed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;

    public AudioClip doorSound;
    private AudioSource audioSource;

    void Start()
    {
        closedPos = door.position;
        openPos = closedPos + openOffset;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = doorSound;
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
            PlaySound(forward: true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = false;
            PlaySound(forward: false);
        }
    }

    void PlaySound(bool forward)
    {
        if (doorSound == null) return;

        if (forward)
        {
            audioSource.pitch = 1f;
            if (!audioSource.isPlaying)
            {
                audioSource.time = 0f;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.pitch = -1f;
            if (!audioSource.isPlaying)
            {
                audioSource.time = doorSound.length - 0.01f;
                audioSource.Play();
            }
        }
    }
}

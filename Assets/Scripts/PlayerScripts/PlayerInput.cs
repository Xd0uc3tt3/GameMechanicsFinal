using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Transform Player;
    public Vector2 moveInput;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded = true;

    public PlayerScript playerScript;

    public void Awake()
    {
        rb = Player.GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerScript.LoseLife();
        }
    }

    private void Update()
    {
        if (moveInput != Vector2.zero) 
        { 
            Player.Translate(new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * 5.0f); 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}

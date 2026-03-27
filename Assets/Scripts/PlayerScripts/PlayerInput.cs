using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    public Transform Player;
    public Vector2 moveInput;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private bool canRestart = true;

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
        if (playerScript.coins <= 0)
        {
            return;
        }

        if (isGrounded)
        {
            playerScript.coins--;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        playerScript.LoseLife();
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

    private void LateUpdate()
    {
        canRestart = true;
    }

}

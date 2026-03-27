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
    public float rotationSpeed = 0.15f;

    private bool canRestart = true;

    private Rigidbody rb;
    private bool isGrounded = true;

    public PlayerScript playerScript;
    public Transform cameraPivot;

    private ShopTrigger currentShopTrigger;

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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (currentShopTrigger != null && currentShopTrigger.PlayerInRange)
        {
            currentShopTrigger.ToggleShop();
        }
    }

    void FixedUpdate()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

            Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
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

    public void SetShopTrigger(ShopTrigger trigger)
    {
        currentShopTrigger = trigger;
    }

    public void ClearShopTrigger()
    {
        currentShopTrigger = null;
    }

}

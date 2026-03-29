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

    private int jumpCount = 0;
    private const int maxJumps = 2;

    public bool hasDoubleJump = false;

    private bool canRestart = true;

    private Rigidbody rb;
    public bool isGrounded = true;

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
        if (!context.performed) 
        { 
            return; 
        }

        if (isGrounded)
        {
            if (playerScript.coins <= 0)
            {
                return;
            }

            playerScript.coins--;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpCount = 1;
        }
        else if (hasDoubleJump && jumpCount == 1)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount = 2;
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (playerScript.lives <= 0 || playerScript.isRespawning)
        {
            return;
        }

        playerScript.coins += playerScript.RespawnCoinReward;
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
            jumpCount = 0;
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

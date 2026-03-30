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

    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    public int jumpCount = 0;
    private const int maxJumps = 2;

    public bool hasDoubleJump = false;
    public bool hasDash = false;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;

    private bool canRestart = true;

    private Rigidbody rb;
    public bool isGrounded = true;

    public PlayerScript playerScript;
    public Transform cameraPivot;

    public AudioClip jumpSound;
    public AudioClip DashSound;
    public AudioClip failSound;
    public AudioClip RestartHoldSound;
    public AudioClip RestartSound;

    private float restartHoldTimer = 0f;
    private const float restartHoldDuration = 7.5f;
    private bool isHoldingRestart = false;

    private ShopTrigger currentShopTrigger;


    public void Awake()
    {
        rb = Player.GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (!hasDash)
        {
            return;
        }

        if (isDashing || dashCooldownTimer > 0f)
        {
            return;
        }

        Vector3 dashDirection;
        if (moveInput.sqrMagnitude > 0.01f)
        {
            playerScript.audioSource.PlayOneShot(DashSound);
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
            dashDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;
        }
        else
        {
            dashDirection = Player.forward;
        }

        rb.linearVelocity = new Vector3(dashDirection.x * dashForce, rb.linearVelocity.y, dashDirection.z * dashForce);
        isDashing = true;
        dashTimer = dashDuration;
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
                playerScript.audioSource.PlayOneShot(failSound);
                return;
            }

            playerScript.coins--;
            playerScript.audioSource.PlayOneShot(jumpSound);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpCount = 1;
        }
        else if (hasDoubleJump && jumpCount == 1)
        {
            playerScript.audioSource.PlayOneShot(jumpSound);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount = 2;
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (playerScript.isRespawning)
            {
                return;
            }

            isHoldingRestart = true;
            restartHoldTimer = 0f;
            playerScript.audioSource.PlayOneShot(RestartHoldSound);
        }
        else if (context.canceled)
        {
            isHoldingRestart = false;
            restartHoldTimer = 0f;
            playerScript.audioSource.Stop();
        }
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
        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x * 0.1f, rb.linearVelocity.y, rb.linearVelocity.z * 0.1f);
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
        }
        else if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.fixedDeltaTime;
        }

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput.sqrMagnitude > 0.01f && !isDashing)
        {
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

            Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
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

    void Update()
    {
        if (isHoldingRestart)
        {
            restartHoldTimer += Time.deltaTime;

            if (restartHoldTimer >= restartHoldDuration)
            {
                isHoldingRestart = false;
                restartHoldTimer = 0f;
                playerScript.audioSource.Stop();
                playerScript.audioSource.PlayOneShot(RestartSound);
                playerScript.coins += playerScript.RespawnCoinReward;
                playerScript.LoseLife();
            }
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

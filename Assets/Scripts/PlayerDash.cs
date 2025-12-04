using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private CharacterController cc;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public KeyCode dashKey = KeyCode.LeftShift;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimer = 0f;
    private Vector3 dashDirection;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey) && canDash)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                EndDash();
            }

            cc.Move(dashDirection * dashSpeed * Time.deltaTime);
        }
    }

    void StartDash()
    {
        canDash = false;
        isDashing = true;
        dashTimer = dashDuration;

        // Get input direction
        float moveInput = Input.GetAxisRaw("Vertical");
        Vector3 forward = transform.forward;

        dashDirection = forward.normalized * Mathf.Sign(moveInput);

        // Default to forward if no input
        if (dashDirection == Vector3.zero)
            dashDirection = transform.forward;

        Debug.Log("Dash started in direction: " + dashDirection);

        Invoke(nameof(ResetDash), dashCooldown);
    }

    void EndDash()
    {
        isDashing = false;
        Debug.Log("Dash ended");
    }

    void ResetDash()
    {
        canDash = true;
        Debug.Log("Dash ready again");
    }
}
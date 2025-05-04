using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;              // Normal movement speed 
    public float attackMovement;         // Attacking movement speed 
    public float shieldedMoveSpeed;      // Movement speed while blocking
    public float dashSpeed;              // Dash speed multiplier
    public float weakDashNerf;           // Speed reduction for weak dashes (after main dashes run out)
    public float dashDuration;           // Duration the dash lasts
    public float dashCooldown;           // Time before dashes replenish
    public float maxDashAmount;          // Max amount of strong dashes available before cooldown
    public float maxWeakDashAmount;      // Max total amount of weak + strong dashes allowed before cooldown

    [Header("Sound Effects")]
    public GameObject dashSound;

    private Rigidbody2D rb { get => GetComponent<Rigidbody2D>(); } // Cached reference to Rigidbody2D
    private Vector2 moveDirection;       // Direction of player movement from input
    private Vector2 dashDirection;       // Stored direction for dash movement
    private bool isDashing = false;      // Whether the player is currently dashing
    private float dashTime = 0f;         // Current dash timer
    private float cooldownTime = 0f;     // Cooldown timer for dash replenishment
    private float dashAmount = 1f;       // Current number of available dashes

    private void Update()
    {
        ApplyMovement();
        ApplyDash();
    }

    /// <summary>
    /// Applies dash movement each frame if dashing is active, including full or weakened dash speed based on remaining charges.
    /// Handles dash duration countdown and replenishes dash charges after the cooldown period ends.
    /// </summary>
    private void ApplyDash()
    {
        // Handle dash movement
        if (isDashing)
        {
            if (dashAmount > 0)
            {
                // Apply full dash speed
                rb.linearVelocity += dashDirection * dashSpeed;
            }
            else
            {
                // Apply weaker dash if strong dashes are depleted
                rb.linearVelocity += dashDirection * (dashSpeed - dashSpeed * weakDashNerf * Mathf.Abs(dashAmount));
            }

            // Reduce dash timer
            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false; // End dash
            }
        }
        else
        {
            // Handle cooldown for restoring dash charges
            if (cooldownTime > 0f)
            {
                cooldownTime -= Time.deltaTime;
            }
            else
            {
                dashAmount = maxDashAmount;
            }
        }
    }

    /// <summary>
    /// Applies current movement velocity based on direction and weapon/blocking status.
    /// </summary>
    private void ApplyMovement()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    /// <summary>
    /// Sets movement-related stats from the weapon or uses default values if unavailable.
    /// </summary>
    private void SetMovementStats()
    {
        moveSpeed = 15f;
        attackMovement = 15f;
        shieldedMoveSpeed = 7f;
        dashSpeed = 40f;
        weakDashNerf = 0.3f;
        dashDuration = 0.1f;
        dashCooldown = 2.5f;
        maxDashAmount = 3f;
        maxWeakDashAmount = 4f;
    }

    /// <summary>
    /// Called by the Input System when movement input is received.
    /// Updates the move direction based on player input.
    /// </summary>
    /// <param name="context">The input context containing directional data.</param>
    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Called by the Input System when the dash action is triggered.
    /// Starts a dash if possible and handles dash logic.
    /// </summary>
    /// <param name="context">The input context for the dash action.</param>
    public void Dash(InputAction.CallbackContext context)
    {
        Vector2 forward = new Vector2(transform.up.x, transform.up.y);

        // Prevent dashing in a direction not aligned enough with forward
        if (Vector2.Dot(moveDirection.normalized, forward) > 1.1f) return;

        // Only allow dash if not currently dashing and max weak dashes not exceeded
        if (context.started && !isDashing && (Mathf.Abs(dashAmount) < maxWeakDashAmount))
        {
            isDashing = true;
            dashTime = dashDuration;           // Set dash duration
            cooldownTime = dashCooldown;       // Start cooldown timer
            dashDirection = moveDirection;     // Store dash direction

            // Grant temporary invincibility during normal dashes
            if (dashAmount > 0)
            {
                if (dashSound)
                    Instantiate(dashSound, transform.position, Quaternion.identity);
                //ApplyInvincibility(dashDuration + (dashAmount * 0.2f));
            }

            dashAmount--;                      // Reduce dash count
        }
    }
    public void Activate() { }
}
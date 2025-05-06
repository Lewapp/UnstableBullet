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
    public GameObject swapObject;

    private Rigidbody2D rb { get => GetComponent<Rigidbody2D>(); } // Cached reference to Rigidbody2D
    private Vector2 moveDirection;       // Direction of player movement from input

    private void Update()
    {
        ApplyMovement();
    }

    /// <summary>
    /// Applies current movement velocity based on direction and weapon/blocking status.
    /// </summary>
    private void ApplyMovement()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
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

    public void Swap(InputAction.CallbackContext context)
    {
        if (!swapObject)
            return;

        Vector2 savedPosition = transform.position;
        transform.position = swapObject.transform.position;
        swapObject.transform.position = savedPosition;
    }
}
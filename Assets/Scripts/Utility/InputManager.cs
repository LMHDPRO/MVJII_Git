using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class handles reading the input given by the player through input devices
/// </summary>
public class InputManager : MonoBehaviour
{
    // A global reference for the input manager that other scripts can access to read the input
    public static InputManager instance;

    /// <summary>
    /// Standard Unity Function called when the script is loaded
    /// </summary>
    private void Awake()
    {
        ResetValuesToDefault();
        // Set up the instance of this
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Sets all the input variables to their default values
    /// </summary>
    void ResetValuesToDefault()
    {
        horizontalMoveAxis = default;
        verticalMoveAxis = default;

        horizontalLookAxis = default;
        verticalLookAxis = default;

        firePressed = default;
        fireHeld = default;

        pausePressed = default;
        rightClickPressed = default;
    }

    [Header("Player Movement Input")]
    [Tooltip("The move input along the horizontal")]
    public float horizontalMoveAxis;
    [Tooltip("The move input along the vertical")]
    public float verticalMoveAxis;
    public void ReadMovementInput(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        horizontalMoveAxis = inputVector.x;
        verticalMoveAxis = inputVector.y;
    }

    [Header("Look Around Input")]
    public float horizontalLookAxis;
    public float verticalLookAxis;

    /// <summary>
    /// Reads the mouse position input
    /// </summary>
    public void ReadMousePositionInput(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        if (Mathf.Abs(inputVector.x) > 1 && Mathf.Abs(inputVector.y) > 1)
        {
            horizontalLookAxis = inputVector.x;
            verticalLookAxis = inputVector.y;
        }
    }

    [Header("Player Fire Input")]
    [Tooltip("Whether or not the fire button was pressed this frame")]
    public bool firePressed;
    [Tooltip("Whether or not the fire button is being held")]
    public bool fireHeld;

    /// <summary>
    /// Reads the fire input
    /// </summary>
    public void ReadFireInput(InputAction.CallbackContext context)
    {
        firePressed = !context.canceled;
        fireHeld = !context.canceled;
        StartCoroutine("ResetFireStart");
    }

    private IEnumerator ResetFireStart()
    {
        yield return new WaitForEndOfFrame();
        firePressed = false;
    }

    [Header("Pause Input")]
    public bool pausePressed;
    public void ReadPauseInput(InputAction.CallbackContext context)
    {
        pausePressed = !context.canceled;
        StartCoroutine(ResetPausePressed());
    }

    IEnumerator ResetPausePressed()
    {
        yield return new WaitForEndOfFrame();
        pausePressed = false;
    }

    [Header("Dash Input (Right Click)")]
    [Tooltip("Detects if the right mouse button was pressed (Dash Action)")]
    public bool rightClickPressed;

    /// <summary>
    /// Reads the right-click input (Dash)
    /// </summary>
    public void ReadDashInput(InputAction.CallbackContext context)
    {
        rightClickPressed = !context.canceled;
        StartCoroutine(ResetRightClick());
    }

    private IEnumerator ResetRightClick()
    {
        yield return new WaitForEndOfFrame();
        rightClickPressed = false;
    }
}

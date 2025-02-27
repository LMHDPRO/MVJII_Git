using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    [Header("GameObject/Component References")]
    [Tooltip("The animator component used to animate the player.")]
    public Animator animator = null;
    [Tooltip("The Rigidbody2D component to use in \"Asteroids Mode\".")]
    public Rigidbody2D myRigidbody = null;

    [Header("Movement Variables")]
    [Tooltip("The speed at which the player will move.")]
    public float moveSpeed = 10.0f;
    [Tooltip("The speed at which the player rotates in asteroids movement mode.")]
    public float rotationSpeed = 60f;

    // The InputManager to read input from
    private InputManager inputManager;

    public enum AimModes { AimTowardsMouse, AimForwards };
    public AimModes aimMode = AimModes.AimTowardsMouse;

    public enum MovementModes { MoveHorizontally, MoveVertically, FreeRoam, Asteroids };
    public MovementModes movementMode = MovementModes.FreeRoam;

    private bool canAimWithMouse => aimMode == AimModes.AimTowardsMouse;
    private bool lockXCoordinate => movementMode == MovementModes.MoveVertically;
    private bool lockYCoordinate => movementMode == MovementModes.MoveHorizontally;

    private void Start()
    {
        SetupInput();
    }

    private void Update()
    {
        HandleInput();
        SignalAnimator();
    }

    private void SetupInput()
    {
        inputManager = InputManager.instance;
        if (inputManager == null)
        {
            Debug.LogWarning("No InputManager found in the scene!");
        }
    }

    private void HandleInput()
    {
        if (inputManager == null) return;

        Vector2 lookPosition = GetLookPosition();
        Vector3 movementVector = new Vector3(inputManager.horizontalMoveAxis, inputManager.verticalMoveAxis, 0);
        MovePlayer(movementVector);
        LookAtPoint(lookPosition);
    }

    private void SignalAnimator()
    {
        if (animator != null)
        {
            // Aquí puedes agregar parámetros para animaciones
            // animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    public Vector2 GetLookPosition()
    {
        if (inputManager == null) return transform.up;
        return aimMode == AimModes.AimForwards
            ? (Vector2)transform.up
            : new Vector2(inputManager.horizontalLookAxis, inputManager.verticalLookAxis);
    }

    private void MovePlayer(Vector3 movement)
    {
        if (movementMode == MovementModes.Asteroids)
        {
            if (myRigidbody == null)
            {
                myRigidbody = GetComponent<Rigidbody2D>();
                if (myRigidbody == null) return;
            }

            Vector2 force = transform.up * movement.y * moveSpeed * Time.deltaTime;
            myRigidbody.AddForce(force);

            float newZRotation = transform.eulerAngles.z - rotationSpeed * movement.x * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, newZRotation);
        }
        else
        {
            if (lockXCoordinate) movement.x = 0;
            if (lockYCoordinate) movement.y = 0;
            transform.position += movement * moveSpeed * Time.deltaTime;
        }
    }

    private void LookAtPoint(Vector3 point)
    {
        if (Time.timeScale <= 0 || Camera.main == null) return;

        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(point);
        Vector2 lookDirection = screenPoint - (Vector2)transform.position;

        if (canAimWithMouse)
        {
            transform.up = lookDirection;
        }
        else if (myRigidbody != null)
        {
            myRigidbody.freezeRotation = true;
        }
    }
}

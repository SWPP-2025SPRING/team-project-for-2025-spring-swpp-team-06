using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    private Rigidbody playerRb;
    private IPlayerState currentState;

    [Header("Movement")]
    public float acceleration = 10f;
    public float maxSpeed = 5f;

    public GroundRotator groundRotator;

    [Header("Rotation")]
    public float rotationAlignmentSpeed = 180f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        groundRotator = GameObject.FindWithTag("Ground")?.GetComponent<GroundRotator>();

        if (groundRotator == null)
        {
            Debug.LogError("GroundRotator를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        ChangeState(new NormalState());
    }

    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate(this);
    }

    void Update()
    {
        currentState?.Update(this);

    }

    public void HandleMovement()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) moveInput = -1f;

        Vector3 groundVelocityAtPlayerPos = Vector3.zero;
        Vector3 groundAngularVelocity = Vector3.zero;

        if (groundRotator != null)
        {
            groundAngularVelocity = groundRotator.GetAngularVelocity();
            Vector3 playerPosRelToPivot = playerRb.position;
            groundVelocityAtPlayerPos = Vector3.Cross(groundAngularVelocity, playerPosRelToPivot);
        }

        Vector3 relativeVelocity = playerRb.velocity - groundVelocityAtPlayerPos;
        Vector3 moveDirection = Vector3.forward;
        float relativeSpeedForward = Vector3.Dot(relativeVelocity, moveDirection);
        Vector3 moveForce = moveDirection * moveInput * acceleration;

        bool canAccelerate = false;
        if (moveInput > 0 && relativeSpeedForward < maxSpeed) canAccelerate = true;
        else if (moveInput < 0 && relativeSpeedForward > -maxSpeed) canAccelerate = true;

        if (canAccelerate)
        {
            playerRb.AddForce(moveForce, ForceMode.Acceleration);
        }

        Vector3 horizontalRelativeVelocity = new Vector3(relativeVelocity.x, 0f, relativeVelocity.z);
        float minSpeedForRotation = 0.1f;

        if (horizontalRelativeVelocity.sqrMagnitude > minSpeedForRotation * minSpeedForRotation)
        {
            Vector3 targetDirection = horizontalRelativeVelocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            float step = rotationAlignmentSpeed * Time.fixedDeltaTime;
            Quaternion newRotation = Quaternion.RotateTowards(playerRb.rotation, targetRotation, step);

            Vector3 euler = newRotation.eulerAngles;

            float clampedY = euler.y > 180f ? euler.y - 360f : euler.y;
            clampedY = Mathf.Clamp(clampedY, -90f, 90f);
            euler.y = clampedY < 0f ? clampedY + 360f : clampedY;

            newRotation = Quaternion.Euler(euler);
            playerRb.MoveRotation(newRotation);
        }
    }

}

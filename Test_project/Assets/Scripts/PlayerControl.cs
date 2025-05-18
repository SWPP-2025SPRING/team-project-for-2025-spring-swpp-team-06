using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    private Rigidbody playerRb;

    [Header("Movement")]
    public float acceleration = 10f;
    public float maxSpeed = 5f;

    public GroundRotator groundRotator;

    [Header("Rotation")]

    public float rotationAlignmentSpeed = 180f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        if (groundRotator == null)
        {
            GameObject ground = GameObject.FindWithTag("Ground");
            if (ground != null)
            {
                groundRotator = ground.GetComponent<GroundRotator>();
            }

            if (groundRotator == null)
            {
                Debug.LogError("PlayerControl: GroundRotator를 찾을 수 없습니다. Ground 오브젝트에 'GroundRotator' 스크립트가 붙어 있고, 'Ground' 태그가 설정되어 있는지 확인하세요.");
                enabled = false;
                return;
            }
        }
    }

    void FixedUpdate()
    {
<<<<<<< Updated upstream

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


        float relativeSpeedForward = Vector3.Dot(relativeVelocity, transform.forward);
        Vector3 moveForce = transform.forward * moveInput * acceleration;

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

            playerRb.MoveRotation(newRotation);
        }
=======
        currentState?.FixedUpdate(this);
        currentState?.HandleMovement(this);
    }

    void Update()
    {
        currentState?.Update(this);

    }

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }
>>>>>>> Stashed changes

    }
}
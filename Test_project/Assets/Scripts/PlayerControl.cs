using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    private Rigidbody playerRb;

    [Header("Movement")]
    public float acceleration = 100f;
    public float maxSpeed = 50f;

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
                Debug.LogError("PlayerControl: GroundRotator�� ã�� �� �����ϴ�. Ground ������Ʈ�� 'GroundRotator' ��ũ��Ʈ�� �پ� �ְ�, 'Ground' �±װ� �����Ǿ� �ִ��� Ȯ���ϼ���.");
                enabled = false;
                return;
            }
        }
    }

    void FixedUpdate()
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

        Vector3 alignedForward = (groundRotator != null)
            ? groundRotator.transform.rotation * Vector3.forward
            : transform.forward;
        float relativeSpeedForward = Vector3.Dot(relativeVelocity, alignedForward);
        Vector3 moveForce = alignedForward * moveInput * acceleration;

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

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection,
                groundRotator != null ? groundRotator.transform.up : Vector3.up);

            float step = rotationAlignmentSpeed * Time.fixedDeltaTime;
            Quaternion newRotation = Quaternion.RotateTowards(playerRb.rotation, targetRotation, step);

            playerRb.MoveRotation(newRotation);
        }

    }
}
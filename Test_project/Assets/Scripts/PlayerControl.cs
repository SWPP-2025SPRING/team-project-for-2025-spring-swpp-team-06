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
    public float turnSpeed = 100f;

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
            }
        }
    }

    void FixedUpdate()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) moveInput = -1f;

        float turnInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) turnInput = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) turnInput = 1f;

        Vector3 groundVelocityAtPlayerPos = Vector3.zero;
        Vector3 groundAngularVelocity = Vector3.zero;
        if (groundRotator != null)
        {
            groundAngularVelocity = groundRotator.GetAngularVelocity();
            Vector3 playerPosRelToPivot = playerRb.position;
            groundVelocityAtPlayerPos = Vector3.Cross(groundAngularVelocity, playerPosRelToPivot);
        }

        Vector3 relativeVelocity = playerRb.velocity - groundVelocityAtPlayerPos;
        float currentSpeed = relativeVelocity.magnitude;
        float relativeSpeedForward = Vector3.Dot(relativeVelocity, transform.forward);

        Vector3 moveForce = transform.forward * moveInput * acceleration;
        bool canAccelerate = false;
        if (moveInput > 0 && relativeSpeedForward < maxSpeed) canAccelerate = true;
        else if (moveInput < 0 && relativeSpeedForward > -maxSpeed) canAccelerate = true;

        if (canAccelerate)
        {
            playerRb.AddForce(moveForce);
        }

        Vector3 totalVelocity = playerRb.velocity;
        Vector3 groundVelocity = groundVelocityAtPlayerPos;
        Vector3 playerOnlyVelocity = totalVelocity - groundVelocity;

        float groundSpeed = groundVelocity.magnitude;
        float playerSpeed = playerOnlyVelocity.magnitude;

        float rotationRatio = 0f;
        if (groundSpeed > 0.01f)
        {
            rotationRatio = Mathf.Clamp(playerSpeed / groundSpeed, 0f, 1f);
        }

        float actualTurnInput = turnInput * rotationRatio;
        transform.Rotate(Vector3.up, actualTurnInput * turnSpeed * Time.fixedDeltaTime);
    }
}

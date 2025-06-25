using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    private float maxRotationSpeed = 40f;
    private float rotationAcceleration = 100f;

    private float currentAngularVelocity = 0f;
    private float currentRotationInput = 0f;

    private float rotationMultiplier = 1f;
    public bool reverseInput = false;

    private PlayerControl playerControl;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerControl = playerObj.GetComponent<PlayerControl>();
            if (playerControl == null)
            {
                Debug.LogError("Player object found, but no PlayerControl component attached.");
            }
        }
        else
        {
            Debug.LogError("No object with tag 'Player' found.");
        }
    }

    void FixedUpdate()
    {
        if (InGameUIControl.isMenuPopped) return;
        currentRotationInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            currentRotationInput = Input.GetKey(KeyCode.LeftArrow) ? 1f : -1f;
        }

        if (reverseInput)
        {
            currentRotationInput *= -1f;
        }

        float targetAngularVelocity = currentRotationInput * maxRotationSpeed * rotationMultiplier;

        if(playerControl != null && playerControl.HasState<SleepingState>())
        {
            targetAngularVelocity = 0f; // Stop rotation if player is sleeping
        }

        currentAngularVelocity = Mathf.MoveTowards(
            currentAngularVelocity,
            targetAngularVelocity,
            rotationAcceleration * rotationMultiplier * Time.fixedDeltaTime
        );

        if (!Mathf.Approximately(currentAngularVelocity, 0f))
        {
            float rotationAmount = currentAngularVelocity * Time.fixedDeltaTime;
            transform.Rotate(Vector3.forward, rotationAmount, Space.World);
        }
    }

    public Vector3 GetAngularVelocity()
    {
        return Vector3.forward * currentAngularVelocity * Mathf.Deg2Rad;
    }

    public void SetRotationMultiplier(float multiplier)
    {
        rotationMultiplier = multiplier;
    }

    public float GetRotationMultiplier()
    {
        return rotationMultiplier;
    }

    public void StopMove(){
        currentRotationInput = 0f;
    }
}

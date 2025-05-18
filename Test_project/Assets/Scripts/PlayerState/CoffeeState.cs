using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeState : IPlayerState
{
    private float timer;
    private float duration = 5f;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        if (player.groundRotator != null)
        {
            player.groundRotator.SetRotationMultiplier(1.2f);
        }
    }

    public void Exit(PlayerControl player)
    {
        if (player.groundRotator != null)
        {
            player.groundRotator.SetRotationMultiplier(1f);
        }
    }

    public void Update(PlayerControl player)
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            player.ChangeState(new NormalState());
        }
    }

    public void FixedUpdate(PlayerControl player) { }

    public void HandleMovement(PlayerControl player)
    {
        float acceleration = player.acceleration * 1.2f;
        float maxSpeed = player.maxSpeed * 1.2f;

        float moveInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) moveInput = -1f;

        float rotationSpeed = player.rotationAlignmentSpeed;

        Rigidbody rb = player.GetComponent<Rigidbody>();

        Vector3 groundVelocityAtPlayerPos = Vector3.zero;
        if (player.groundRotator != null)
        {
            Vector3 angularVel = player.groundRotator.GetAngularVelocity();
            groundVelocityAtPlayerPos = Vector3.Cross(angularVel, rb.position);
        }

        Vector3 relativeVelocity = rb.velocity - groundVelocityAtPlayerPos;
        float forwardSpeed = Vector3.Dot(relativeVelocity, player.transform.forward);
        Vector3 moveForce = player.transform.forward * moveInput * acceleration;

        bool canAccelerate = moveInput > 0 && forwardSpeed < maxSpeed
                          || moveInput < 0 && forwardSpeed > -maxSpeed;

        if (canAccelerate)
            rb.AddForce(moveForce, ForceMode.Acceleration);

        Vector3 horizontalVelocity = new Vector3(relativeVelocity.x, 0, relativeVelocity.z);
        if (horizontalVelocity.sqrMagnitude > 0.01f)
        {
            Vector3 dir = horizontalVelocity.normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            float step = rotationSpeed * Time.fixedDeltaTime;
            Quaternion newRot = Quaternion.RotateTowards(rb.rotation, targetRot, step);

            Vector3 euler = newRot.eulerAngles;
            float clampedY = euler.y > 180 ? euler.y - 360 : euler.y;
            clampedY = Mathf.Clamp(clampedY, -90, 90);
            euler.y = clampedY < 0 ? clampedY + 360 : clampedY;

            newRot = Quaternion.Euler(euler);
            rb.MoveRotation(newRot);
        }

    }

    public void ResetTimer()
    {
        timer = 0f;
    }
}

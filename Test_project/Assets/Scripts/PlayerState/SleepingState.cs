using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingState : IPlayerState, IMovementModifier, IRemovable
{
    private float timer = 0f;
    private float sleepDuration = 3f;
    public bool ShouldRemove { get; private set; } = false;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.groundRotator.enabled = false;
        ShouldRemove = false;
    }

    public void Exit(PlayerControl player)
    {
        player.groundRotator.enabled = true;
    }

    public void Update(PlayerControl player)
    {
        timer += Time.deltaTime;
        if (timer >= sleepDuration)
        {
            ShouldRemove = true;
        }
    }

    public void FixedUpdate(PlayerControl player) 
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public bool IsBlocking()
    {
        return true;
    }

    public bool IsPenalty()
    {
        return true;
    }

    public float GetAccelerationFactor()
    {
        return 0f;
    }

    public float GetMaxSpeedFactor()
    {
        return 0f;
    }

    
}

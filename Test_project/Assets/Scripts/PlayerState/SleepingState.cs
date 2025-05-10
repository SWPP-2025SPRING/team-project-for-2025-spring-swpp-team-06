using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingState : IPlayerState
{
    private float sleepDuration = 3f;
    private float timer;

    private GroundRotator groundRotator;
    private bool wasGroundRotatorEnabled;

    public void Enter(PlayerControl player)
    {
        timer = 0f;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        groundRotator = player.groundRotator;
        if (groundRotator != null)
        {
            wasGroundRotatorEnabled = groundRotator.enabled;
            groundRotator.enabled = false;
        }
    }

    public void Exit(PlayerControl player)
    {
        if (groundRotator != null)
        {
            groundRotator.enabled = wasGroundRotatorEnabled;
        }
    }

    public void FixedUpdate(PlayerControl player)
    {
    }

    public void Update(PlayerControl player)
    {
        timer += Time.deltaTime;
        if (timer >= sleepDuration)
        {
            player.ChangeState(new NormalState());
        }
    }
}

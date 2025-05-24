using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeState : IPlayerState, IMovementModifier, IRemovable
{
    private float duration = 5f;
    private float timer = 0f;

    public bool ShouldRemove { get; private set; } = false;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        ShouldRemove = false;
    }

    public void Exit(PlayerControl player)
    {
    }

    public void Update(PlayerControl player)
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            ShouldRemove = true;
        }
    }

    public void FixedUpdate(PlayerControl player)
    {
    }

    public bool IsBlocking()
    {
        return false;
    }

    public bool IsPenalty()
    {
        return false;
    }

    public float GetAccelerationFactor()
    {
        return 0.5f;
    }

    public float GetMaxSpeedFactor()
    {
        return 0.5f;
    }

    public void ResetTimer()
    {
        timer = 0f;
    }
}

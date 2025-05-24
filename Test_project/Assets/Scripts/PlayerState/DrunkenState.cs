using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkenState : IPlayerState, IRemovable
{
    private float duration = 5f;
    private float timer;
    private GroundRotator groundRotator;

    public bool ShouldRemove { get; private set; } = false;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        groundRotator = player.groundRotator;
        if (groundRotator != null)
        {
            groundRotator.reverseInput = true;
        }
        ShouldRemove = false;
    }

    public void Exit(PlayerControl player)
    {
        if (groundRotator != null)
        {
            groundRotator.reverseInput = false;
        }
    }

    public void Update(PlayerControl player)
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            ShouldRemove = true;
        }
    }

    public void FixedUpdate(PlayerControl player) { }

    public bool IsBlocking()
    {
        return false;
    }    

    public bool IsPenalty()
    {
        return true;
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkState : IPlayerState, IRemovable
{
    private float duration = 3f;
    private float timer = 0f;
    public bool ShouldRemove { get; private set; } = false;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        ShouldRemove = false;
        if (player.HasState<DrunkenState>())
        {
            player.RemoveState(player.GetState<DrunkenState>());
        }

        if (player.HasState<SleepingState>())
        {
            player.RemoveState(player.GetState<SleepingState>());
        }

        if (player.HasState<PlayerGamingState>())
        {
            player.RemoveState(player.GetState<PlayerGamingState>());
        }
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
        return true;
    }

    public bool IsPenalty()
    {
        return false;
    }
}

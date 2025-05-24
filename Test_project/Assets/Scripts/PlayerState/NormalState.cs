using UnityEngine;

public class NormalState : IPlayerState, IMovementModifier
{
    public void Enter(PlayerControl player)
    {
    }

    public void Exit(PlayerControl player)
    {
    }

    public void Update(PlayerControl player)
    {
    }

    public void FixedUpdate(PlayerControl player)
    {
        player.MovePlayer();
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
        return 1f;
    }

    public float GetMaxSpeedFactor()
    {
        return 1f;
    }
}

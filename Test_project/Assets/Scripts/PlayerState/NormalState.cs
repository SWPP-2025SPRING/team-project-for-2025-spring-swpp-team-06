using UnityEngine;

public class NormalState : IPlayerState, IMovementModifier
{
    public void Enter(PlayerControl player)
    {
    }

    public void Exit(PlayerControl player)
    {
    }

    public void FixedUpdate(PlayerControl player)
    {
        player.MovePlayer();
    }

    public Type TypeOf()
    {
        return Type.Normal;
    }

    public float GetAccelerationFactor()
    {
        return 1f;
    }

    public float GetMaxSpeedFactor()
    {
        return 1f;
    }
    public void ResetTimer()
    {
        
    }

}

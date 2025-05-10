using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : IPlayerState
{
    public void Enter(PlayerControl player)
    {
        Debug.Log("상태: Normal");
    }

    public void Exit(PlayerControl player) { }

    public void FixedUpdate(PlayerControl player)
    {
        player.HandleMovement();
    }

    public void Update(PlayerControl player)
    {
        // 예: 추가 행동
    }
}

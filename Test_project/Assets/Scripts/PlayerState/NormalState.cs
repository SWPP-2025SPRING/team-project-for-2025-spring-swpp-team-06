using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : IPlayerState
{
    public void Enter(PlayerControl player)
    {
        Debug.Log("����: Normal");
    }

    public void Exit(PlayerControl player) { }

    public void FixedUpdate(PlayerControl player)
    {
        player.HandleMovement();
    }

    public void Update(PlayerControl player)
    {
        // ��: �߰� �ൿ
    }
}

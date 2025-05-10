using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void Enter(PlayerControl player);
    void Exit(PlayerControl player);
    void FixedUpdate(PlayerControl player);
    void Update(PlayerControl player);
}


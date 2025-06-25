using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Type
{
    FullPenalty = 0, OverlapOKPenalty = 1, Coffee = 2, EnergyDrink = 3, Normal = 4
}

public interface IPlayerState
{
    void Enter(PlayerControl player);
    void Exit(PlayerControl player);
    void FixedUpdate(PlayerControl player);

    Type TypeOf();

    void ResetTimer();
}




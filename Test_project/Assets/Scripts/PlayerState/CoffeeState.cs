using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeState : IPlayerState, IMovementModifier, IRemovable
{
    private float duration = 5f;
    private float timer = 0f;

    public bool ShouldRemove { get; private set; } = false;
    private InGameUIControl uiScript;

    public void Enter(PlayerControl player)
    {
        timer = 0f;
        ShouldRemove = false;

        GameObject uiObject = GameObject.FindWithTag("UI");
        if (uiObject != null)
        {
            uiScript = uiObject.GetComponent<InGameUIControl>();
            uiScript?.ToggleBuff(true);
        }
    }

    public void Exit(PlayerControl player)
    {
        uiScript?.ToggleBuff(false);
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
    public bool IsCoffee()
    {
        return true;
    }

    public float GetAccelerationFactor()
    {
        return 1.25f;
    }

    public float GetMaxSpeedFactor()
    {
        return 1.25f;
    }

    public void ResetTimer()
    {
        timer = 0f;
    }
}

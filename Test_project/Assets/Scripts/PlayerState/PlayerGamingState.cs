using UnityEngine;

public class PlayerGamingState : IPlayerState, IMovementModifier, IRemovable
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
            uiScript?.TogglePenalty(true);
        }

        Debug.Assert(uiScript != null);

        uiScript.TurnOnGauge(uiScript.gamingTimeGauge);

        uiScript.InitiateGauge(duration, uiScript.gamingTimeGauge);
    }

    public void Exit(PlayerControl player)
    {
        uiScript?.TogglePenalty(false);
        uiScript.TurnOffGauge(uiScript.gamingTimeGauge);
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
        return true;
    }
    public bool IsCoffee()
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
        uiScript.InitiateGauge(duration, uiScript.gamingTimeGauge);
    }

    public bool IsSoju(){ return false; }

    
}

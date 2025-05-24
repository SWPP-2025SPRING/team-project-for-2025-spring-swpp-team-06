using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    private Rigidbody playerRb;

    public GroundRotator groundRotator;

    [Header("Movement")]
    public float acceleration = 10f;
    public float maxSpeed = 5f;

    [Header("Rotation")]
    public float rotationAlignmentSpeed = 180f;

    public int drinkCounts = 1;

    private List<IPlayerState> stateList = new List<IPlayerState>();
    public IReadOnlyList<IPlayerState> States => stateList.AsReadOnly();

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        groundRotator = GameObject.FindWithTag("Ground")?.GetComponent<GroundRotator>();

        if (groundRotator == null)
        {
            Debug.LogError("GroundRotator를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        PushState(new NormalState());
    }

    void Update()
    {
        for (int i = stateList.Count - 1; i >= 0; i--)
        {
            stateList[i].Update(this);

            if (stateList[i] is IRemovable removable && removable.ShouldRemove)
            {
                RemoveState(stateList[i]);
            }
        }

        ApplyMovementModifiers();

        if (Input.GetKeyDown(KeyCode.Space) && drinkCounts > 0)
        {
            PushState(new EnergyDrinkState());
            drinkCounts -= 1;
        }
    }

    void FixedUpdate()
    {
        for (int i = stateList.Count - 1; i >= 0; i--)
        {
            stateList[i].FixedUpdate(this);
        }
    }

    public void PushState(IPlayerState newState)
    {
        if (newState.IsPenalty() && HasState<EnergyDrinkState>())
        {
            return;
        }

        if (HasState(newState.GetType()))
        {
            return;
        }

        stateList.Add(newState);
        newState.Enter(this);
    }

    public void RemoveState(IPlayerState stateToRemove)
    {
        if (stateList.Contains(stateToRemove))
        {
            stateToRemove.Exit(this);
            stateList.Remove(stateToRemove);
        }
    }

    public T GetState<T>() where T : class, IPlayerState
    {
        foreach (var state in stateList)
        {
            if (state is T match)
                return match;
        }
        return null;
    }

    public bool HasState<T>() where T : class, IPlayerState
    {
        return GetState<T>() != null;
    }

    public bool HasState(System.Type stateType)
    {
        foreach (var state in stateList)
        {
            if (state.GetType() == stateType)
                return true;
        }
        return false;
    }

    public IPlayerState GetTopState()
    {
        return stateList.Count > 0 ? stateList[stateList.Count - 1] : null;
    }

    private void ApplyMovementModifiers()
    {
        float accelFactor = 1f;
        float maxSpeedFactor = 1f;

        foreach (var state in stateList)
        {
            if (state is IMovementModifier mod)
            {
                accelFactor *= mod.GetAccelerationFactor();
                maxSpeedFactor *= mod.GetMaxSpeedFactor();
            }
        }

        MovePlayer(accelFactor, maxSpeedFactor);
    }

    public void MovePlayer(float accelerationFactor = 1f, float maxSpeedFactor = 1f)
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) moveInput = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) moveInput = -1f;

        Vector3 groundVelocityAtPlayerPos = Vector3.zero;
        if (groundRotator != null)
        {
            Vector3 angularVelocity = groundRotator.GetAngularVelocity();
            groundVelocityAtPlayerPos = Vector3.Cross(angularVelocity, playerRb.position);
        }

        Vector3 relativeVelocity = playerRb.velocity - groundVelocityAtPlayerPos;
        float relativeSpeedForward = Vector3.Dot(relativeVelocity, transform.forward);
        Vector3 moveForce = transform.forward * moveInput * acceleration * accelerationFactor;

        bool canAccelerate = (moveInput > 0 && relativeSpeedForward < maxSpeed * maxSpeedFactor)
                          || (moveInput < 0 && relativeSpeedForward > -maxSpeed * maxSpeedFactor);

        if (canAccelerate)
        {
            playerRb.AddForce(moveForce, ForceMode.Acceleration);
        }

        Vector3 horizontalRelativeVelocity = new Vector3(relativeVelocity.x, 0f, relativeVelocity.z);
        float minSpeedForRotation = 0.1f;

        if (horizontalRelativeVelocity.sqrMagnitude > minSpeedForRotation * minSpeedForRotation)
        {
            Vector3 targetDirection = horizontalRelativeVelocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            float step = rotationAlignmentSpeed * Time.fixedDeltaTime;
            Quaternion newRotation = Quaternion.RotateTowards(playerRb.rotation, targetRotation, step);

            Vector3 euler = newRotation.eulerAngles;
            float clampedY = euler.y > 180f ? euler.y - 360f : euler.y;
            clampedY = Mathf.Clamp(clampedY, -90f, 90f);
            euler.y = clampedY < 0f ? clampedY + 360f : clampedY;

            newRotation = Quaternion.Euler(euler);
            playerRb.MoveRotation(newRotation);
        }
    }
}

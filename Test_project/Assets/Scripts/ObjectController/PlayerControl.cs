using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    private Rigidbody playerRb;
    public IPlayerState currentState;

    public GroundRotator groundRotator;

    [Header("Movement")]
    private float acceleration = 12f;
    private float maxSpeed = 40f;

    [Header("Rotation")]
    public float rotationAlignmentSpeed = 180f;

    public int drinkCounts = 1;

    private List<IPlayerState> stateList = new List<IPlayerState>();
    public IReadOnlyList<IPlayerState> States => stateList.AsReadOnly();
    private TMP_Text energyDrinkText;
    public bool isPaused;
    public bool isTutorial = false, isExplain = false;
    private GameObject ui;
    private InGameUIControl uiScript;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        groundRotator = GameObject.FindWithTag("Ground")?.GetComponent<GroundRotator>();
        isPaused = true;
        Time.timeScale = 0f;

        if (groundRotator == null)
        {
            Debug.LogError("No GroundRotator");
            enabled = false;
            return;
        }

        ui = GameObject.FindWithTag("UI");
        uiScript = ui.GetComponent<InGameUIControl>();

        if (ui != null)
        {
            Transform energyDrinkObj = FindChildWithTag(ui.transform, "EnergyDrinkQuantity");
            if (energyDrinkObj != null)
            {
                energyDrinkText = energyDrinkObj.GetComponent<TMP_Text>();
                energyDrinkText.text = drinkCounts.ToString();
            }
            else Debug.LogError("No EnergyDrinkQuantity Text detected or without EnergyDrinkQuantity tag attached");
        }
        else Debug.LogError("No UI or UI without UI tag attached");

        if (uiScript == null) Debug.LogError("No InGameUIControl");

        PushState(new NormalState());
    }

    void Update()
    {

        if (InGameUIControl.isMenuPopped || isExplain) return;
        for (int i = stateList.Count - 1; i >= 0; i--)
        {
            //stateList[i].Update(this);

            if (stateList[i] is IRemovable removable && removable.ShouldRemove)
            {
                RemoveState(stateList[i]);
            }
        }



        if (Input.GetKeyDown(KeyCode.Space) && drinkCounts > 0 && !isPaused)
        {
            if (HasState<EnergyDrinkState>())
            {
                IPlayerState drinkState = GetState<EnergyDrinkState>();
                drinkState.ResetTimer();
                drinkState.ResetTimer();
            }
            else
            {
                PushState(new EnergyDrinkState());
            }
            drinkCounts -= 1;
            energyDrinkText.text = drinkCounts.ToString();
        }

        if (isPaused)
        {
            if (isTutorial || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Time.timeScale = 1f;
                isPaused = false;
                uiScript.ToggleStartTexts(false);
            }
        }

        LimitMaxSpeed();
    }

    // 자식들 중 태그로 찾는 재귀 함수 By ChatGPT
    private Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag(tag))
                return child;
        }
        return null;
    }

    void FixedUpdate()
    {
        if (InGameUIControl.isMenuPopped || isExplain) return;
        for (int i = stateList.Count - 1; i >= 0; i--)
        {
            stateList[i].FixedUpdate(this);
        }

        if (!isPaused)
        {
            ApplyMovementModifiers();
            RotateWithGround();
        }

        
    }

    public bool IsStatePenalty(IPlayerState state) {
        switch (state.TypeOf())
        {
            case Type.FullPenalty:
                return true;
            case Type.OverlapOKPenalty:
                return true;
            default:
                return false;
        }
    }

    public void PushState(IPlayerState newState)
    {
        if (IsStatePenalty(newState) && HasState<EnergyDrinkState>())
        {
            // new state is penalty but we're on energy drink state
            return;
        }
        if ((newState.TypeOf() == Type.FullPenalty) && HasState<CoffeeState>())
        {
            // new state is penalty and we have to turn off coffee state
            IPlayerState coffee = GetState<CoffeeState>();
            Debug.Assert(coffee != null);
            RemoveState(coffee);
        }
        if (newState.TypeOf() == Type.Coffee && IsPenalized())
        {
            // if player is now penalized, ignore coffee
            return;
        }


        if (HasState(newState.GetType()))
        {
            // already has it
            newState.ResetTimer();
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

    public bool IsPenalized()
    {
        foreach (var state in stateList)
        {
            if (state.TypeOf() == Type.FullPenalty)
            {
                // TypeOf 0 and 1 is Penalties(Bed, Gaming, Soju)
                return true;
            }
        }
        return false;
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
                if (groundRotator != null)
                {
                    groundRotator.SetRotationMultiplier(mod.GetAccelerationFactor());
                }
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

        if (moveInput == 0f) return;

        Vector3 moveDirection = Vector3.forward * moveInput;
        Vector3 moveForce = moveDirection * acceleration * accelerationFactor;

        Vector3 groundVelocityAtPlayerPos = Vector3.zero;
        if (groundRotator != null)
        {
            Vector3 angularVelocity = groundRotator.GetAngularVelocity();
            groundVelocityAtPlayerPos = Vector3.Cross(angularVelocity, playerRb.position);
        }

        Vector3 relativeVelocity = playerRb.velocity - groundVelocityAtPlayerPos;
        float relativeSpeedForward = Vector3.Dot(relativeVelocity, Vector3.forward);

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
            if (HasState<SleepingState>()) targetDirection = new Vector3(0, 0, 10000);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            float step = rotationAlignmentSpeed * Time.fixedDeltaTime;
            Quaternion newRotation = Quaternion.RotateTowards(playerRb.rotation, targetRotation, step);

            Vector3 euler = newRotation.eulerAngles;

            newRotation = Quaternion.Euler(euler);
            playerRb.MoveRotation(newRotation);
        }
    }

    private void LimitMaxSpeed()
    {

        float maxSpeedFactor = 1f;

        foreach (var state in stateList)
        {
            if (state is IMovementModifier mod)
            {
                maxSpeedFactor *= mod.GetMaxSpeedFactor();

            }
        }
        Vector3 groundVelocityAtPlayerPos = Vector3.zero;
        if (groundRotator != null)
        {
            Vector3 angularVelocity = groundRotator.GetAngularVelocity();
            groundVelocityAtPlayerPos = Vector3.Cross(angularVelocity, playerRb.position);
        }

        Vector3 relativeVelocity = playerRb.velocity - groundVelocityAtPlayerPos;
        Vector3 flatRelative = new Vector3(relativeVelocity.x, 0, relativeVelocity.z);

        if (flatRelative.magnitude > maxSpeed * maxSpeedFactor)
        {
            Vector3 limitedFlat = flatRelative.normalized * maxSpeed * maxSpeedFactor;
            Vector3 newVelocity = limitedFlat + groundVelocityAtPlayerPos;
            newVelocity.y = playerRb.velocity.y; // preserve vertical velocity
            playerRb.velocity = newVelocity;
        }
    }
    
    private void RotateWithGround()
    {
        if (groundRotator == null) return;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) return;
        if (HasState<SleepingState>()) return;

        float angularSpeed = groundRotator.GetAngularVelocity().z; // rad/s
        float absAngularSpeed = Mathf.Abs(angularSpeed);
        Vector3 groundVelocityAtPlayerPos = Vector3.Cross(groundRotator.GetAngularVelocity(), playerRb.position);
        Vector3 relativeVelocity = playerRb.velocity - groundVelocityAtPlayerPos;

        

        
        //float minAngularSpeedForRotation = 0.05f;

        //if (absAngularSpeed < minAngularSpeedForRotation) return;

        Vector3 targetDirection = (angularSpeed > 0) ? Vector3.left : Vector3.right;

        bool isRotatingInput = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
        if (!isRotatingInput)
        {
            float relSpeed = Vector3.Dot(relativeVelocity, Vector3.forward);
            // no inputs, 상하좌우 아무 입력도 없는 경우임

            if (Mathf.Abs(relSpeed) < 0.2f)
            {
                // 너무 느려서 명확한 방향 판단 불가, 회전 안 함
                return;
            }
            targetDirection = (relSpeed > -0.1f) ? Vector3.forward : Vector3.back;
        }

        
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        float step = rotationAlignmentSpeed * Time.fixedDeltaTime;
        Quaternion newRotation = Quaternion.RotateTowards(playerRb.rotation, targetRotation, step);

        playerRb.MoveRotation(newRotation);
    }

    public void StopMove(){
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        groundRotator.StopMove();
    }

}
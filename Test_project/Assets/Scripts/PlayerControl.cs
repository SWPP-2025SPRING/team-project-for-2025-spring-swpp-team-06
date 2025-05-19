using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    private Rigidbody playerRb;
    public IPlayerState currentState;

    [Header("Movement")]
    public float acceleration = 10f;
    public float maxSpeed = 5f;

    public GroundRotator groundRotator;

    [Header("Rotation")]

    public float rotationAlignmentSpeed = 180f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        if (groundRotator == null)
        {
            GameObject ground = GameObject.FindWithTag("Ground");
            if (ground != null)
            {
                groundRotator = ground.GetComponent<GroundRotator>();
            }

            if (groundRotator == null)
            {
                Debug.LogError("PlayerControl: GroundRotator를 찾을 수 없습니다. Ground 오브젝트에 'GroundRotator' 스크립트가 붙어 있고, 'Ground' 태그가 설정되어 있는지 확인하세요.");
                enabled = false;
                return;
            }
        }

        ChangeState(new NormalState());
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate(this);
        currentState?.HandleMovement(this);
    }

    void Update()
    {
        currentState?.Update(this);

    }
    
    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    public IPlayerState GetCurrentState()
    {
        return currentState;
    }

}

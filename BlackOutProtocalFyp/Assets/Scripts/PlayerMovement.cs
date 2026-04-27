
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Parameters")]
    private CharacterController cc;
    [SerializeField]private float MaxSpeed => SprintInput ? SprintSpeed : WalkSpeed;
    [SerializeField] private float Acceleration = 15f;

    [SerializeField] float WalkSpeed = 3.5f;
    [SerializeField] float SprintSpeed = 8f;

    [Space(15)]
    [Tooltip("This is how high the character can jump.")]
    [SerializeField] float JumpHeight = 2f;

    

    public bool Sprinting
    {
        get
        {
            return SprintInput && CurrentSpeed > 0.1f;
        }
    }

    public Vector2 LookSensitivity = new Vector2(0.1f, 0.1f);

    public float PitchLimit = 85f;

    [SerializeField] float currentPitch = 0f;

    public float CurrentPitch
    {
        get => currentPitch;

        set
        {
            currentPitch = Mathf.Clamp(value, -PitchLimit, PitchLimit);
        }
    }

    [Header("Camera Parameters")]
    [SerializeField] float CameraNormalFOV = 90f;
    [SerializeField] float CameraSprintFOV = 110f;
    [SerializeField] float CameraFOVSmoothing = 1f;

    float TargetCameraFOV
    {
        get
        {
            return Sprinting ? CameraSprintFOV : CameraNormalFOV;
        }
    }

    [Header("PhysicsParameter")]
    [SerializeField] float GravityScale = 3f;

    public float VerticalVelocity = 0f;

    public Vector3 CurrentVelocity;
    public float CurrentSpeed;
    public bool isGrounded => cc.isGrounded;

    [Header("Input")]
    public Vector2 MovementInput;
    public Vector2 LookInput;
    public bool SprintInput;


    [SerializeField] Camera PlayerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        MovePlayer();
        if (Time.timeScale == 1)
        {
            Look();
        }
        CameraUpdate();
    }

    public void TryJump()
    {
        if (isGrounded == false)
        {
            return;
        }

        VerticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y * GravityScale);
    }

    public void MovePlayer()
    {
        Vector3 motion = transform.forward * MovementInput.y + transform.right * MovementInput.x;
        motion.y = 0f;
        motion.Normalize();

        if (motion.sqrMagnitude >= 0.01f)
        {
            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, motion * MaxSpeed, Acceleration * Time.deltaTime);
        }
        else
        {
            CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, Vector3.zero, Acceleration * Time.deltaTime);
        }

         if (isGrounded && VerticalVelocity <= 0.01f)
        {
            VerticalVelocity = -3f;
        }
        else
        {
            VerticalVelocity += Physics.gravity.y * GravityScale * Time.deltaTime;
        }

            Vector3 fullVelocity = new Vector3(CurrentVelocity.x, VerticalVelocity, CurrentVelocity.z);

        cc.Move(fullVelocity * Time.deltaTime);

        CurrentSpeed = CurrentVelocity.magnitude;

    }

    public void Look()
    {
        Vector2 input = new Vector2(LookInput.x * LookSensitivity.x, LookInput.y * LookSensitivity.y);

        CurrentPitch -= input.y;

        PlayerCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

        transform.Rotate(Vector3.up * input.x);
    }

    public void CameraUpdate()
    {

        float targetFOV = CameraNormalFOV;

        if (Sprinting)
        {
            float speedRatio = CurrentSpeed / SprintSpeed;

            targetFOV = Mathf.Lerp(CameraNormalFOV, CameraSprintFOV, speedRatio);
        }


        PlayerCamera.fieldOfView = Mathf.Lerp(PlayerCamera.fieldOfView, targetFOV, CameraFOVSmoothing *  Time.deltaTime);
    }

    void OnMove(InputValue value)
    {
        MovementInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        SprintInput = value.isPressed;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            TryJump();
        }
    }
}

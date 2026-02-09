using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float Acceleration;
    

    public Vector3 CurrentVelocity;
    public float CurrentSpeed;

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

    public Vector2 MovementInput;
    public Vector2 LookInput;


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

        float VerticalVelocity = Physics.gravity.y * 20.0f * Time.deltaTime;

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

    void OnMove(InputValue value)
    {
        MovementInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
    }
}

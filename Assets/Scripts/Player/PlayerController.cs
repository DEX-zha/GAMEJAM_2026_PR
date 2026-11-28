using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputActions = global::PlayerInput;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float lookSensitivity = 150f;
    [SerializeField] private float maxLookAngle = 80f;

    private Rigidbody _rb;
    private PlayerInputActions _input;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _jumpPressed;
    private float _currentSpeed;
    private float _yaw;
    private float _pitch;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        _yaw = transform.eulerAngles.y;

        _input = new PlayerInputActions();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;
        _input.Player.LookAround.performed += OnLookAround;
        _input.Player.LookAround.canceled += OnLookAround;
        _input.Player.Jump.performed += OnJump;
        _input.Player.Run.performed += OnRun;
        _input.Player.Run.canceled += OnRun;

        _currentSpeed = moveSpeed;
    }

    private void OnEnable() => _input.Player.Enable();
    private void OnDisable() => _input.Player.Disable();

    private void OnDestroy()
    {
        if (_input == null)
            return;

        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMove;
        _input.Player.LookAround.performed -= OnLookAround;
        _input.Player.LookAround.canceled -= OnLookAround;
        _input.Player.Jump.performed -= OnJump;
        _input.Player.Run.performed -= OnRun;
        _input.Player.Run.canceled -= OnRun;
        _input.Dispose();
    }

    private void Update()
    {
        if (_lookInput.sqrMagnitude < 0.01f)
            return;

        _yaw += _lookInput.x * lookSensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch - _lookInput.y * lookSensitivity * Time.deltaTime, -maxLookAngle, maxLookAngle);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void FixedUpdate()
    {
        Quaternion yawRotation = Quaternion.Euler(0f, _yaw, 0f);
        _rb.MoveRotation(yawRotation);

        Vector3 moveDir = yawRotation * new Vector3(_moveInput.x, 0f, _moveInput.y);
        Vector3 velocity = _rb.linearVelocity;

        velocity.x = moveDir.x * _currentSpeed;
        velocity.z = moveDir.z * _currentSpeed;

        bool grounded = IsGrounded();
        if (_jumpPressed && grounded && velocity.y <= 0.1f)
            velocity.y = jumpForce;
        else if (grounded && velocity.y < 0f)
            velocity.y = 0f;
        else
            velocity.y -= gravity * Time.fixedDeltaTime;

        _jumpPressed = false;
        _rb.linearVelocity = velocity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 raw = context.ReadValue<Vector2>();
        _moveInput = raw.magnitude >= 0.15f ? Vector2.ClampMagnitude(raw, 1f) : Vector2.zero;
    }

    public void OnLookAround(InputAction.CallbackContext context)
    {
        if (context.control?.device is not Gamepad)
            return;

        Vector2 raw = context.ReadValue<Vector2>();
        _lookInput = raw.magnitude >= 0.2f ? raw : Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _jumpPressed = true;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _currentSpeed = runSpeed;
        }
        else if (context.canceled)
        {
            _currentSpeed = moveSpeed;
        }
    }
}

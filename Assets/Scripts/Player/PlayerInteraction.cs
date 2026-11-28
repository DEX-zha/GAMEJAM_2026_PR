using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInputActions = global::PlayerInput;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float interactDistance = 4f;

    private PlayerInputActions _input;
    private IInteractable _currentHovered;
    private IInteractable _heldItem;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (holdPoint == null)
            holdPoint = cameraTransform;

        _input = new PlayerInputActions();
        _input.Player.Grab.performed += OnGrab;
        _input.Player.Grab.canceled += OnGrab;
        _input.Player.Interact.performed += OnInteract;
    }

    private void OnEnable()
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
        DropHeldItem();
        ClearHover();
    }

    private void OnDestroy()
    {
        if (_input == null)
            return;

        _input.Player.Grab.performed -= OnGrab;
        _input.Player.Grab.canceled -= OnGrab;
        _input.Player.Interact.performed -= OnInteract;
        _input.Dispose();
    }

    private void Update()
    {
        if (_heldItem != null)
            return;

        IInteractable target = FindInteractable();
        if (target == _currentHovered)
            return;

        ClearHover();
        _currentHovered = target;
        _currentHovered?.OnHover();
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_heldItem == null && _currentHovered != null && _currentHovered.CanGrab())
            {
                _heldItem = _currentHovered;
                ClearHover();
                _heldItem.OnGrab(holdPoint);
            }
        }
        else if (context.canceled)
        {
            DropHeldItem();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (_heldItem != null)
        {
            if (_heldItem.CanInteract())
                _heldItem.OnInteract(true);
            return;
        }

        if (_currentHovered != null && _currentHovered.CanInteract())
            _currentHovered.OnInteract(false);
    }

    private IInteractable FindInteractable()
    {
        if (cameraTransform == null)
            return null;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            return hit.collider.GetComponentInParent<IInteractable>();

        return null;
    }

    private void DropHeldItem()
    {
        if (_heldItem == null)
            return;

        _heldItem.OnDrop();
        _heldItem = null;
    }

    private void ClearHover()
    {
        if (_currentHovered != null)
            _currentHovered.OnHoverExit();

        _currentHovered = null;
    }
}

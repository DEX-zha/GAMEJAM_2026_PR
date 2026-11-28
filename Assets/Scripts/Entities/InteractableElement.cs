using UnityEngine;

public class InteractableElement : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _hoverMaterial;
    [SerializeField] private Material _baseMaterial;
    [SerializeField] private Material _interactMaterial;

    [SerializeField] private bool isGrabbable = true;
    [SerializeField] private bool isInteractable = true;

    private Renderer _renderer;
    private Rigidbody _rb;
    private Collider _collider;
    private bool _isGrabbed;
    private bool _isInteracted;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        if (_baseMaterial == null && _renderer != null)
            _baseMaterial = _renderer.sharedMaterial;
    }

    public bool CanGrab()
    {
        return isGrabbable;
    }

    public bool CanInteract()
    {
        return isInteractable;
    }

    public void OnHover()
    {
        if (!_isGrabbed)
            SetMaterial(_hoverMaterial);
    }

    public void OnHoverExit()
    {
        if (!_isGrabbed)
            SetMaterial(_isInteracted && _interactMaterial != null ? _interactMaterial : _baseMaterial);
    }

    public void OnInteract(bool isGrabbed)
    {
        _isInteracted = !_isInteracted;
        SetMaterial(_isInteracted && _interactMaterial != null ? _interactMaterial : _baseMaterial);
    }

    private void SetMaterial(Material material)
    {
        if (_renderer != null && material != null)
            _renderer.sharedMaterial = material;
    }

    public void OnGrab(Transform holdPoint)
    {
        _isGrabbed = true;
        SetMaterial(_isInteracted && _interactMaterial != null ? _interactMaterial : _baseMaterial);

        if (_rb != null)
            _rb.isKinematic = true;

        if (_collider != null)
            _collider.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDrop()
    {
        _isGrabbed = false;
        transform.SetParent(null);

        if (_collider != null)
            _collider.enabled = true;

        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    public void OnThrow(Vector3 force)
    {
        OnDrop();

        if (_rb != null)
            _rb.AddForce(force, ForceMode.Impulse);
    }
}

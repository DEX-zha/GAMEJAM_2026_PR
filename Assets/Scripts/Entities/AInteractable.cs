using UnityEngine;

public abstract class AInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected Material _hoverMaterial;
    [SerializeField] protected Material _baseMaterial;
    [SerializeField] protected Material _interactMaterial;

    [SerializeField] protected bool isGrabbable = true;
    [SerializeField] protected bool isInteractable = true;

    protected Renderer _renderer;
    protected Rigidbody _rb;
    protected Collider _collider;
    protected bool _isGrabbed;
    protected bool _isInteracted;

    protected virtual void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        if (_baseMaterial == null && _renderer != null)
            _baseMaterial = _renderer.sharedMaterial;
    }

    public virtual bool CanGrab()
    {
        return isGrabbable;
    }

    public virtual bool CanInteract()
    {
        return isInteractable;
    }

    public virtual void OnHover()
    {
        if (!_isGrabbed)
            SetMaterial(_hoverMaterial);
    }

    public virtual void OnHoverExit()
    {
        if (!_isGrabbed)
            SetMaterial(_isInteracted && _interactMaterial != null ? _interactMaterial : _baseMaterial);
    }

    public virtual void OnInteract(bool isGrabbed)
    {
        _isInteracted = !_isInteracted;
        SetMaterial(_isInteracted && _interactMaterial != null ? _interactMaterial : _baseMaterial);
    }

    protected virtual void SetMaterial(Material material)
    {
        if (_renderer != null && material != null)
            _renderer.sharedMaterial = material;
    }

    public virtual void OnGrab(Transform holdPoint)
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

    public virtual void OnDrop()
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

    public virtual void OnThrow(Vector3 force)
    {
        OnDrop();

        if (_rb != null)
            _rb.AddForce(force, ForceMode.Impulse);
    }
}

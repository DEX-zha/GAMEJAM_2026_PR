using UnityEngine;

public class InteractableElement : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _hoverMaterial;
    [SerializeField] private Material _baseMaterial;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();

        if (_baseMaterial == null && _renderer != null)
            _baseMaterial = _renderer.sharedMaterial;
    }

    public void OnHover()
    {
        SetMaterial(_hoverMaterial);
    }

    public void OnHoverExit()
    {
        SetMaterial(_baseMaterial);
    }

    private void SetMaterial(Material material)
    {
        if (_renderer != null && material != null)
            _renderer.sharedMaterial = material;
    }

    public void OnGrab(Transform holdPoint)
    {
    }

    public void OnThrow(Vector3 force)
    {
    }
}

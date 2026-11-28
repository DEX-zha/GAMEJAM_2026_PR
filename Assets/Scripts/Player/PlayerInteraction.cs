using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float interactDistance = 4f;

    private IInteractable _currentHovered;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void OnDisable()
    {
        ClearHover();
    }

    private void Update()
    {
        IInteractable target = FindInteractable();
        if (target == _currentHovered)
            return;

        ClearHover();
        _currentHovered = target;
        _currentHovered?.OnHover();
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

    private void ClearHover()
    {
        if (_currentHovered != null)
            _currentHovered.OnHoverExit();

        _currentHovered = null;
    }
}

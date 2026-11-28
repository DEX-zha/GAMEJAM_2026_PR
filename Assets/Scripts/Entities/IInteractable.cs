using UnityEngine;

public interface IInteractable
{
    bool CanGrab();
    bool CanInteract();

    void OnHover();
    void OnHoverExit();
    void OnGrab(Transform holdPoint);
    void OnDrop();
    void OnInteract(bool isGrabbed);
    void OnThrow(Vector3 force);
}

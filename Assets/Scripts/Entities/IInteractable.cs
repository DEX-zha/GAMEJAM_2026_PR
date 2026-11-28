using UnityEngine;

public interface IInteractable
{
    void OnHover();
    void OnHoverExit();
    void OnGrab(Transform holdPoint);
    void OnThrow(Vector3 force);
}

using UnityEngine;

public interface IInteractable
{
    public void OnHover();
    public void OnGrab(Transform holdPoint);
    public void OnThrow(Vector3 force);
}
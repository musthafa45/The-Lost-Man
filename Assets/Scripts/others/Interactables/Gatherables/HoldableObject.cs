using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private HoldableObjectSO holdableObjectSO;
    public void Interact(Transform interactorTransform)
    {
        ObjectHolder objectHolder = FindObjectOfType<ObjectHolder>();

        if (objectHolder != null)
        {
            objectHolder.SetParent(this);
        }
        else
        {
            Debug.LogError("No Object Holder Script Found");
        }
    }

    public HoldableObjectSO GetHoldableObjectSO()
    {
        return holdableObjectSO;
    }
}

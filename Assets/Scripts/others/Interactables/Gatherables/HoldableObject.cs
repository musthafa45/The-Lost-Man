using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private HoldableObjectSO holdableObjectSO;
    [SerializeField] private Transform objectToFollowTr;
    private bool isThrowedByPlayer = false;
    private bool isPlacedInTruck = false;

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

    public bool IsThrowedByPlayer()
    {
        return isThrowedByPlayer;
    }
    public bool IsPlacedInTruck()
    {
        return isPlacedInTruck;
    }
    public void SetIsThrowedByPlayer(bool isThrowedByPlayer)
    {
        this.isThrowedByPlayer = isThrowedByPlayer;
    }
    public void SetIsPlacedInTruck(bool isPlacedInTruck)
    {
        this.isPlacedInTruck = isPlacedInTruck;
    }

    internal Transform GetObjectToFollowTransform()
    {
        return objectToFollowTr;
    }
}

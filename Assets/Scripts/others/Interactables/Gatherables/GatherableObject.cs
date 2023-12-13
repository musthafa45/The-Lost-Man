using DG.Tweening;
using ProjectMiamiTestInventory;
using System;
using TMPro;
using UnityEngine;

public class GatherableObject : MonoBehaviour,IInteractable
{
    [SerializeField] private GatherableSO gatherableSO;                  // Gatherable object data

    public void Interact(Transform interactorTransform)
    {
        bool isInventoryAccepted = EventManager.Instance.InvokeTryPickupedItem(gatherableSO);

        if (isInventoryAccepted) Destroy(this.gameObject);
    }

    public GatherableSO GetGatherableSO()
    {
        return gatherableSO;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out FirstPersonController player))
        {
            if (TryGetComponent<Rigidbody>(out var rb)) Destroy(rb);
            if (TryGetComponent<Collider>(out var collider)) collider.isTrigger = true;
        }
    }
}

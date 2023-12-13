using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action<GatherableSO> OnSlotItemButtonPerformed;

    public event Predicate<GatherableSO> OnPlayerPickupedItem;
    public event Predicate<GatherableSO> OnPlayerTryOpenDoor;

    public event EventHandler OnSelectedItemChanged;

    public event Action<GatherableSO> OnHealableItemUsed;

    public event EventHandler OnInventoryItemsModified;

    public event EventHandler OnInventoryOpened;
    public event EventHandler OninventoryClosed;

    public event Action<GatherableSO> OnEquipableItemEquipped;

    public event EventHandler OnStaminaFinished;
    public event EventHandler OnStaminaTopUpped;

    
    private void Awake()
    {
        Instance = this;
    }

    public bool InvokeTryPickupedItem(GatherableSO pickupItem)
    {
        return OnPlayerPickupedItem?.Invoke(pickupItem) == true;
    }

    public bool InvokeTryOpenDoor(GatherableSO validKeySO)
    {
        return OnPlayerTryOpenDoor?.Invoke(validKeySO) == true;
    }

    public void InvokeSelectedItemChanged()
    {
        OnSelectedItemChanged?.Invoke(this,EventArgs.Empty);
    }

    public void InvokeUseItemHealable(GatherableSO currentSelectedItem)
    {
        OnHealableItemUsed?.Invoke(currentSelectedItem);
    }

    public void InvokeInventoryItemsModified()
    {
        OnInventoryItemsModified?.Invoke(this,EventArgs.Empty);
    }

    public void InvokeInventoryOpened()
    {
        OnInventoryOpened?.Invoke(this,EventArgs.Empty);
    }

    public void InvokeInventoryClosed()
    {
        OninventoryClosed?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeEquipItemEquipable(GatherableSO currentSelectedItem)
    {
        OnEquipableItemEquipped?.Invoke(currentSelectedItem);
    }

    public void InvokeOnSlotItemBtnPerformed(GatherableSO item)
    {
        OnSlotItemButtonPerformed?.Invoke(item);
    }

    public void InvokeStaminaFinished()
    {
        OnStaminaFinished?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeStaminaTopUpped()
    {
        OnStaminaTopUpped?.Invoke(this, EventArgs.Empty);
    }

   
}

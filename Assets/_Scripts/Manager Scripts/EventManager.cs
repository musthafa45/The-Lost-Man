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

    public event Action<Transform, TruckDoor> OnPlayerTryGetInTruck;

    public event EventHandler OnPlayerGetsInTruck;
    public event EventHandler OnPlayerGetsOutTruck;

    public event EventHandler<OnPlayerThrowedSpearArgs> OnPlayerThrowedSpear;
    public class OnPlayerThrowedSpearArgs : EventArgs
    {
        public Vector3 throwedPosition;
        public float impactRadius;
    }

    public event EventHandler OnPlayerOpensDoor;
    public event EventHandler OnPlayerCloseDoor;

    public event Action<EquipItemSlot, GatherableSO> OnEquipSlotModified;

    public static event EventHandler OnAnyOutHousePlayerEntered;
    public static event EventHandler OnAnyOutHousePlayerExited;

    public static event Action<float> OnAnyGetMicSoundData;
    
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
        OnSelectedItemChanged?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeUseItemHealable(GatherableSO currentSelectedItem)
    {
        OnHealableItemUsed?.Invoke(currentSelectedItem);
    }

    public void InvokeInventoryItemsModified()
    {
        OnInventoryItemsModified?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeInventoryOpened()
    {
        OnInventoryOpened?.Invoke(this, EventArgs.Empty);
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

    public void InvokePlayerTryGetInTruck(Transform playerTransform, TruckDoor truckDoor)
    {
        OnPlayerTryGetInTruck?.Invoke(playerTransform, truckDoor);
    }
    public void InvokePlayerGetsInTruck()
    {
        OnPlayerGetsInTruck?.Invoke(this, EventArgs.Empty);
    }

    public void InvokePlayerGetsOutTruck()
    {
        OnPlayerGetsOutTruck?.Invoke(this, EventArgs.Empty);
    }
    public void InvokeSpearThrowedTowardsFish(Vector3 throwedPosition, float impactRadius = 10)
    {
        OnPlayerThrowedSpear?.Invoke(this, new OnPlayerThrowedSpearArgs
        {
            throwedPosition = throwedPosition,
            impactRadius = impactRadius
        });
    }
    public void InvokeOnDoorOpen()
    {
        OnPlayerOpensDoor?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeOnDoorClosed()
    {
        OnPlayerCloseDoor?.Invoke(this, EventArgs.Empty);
    }

    public void InvokePlayerEnteredAnyOutHouse()
    {
        OnAnyOutHousePlayerEntered?.Invoke(this, EventArgs.Empty);
    }

    public void InvokePlayerExitedAnyOutHouse()
    {
        OnAnyOutHousePlayerExited?.Invoke(this, EventArgs.Empty);
    }

    public void InvokeGetAnyMicOutPutData(float micSoundData)
    {
        OnAnyGetMicSoundData?.Invoke(micSoundData);
    }

    public void InvokeOnEquipSlotModified(EquipItemSlot equipItemSlot,GatherableSO gatherableSO)
    {
        OnEquipSlotModified?.Invoke(equipItemSlot,gatherableSO);
    }
}
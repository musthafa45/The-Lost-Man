using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private int inventoryItemLimit = 8;

    private List<GatherableSO> inventoryItems;

    private DropObjectSensor dropObjectSensor;
    private void Awake()
    {
        Instance = this;
        dropObjectSensor = FindObjectOfType<DropObjectSensor>();
        inventoryItems = new List<GatherableSO>(inventoryItemLimit);

        if(dropObjectSensor != null)
        dropObjectSensor.enabled = false;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnPlayerPickupedItem += EventManager_Instance_OnPlayerPickupedItem;
        EventManager.Instance.OnPlayerTryOpenDoor += EventManager_Instance_OnPlayerTryOpenDoor;
        EventManager.Instance.OnInventoryOpened += EventManager_Instance_OnInventoryOpened;
        EventManager.Instance.OninventoryClosed += EventManager_Instance_OninventoryClosed;
    }

    private bool EventManager_Instance_OnPlayerTryOpenDoor(GatherableSO validKey)
    {
        if(inventoryItems.Contains(validKey))
        {
            inventoryItems.Remove(validKey);
            EventManager.Instance.InvokeInventoryItemsModified();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void EventManager_Instance_OnInventoryOpened(object sender, EventArgs e)
    {
        dropObjectSensor.enabled = true;
    }

    private void EventManager_Instance_OninventoryClosed(object sender, EventArgs e)
    {
        dropObjectSensor.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnPlayerPickupedItem -= EventManager_Instance_OnPlayerPickupedItem;
        EventManager.Instance.OnPlayerTryOpenDoor -= EventManager_Instance_OnPlayerTryOpenDoor;
        EventManager.Instance.OnInventoryOpened -= EventManager_Instance_OnInventoryOpened;
        EventManager.Instance.OninventoryClosed -= EventManager_Instance_OninventoryClosed;
    }
    private bool EventManager_Instance_OnPlayerPickupedItem(GatherableSO obj) //Actually it Usses predicate
    {
        GatherableSO pickuppedItemSO = obj;

        if (inventoryItems.Count < inventoryItemLimit)
        {
            inventoryItems.Add(pickuppedItemSO);

            if (InventoryUIManager.Instance != null)
            {
                InventoryUIManager.Instance.UpdateUI(pickuppedItemSO);
            }
            else
            {
                Debug.LogWarning("Inventory Ui Manager Disabled Cannot Show Gathered Objects");
            }
           
            return true;
        }
        else
        {
            Debug.LogWarning("No More Space To Add Item To Inventory");
            return false;
        }
    }

    public void UseItem(GatherableSO CurrentSelectedItem)
    {
        if (CurrentSelectedItem != null)
        {
            switch (CurrentSelectedItem.gatherableType)
            {
                case GatherableObjectType.Healable:
                    EventManager.Instance.InvokeUseItemHealable(CurrentSelectedItem);
                    inventoryItems.Remove(CurrentSelectedItem);
                    Debug.Log("Healable Used Name of :" + CurrentSelectedItem.gatherableObjectName);
                    EventManager.Instance.InvokeInventoryItemsModified();
                    break;
                case GatherableObjectType.Collectable:
                    //Search Like Doors Somthing
                    break;
                case GatherableObjectType.Usable:
                    // Kind A like Battery
                    break;

            }
        }
    }

    public List<GatherableSO> GetInventoryItems() => inventoryItems;

    public void EquipItem(GatherableSO currentSelectedItem, Action<GatherableSO> OnSuccessfullyEquippedItem)
    {
        EventManager.Instance.InvokeEquipItemEquipable(currentSelectedItem);

        inventoryItems.Remove(currentSelectedItem);

        EventManager.Instance.InvokeInventoryItemsModified();

        OnSuccessfullyEquippedItem?.Invoke(currentSelectedItem);
    }

    public void DropUsableItem(GatherableSO currentSelectedItem)
    {
        DropCurrentSelectedItem(currentSelectedItem);
    }

    public void DropEquipableItem(GatherableSO currentSelectedItem)
    {
        DropCurrentSelectedItem(currentSelectedItem);
    }

    private void DropCurrentSelectedItem(GatherableSO currentSelectedItem)
    {
        var obj = Instantiate(currentSelectedItem.itemSetUppedPrefab, dropObjectSensor.GetRandomDropPoint(), Quaternion.identity);

        if (obj.TryGetComponent<Collider>(out var collider)) collider.isTrigger = false;
        var rb = obj.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        inventoryItems.Remove(currentSelectedItem);

        EventManager.Instance.InvokeInventoryItemsModified();
    }

}

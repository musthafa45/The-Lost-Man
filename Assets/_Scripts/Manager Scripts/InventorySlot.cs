using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    protected GatherableSO item;
    protected GameObject itemPrefab = null;

    public static event Action OnSlotItemModified;
    public bool IsEmpty() => item == null;
    
    public void SetItem(GatherableSO pickuppedItemSO)
    {
       this.item = pickuppedItemSO;

       UpdateVisual();
    }

    public void ClearItem()
    {
        item = null;
        UpdateVisual();
    }

    private void Start()
    {
        OnSlotItemModified += () => 
        { 
            RefreshSlotData(); 
        };
    }

    protected virtual void RefreshSlotData()
    {
        var draggableItem = GetComponentInChildren<DraggableItem>();
        if(draggableItem == null)
        {
            item = null;
            ItemInfoHandlerUI.Instance.ClearInfoObject();
        }
    }

    protected virtual void UpdateVisual()
    {
        var draggableItem = GetComponentInChildren<DraggableItem>();

        if(draggableItem == null && item != null)
        {
            if (itemPrefab != null)
            {
                Destroy(itemPrefab);
            }

           itemPrefab = Instantiate(Prefabs.Instance.GetInventorySlotItemTemplate(),transform);

           if(itemPrefab.TryGetComponent(out DraggableItem draggable))
           {
               draggable.SetGatherableObjSO(item);
               draggable.UpdateSlotImage();
           }
        }
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            GameObject droppedObj = eventData.pointerDrag;
            if (droppedObj.TryGetComponent(out DraggableItem droppedItem))
            {
                SetItem(droppedItem.GetGatherableSO());
                UpdateVisual();

                OnSlotItemModified?.Invoke();
                Destroy(droppedObj);
            }
        }
       
    }

    public GatherableSO GetGatherableObjSO()
    {
        return item;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var draggableItem = GetComponentInChildren<DraggableItem>();
        if (item != null && draggableItem != null)
        {
            ItemInfoHandlerUI.Instance.SetItemAndShow(GetComponentInChildren<DraggableItem>().GetGatherableSO(), transform.position);
        }
        else
        {
            Debug.LogWarning("There Is No Item To Show Info In this Slot");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemInfoHandlerUI.Instance.ClearInfoObject();
    }
}

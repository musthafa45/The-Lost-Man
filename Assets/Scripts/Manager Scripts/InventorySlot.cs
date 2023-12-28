using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,IDropHandler
{
    private GatherableSO item;
    [SerializeField] private Image itemIconImage;

    public void OnSelect(BaseEventData eventData)
    {
        EventManager.Instance.InvokeSelectedItemChanged();
        
        InventoryUIManager.Instance.OnItemSelected(item); // Need this For Update Ui Description

        UpdateVisual();
    }

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

    private void Awake()
    {
        //bgImage = GetComponent<Image>();
        //normalColor = bgImage.color;

        //slotItemButton.onClick.AddListener(() =>
        //{
        //    if(item != null)
        //    {
        //        Debug.Log("Input From : " + item.gatherableObjectName +" Slot");
        //        EventManager.Instance.InvokeOnSlotItemBtnPerformed(item);
        //    }
        //    else
        //    {
        //        Debug.Log("Input From : Empty Slot");
        //        EventManager.Instance.InvokeOnSlotItemBtnPerformed(null);
        //    }

        //});

        //UpdateVisual();

        //Debug.Log("Item Slot Debug listners Count" + " " + slotItemButton.onClick.GetPersistentEventCount());
    }
   
    private void UpdateVisual()
    {
        if (item != null)
        {
            itemIconImage.enabled = true;
            itemIconImage.sprite = item.gatherableImageSprite;
        }
        else
        {
            itemIconImage.enabled = false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if(droppedObj.TryGetComponent(out DraggableItem droppedItem))
        {
            var currentSlotItem = GetComponentInChildren<DraggableItem>();

            if (currentSlotItem != null && currentSlotItem.GetGatherableSO() == null)
            {
                Destroy(currentSlotItem.gameObject);
            }

            droppedItem.SetParent(this.transform);
            item = droppedItem.GetGatherableSO();
            UpdateVisual();

        }
    }

    public GatherableSO GetGatherableObjSO()
    {
        return item;
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    itemIconImage.raycastTarget = false;
    //    oldParentTransform = transform.parent;
    //    transform.SetParent(transform.root);
    //    transform.SetAsLastSibling();
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //   transform.position = Input.mousePosition;
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    itemIconImage.raycastTarget = true;
    //    transform.SetParent(oldParentTransform);
    //}

}

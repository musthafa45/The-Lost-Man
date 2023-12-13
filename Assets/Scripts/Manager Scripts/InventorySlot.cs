using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, ISelectHandler
{
    private GatherableSO item;
    private Button slotItemButton;


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
        slotItemButton = GetComponent<Button>();

        slotItemButton.onClick.AddListener(() =>
        {
            if(item != null)
            {
                Debug.Log("Input From : " + item.gatherableObjectName +" Slot");
                EventManager.Instance.InvokeOnSlotItemBtnPerformed(item);
            }
            else
            {
                Debug.Log("Input From : Empty Slot");
                EventManager.Instance.InvokeOnSlotItemBtnPerformed(null);
            }

        });

        UpdateVisual();

        //Debug.Log("Item Slot Debug listners Count" + " " + slotItemButton.onClick.GetPersistentEventCount());
    }
   
    //private void OnDisable()
    //{
    //    slotItemButton.onClick.RemoveAllListeners();
    //}

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

}

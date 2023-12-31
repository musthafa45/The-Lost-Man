using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelectorSlot : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        EventManager.Instance.InvokeSelectedItemChanged();

        InventoryUIManager.Instance.OnItemSelected(null);
    }
}

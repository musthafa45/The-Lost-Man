using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemSlot : MonoBehaviour
{
    private GatherableSO item;

    [SerializeField] private Image itemIconImage;

    public void SetItem(GatherableSO pickuppedItemSO)
    {
        this.item = pickuppedItemSO;

        UpdateVisual();
    }
    private void Awake()
    {
        UpdateVisual();
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

    public void ClearItem()
    {
        item = null;
        UpdateVisual();
    }

    public bool IsEmpty() => item == null;
}

using ProjectMiamiTestInventory;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIconTemplate : MonoBehaviour
{
    public static Action<GatherableSO> OnAnyObjectUsedAndRemoved;

    [SerializeField] private Button useBtn, removeBtn;
    [SerializeField] private TextMeshProUGUI itemQuantitytext;
    private string itemName;
    [SerializeField] private Image itemIconImage;

    private GatherableSO gatherableSO;
    private void Awake()
    {
        useBtn.onClick.AddListener(() =>
        {
            UseItem();
        });

        removeBtn.onClick.AddListener(() =>
        {
            InventoryTest.Instance.RemoveObjectFromInventory(gatherableSO);
            Destroy(this.gameObject);
        });

    }

    private void CheckObjectStoringType()
    {
        if(gatherableSO.storingType == StoringType.Removable)  // Dont Remove, RemoveIcon Btn.
        {

        }
        else if(gatherableSO.storingType == StoringType.NonRemovable) // remove Remove Item Btn.
        {
            removeBtn.gameObject.SetActive(false);
            useBtn.gameObject.SetActive(false);
        }
    }

    public void SetUpItemTemplatePropsUI(GatherableSO gatherableSO,int itemQuantity)
    {
        this.gatherableSO = gatherableSO; 
        itemName = gatherableSO.gatherableObjectName;
        itemIconImage.sprite = gatherableSO.gatherableImageSprite;
        if(itemQuantity > 1) // Object not multiple time Gatherable 
        {
            itemQuantitytext.text = itemQuantity.ToString();
        }
        else
        {
            // Disable item Count Text
            itemQuantitytext.gameObject.SetActive(false);
        }

        CheckObjectStoringType();
    }

    public void UseItem()
    {
        if(TryUseItem(gatherableSO))
        {
            Debug.Log("item" + itemName + " " + " Used");
            OnAnyObjectUsedAndRemoved?.Invoke(gatherableSO);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("item" + itemName + " " + " Not Selected Any type Or Different item");
        }
        
    }

    private bool TryUseItem(GatherableSO gatherableSO)
    {
        switch (gatherableSO.gatherableType)
        {
            case GatherableObjectType.Healable:
                // Assuming you have a HealthSystem component on the player
                PlayerStaminaSystem playerStamina = FindObjectOfType<PlayerStaminaSystem>();
                playerStamina.AddStamina(gatherableSO.value);
                break;
            case GatherableObjectType.Usable:
                // Implement logic for Usable items here
                break;
            case GatherableObjectType.Collectable:
                // Implement logic for Collectable items here
                break;
            case GatherableObjectType.Equipable:
                // Implement logic for Equipable items here
                break;
            default:
                // Handle the case where the item type is not recognized
                return false;
        }

        // Return true to indicate that the item was successfully used
        return true;
    }

    //private void OnDisable()
    //{
    //    useBtn.onClick.RemoveAllListeners();
    //    removeBtn.onClick.RemoveAllListeners();
    //}

}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance { get; private set; }

    private enum MenuStates
    {
        SlotMenuFocused,
        CommandMenuFocused,
        ObjectDetailViewFocused
    }
    private MenuStates menuStates;

    private GatherableSO currentSelectedItem;

    private bool isInventoryUiOpened = false;
    private bool isCommandMenuOpened = false;

    [SerializeField] private Transform inventoryUiMainVisual;
    [SerializeField] private Transform commandMenuUsable,commandMenuEquipable;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI playerCashAmount;

    [SerializeField] private List<InventorySlot> itemSlots;
    [SerializeField] private List<EquipItemSlot> equippedItemSlots;
    [SerializeField] private List<Button> ignoreButtonsWhileOpenCommandMenu;

    [SerializeField] private Button useItemButton, equipItemButton;
    [SerializeField] private Button dropUsableItemButton, dropEquipableItemButton;
    [SerializeField] private Button checkUsableItemButton, checkEquipableItemButton;

    [SerializeField] private Button exitInventoryButton;
    [SerializeField] private Button objectDetailViewExitButton;

    private GameObject lastSelectedobj;

    private Transform itemVisualPrefab;

    public Transform GetGridTransform() => inventoryUiMainVisual;
    public GatherableSO GetCurrentSelectedItem() => currentSelectedItem;

    public void OnItemSelected(GatherableSO item) // For Updating Ui description Purpose
    {
        SeCurrentGetherableSO(item);
        UpdateDescriptionUI(item);
    }

    private void UpdateDescriptionUI(GatherableSO item)
    {
        if(item != null)
        {
            itemName.text = item.gatherableObjectName;
            itemDescription.text = item.description[0];
        }
        else
        {
            ClearItemDescription();
        }
    }

    private void SeCurrentGetherableSO(GatherableSO item)
    {
        currentSelectedItem = item;
    }

    private void ClearItemDescription()
    {
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
    }

    private void UpdateMenuStatesOnce()
    {
        switch(menuStates)
        {
            case MenuStates.SlotMenuFocused:
                DestroyDetailedViewObject();
                DisableCommandMenues();
                SetInteractionToIgnoreBtns(true);
                SetSelectedObject(itemSlots[0].gameObject);

                break;
            case MenuStates.CommandMenuFocused:
                DestroyDetailedViewObject();
                SetInteractionToIgnoreBtns(true);
                OpenCommandMenuUI(currentSelectedItem);
                objectDetailViewExitButton.gameObject.SetActive(false);

                break;
            case MenuStates.ObjectDetailViewFocused:
                DisableCommandMenues();
                SetInteractionToIgnoreBtns(false);
                objectDetailViewExitButton.gameObject.SetActive(true);
                SpawnCheckingItem(currentSelectedItem);
                SetSelectedObject(objectDetailViewExitButton.gameObject);

                break;
        }
    }
    private void SetMenuState(MenuStates state)
    {
        this.menuStates = state;
    }

    public void UpdateUI(GatherableSO pickuppedItemSO)
    {
        bool addedToInventory = false;

        foreach (InventorySlot slot in itemSlots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(pickuppedItemSO);
                addedToInventory = true;
                break; // Break out of the loop as soon as an empty slot is found
            }
        }

        if (!addedToInventory)
        {
            // All slots are full; handle the case where there's no more space
            Debug.LogWarning("No More Slots To Add Item");
        }

    }
    private void Awake()
    {
        Instance = this;

        objectDetailViewExitButton.gameObject.SetActive(false);

        useItemButton.onClick.AddListener(() =>
        {
            UseItem();

            SetMenuState(MenuStates.SlotMenuFocused);
            UpdateMenuStatesOnce();
        });

        equipItemButton.onClick.AddListener(() =>
        {
            EquipItem();

            SetMenuState(MenuStates.SlotMenuFocused);
            UpdateMenuStatesOnce();
        });

        dropUsableItemButton.onClick.AddListener(() =>
        {
            DropUsableItem();

            SetMenuState(MenuStates.SlotMenuFocused);
            UpdateMenuStatesOnce();
        });

        dropEquipableItemButton.onClick.AddListener(() =>
        {
            DropEquipableItem();

            SetMenuState(MenuStates.SlotMenuFocused);
            UpdateMenuStatesOnce();
        });

        checkUsableItemButton.onClick.AddListener(() =>
        {
            SetMenuState(MenuStates.ObjectDetailViewFocused);
            UpdateMenuStatesOnce();
        });

        checkEquipableItemButton.onClick.AddListener(() =>
        {
            SetMenuState(MenuStates.ObjectDetailViewFocused);
            UpdateMenuStatesOnce();
        });

        objectDetailViewExitButton.onClick.AddListener(() =>
        {
            SetMenuState(MenuStates.CommandMenuFocused);
            UpdateMenuStatesOnce();
        });

        exitInventoryButton.onClick.AddListener(() =>
        {
            ToggleInventoryUI();
        });

    }

    private void Start()
    {
        ClearItemDescription();

        ClearItemsOnSlot();

        SetupItemSlots();

        inventoryUiMainVisual.gameObject.SetActive(false);

        DisableCommandMenues();

        UpdateCashAmountUi();

        InputManager.Instance.OnInventoyKeyPerformed += InputManager_Instance_OnInventoyKeyPerformed;

        EventManager.Instance.OnSelectedItemChanged += EventManager_Instance_OnSelectedItemChanged;
        EventManager.Instance.OnInventoryItemsModified += EventManager_Instance_OnInventoryItemsModified;
        EventManager.Instance.OnSlotItemButtonPerformed += EventManager_Instance_OnSlotItemButtonPerformed;
    }

    private void EventManager_Instance_OnSlotItemButtonPerformed(GatherableSO item)
    {
        SeCurrentGetherableSO(item);

        UpdateDescriptionUI(item);

        if (item != null)
        {
            OpenCommandMenuUI(item);
        }
        else
        {
            //inventory Slot Empty So Close Or Dont Open Command Menus.
            DisableCommandMenues();
        }
    }

    private void OpenCommandMenuUI(GatherableSO item)
    {
        if(item.gatherableType == GatherableObjectType.Healable || item.gatherableType == GatherableObjectType.Usable || item.gatherableType == GatherableObjectType.Collectable)
        {
            OpenUseCommandmenu();
        }
        else if(item.gatherableType == GatherableObjectType.Equipable)
        {
            OpenEquipCommandmenu();
        }
        
    }

    private void DestroyDetailedViewObject()
    {
        if (itemVisualPrefab != null)
            Destroy(itemVisualPrefab.gameObject);
    }

    private void UpdateCashAmountUi()
    {
        PlayerWallet.AddCashToWallet(69); // TEST ADD CASH, DONT FORGET TO REMOVE THIS LATER
        playerCashAmount.text = PlayerWallet.GetCurrentCashAmount().ToString();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInventoyKeyPerformed -= InputManager_Instance_OnInventoyKeyPerformed;
        //InputManager.Instance.OnInteractionKeyPerformed -= InputManager_Instance_OnInteractionKeyPerformed;
        EventManager.Instance.OnSelectedItemChanged -= EventManager_Instance_OnSelectedItemChanged;
        EventManager.Instance.OnInventoryItemsModified -= EventManager_Instance_OnInventoryItemsModified;
        EventManager.Instance.OnSlotItemButtonPerformed -= EventManager_Instance_OnSlotItemButtonPerformed;

        useItemButton.onClick.RemoveAllListeners();
        equipItemButton.onClick.RemoveAllListeners();

        dropUsableItemButton.onClick.RemoveAllListeners();
        dropEquipableItemButton.onClick.RemoveAllListeners();

        checkUsableItemButton.onClick.RemoveAllListeners();
        checkEquipableItemButton.onClick.RemoveAllListeners();

        exitInventoryButton.onClick.RemoveAllListeners();
        objectDetailViewExitButton.onClick.RemoveAllListeners();
    }

    private void EventManager_Instance_OnInventoryItemsModified(object sender, EventArgs e)
    {
        ClearItemsOnSlot();

        SetupItemSlots();

        DisableCommandMenues();

        SetInteractionToIgnoreBtns(true);

        SetSelectedObject(lastSelectedobj); // Select the First elemet HighLighted

        isCommandMenuOpened = !isCommandMenuOpened;
    }

    public void DisableCommandMenues()
    {
        commandMenuUsable.gameObject.SetActive(false);
        commandMenuEquipable.gameObject.SetActive(false);
    }

    private void EventManager_Instance_OnSelectedItemChanged(object sender, EventArgs e)
    {
        DisableCommandMenues();
    }

   

    public void SetInteractionToIgnoreBtns(bool interact)
    {
        foreach (Button button in ignoreButtonsWhileOpenCommandMenu)
        {
            button.interactable = interact;
        }
    }

    public void UseItem()
    {
        Inventory.Instance.UseItem(currentSelectedItem);
    }

    public void EquipItem()
    {
        Inventory.Instance.EquipItem(currentSelectedItem,(item) =>
        {
            // OnSuccessfully Item Equipped
            //ClearEquipeSlots();
            SetupEquipSlots(item);
        });
    }

    private void ClearEquipeSlots()
    {
        foreach (EquipItemSlot item in equippedItemSlots)
        {
            item.ClearItem();
        }
    }

    private void SetupEquipSlots(GatherableSO equipableItem)
    {
        bool itemSetupped = false;
        foreach (EquipItemSlot item in equippedItemSlots)
        {
            if(item.IsEmpty())
            {
                item.SetItem(equipableItem);
                itemSetupped = true;    
                break;
            }
        }

        if(!itemSetupped)
        {
            Debug.LogWarning("No Slot There To Place Equipped item");
        }
    }

    private void DropUsableItem()
    {
        Inventory.Instance.DropUsableItem(currentSelectedItem);
    }

    private void DropEquipableItem()
    {
        Inventory.Instance.DropEquipableItem(currentSelectedItem);
    }
    private void SpawnCheckingItem(GatherableSO currentSelectedItem)
    {
        if(itemVisualPrefab  != null)
        {
            Destroy(itemVisualPrefab.gameObject);
        }

        itemVisualPrefab = Instantiate(currentSelectedItem.gatherableObjectPrefab, new Vector3(1000f, 1000f, 1000f), Quaternion.identity).transform;
        itemVisualPrefab.gameObject.AddComponent<ObjectRotator>();

    }

    private void InputManager_Instance_OnInventoyKeyPerformed(object sender, EventArgs e)
    {
        ToggleInventoryUI();
    }

    private void SetupItemSlots()
    {
        List<GatherableSO> items = Inventory.Instance.GetInventoryItems();

        if (items.Count > 0)
            SetupItemsInSlots(items);
        else
            Debug.LogWarning("There is No Item to Set up Visual");
    }

    private void SetupItemsInSlots(List<GatherableSO> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            itemSlots[i].SetItem(items[i]);
        }
    }

    private void ClearItemsOnSlot()
    {
        foreach (InventorySlot slot in itemSlots)
        {
            slot.ClearItem();
        }
    }

    private void ToggleInventoryUI()
    {
        isInventoryUiOpened = !isInventoryUiOpened;

        if (isInventoryUiOpened)
        {
            inventoryUiMainVisual.gameObject.SetActive(true);
            SetSelectedObject(itemSlots[0].gameObject); // whenever inventory opens Select the First elemet HighLighted
            UpdateDescriptionUI(currentSelectedItem);

            EventManager.Instance.InvokeInventoryOpened();
            FirstPersonController.SetCurserLockMode(true);
        }
        else
        {
            commandMenuEquipable.gameObject.SetActive(false);
            commandMenuUsable.gameObject.SetActive(false);
            inventoryUiMainVisual.gameObject.SetActive(false);

            objectDetailViewExitButton.gameObject.SetActive(false);
            SetInteractionToIgnoreBtns(true);
            DestroyDetailedViewObject();

            EventManager.Instance.InvokeInventoryClosed();
        }
    }

    private void OpenUseCommandmenu()
    {
        commandMenuEquipable.gameObject.SetActive(false);
        commandMenuUsable.gameObject.SetActive(true);

        SetSelectedObject(commandMenuUsable.GetChild(0).gameObject);
    }


    private void OpenEquipCommandmenu()
    {
        commandMenuUsable.gameObject.SetActive(false);
        commandMenuEquipable.gameObject.SetActive(true);

        SetSelectedObject(commandMenuEquipable.GetChild(0).gameObject);
    }
    private void SetSelectedObject(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(obj);
    }

    

}

using System;
using System.Collections.Generic;
using System.Linq;
using ProjectMiamiTestInventory;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;

    [SerializeField] private GatherableSO torchSO;
    [SerializeField] private Transform torchEquip;
    [SerializeField] private Transform equipmentsHolder;
    [SerializeField] private Transform inventoryItemsHolder;
    private bool isGotTorch = false;
    private bool isTorchActivated;

    private List<EquipData> equipmentData;
    private void Awake()
    {
        Instance = this;
        equipmentData = new List<EquipData>(2);
    }
    private void OnEnable()
    {
        if (EventManager.Instance == null)
        {
            Debug.LogWarning("EventManager Is Null So Cant Sub Event From Event Manager");
            return;
        }

        EventManager.Instance.OnEquipableItemEquipped += EventManager_Instance_OnEquipableItemEquipped;
        EventManager.Instance.OnEquipSlotModified += EventManager_OnEquipSlotModified;

        EventManager.Instance.OnEquipSlotModified += (equipmentSlot,gatherableSO) =>
        {
            if(gatherableSO == null)
            {
                for(int i = 0; i < equipmentData.Count; i++)
                {
                    if(equipmentData[i].equipItemSlot == equipmentSlot)
                    {
                        equipmentData[i].equipItemSlot = null;
                        Destroy(equipmentData[i].equipItemSlotItemgameObj); 
                    }
                }
            }

        };
    }

    private void EventManager_OnEquipSlotModified(EquipItemSlot equipItemSlot, GatherableSO gatherableSO)
    {
        if (gatherableSO != null && equipItemSlot != null /*&& !equipmentData.Any(e => e.equipItemSlot == equipItemSlot)*/)
        {
            Debug.Log(equipItemSlot.name + " Modified With :" + gatherableSO.gatherableObjectName);

            if (inventoryItemsHolder.childCount < 2)
            {
                EquipData emptySlot = equipmentData.FirstOrDefault(equipData => equipData.equipItemSlotItemgameObj == null);

                if (emptySlot == null)
                {
                    emptySlot = new EquipData();
                    emptySlot.equipItemSlotItemgameObj = Instantiate(gatherableSO.gatherableObjectPrefab, inventoryItemsHolder);
                    emptySlot.equipItemSlotItemgameObj.transform.localPosition = Vector3.zero + new Vector3(0.3f, 0.2f, 0.3f);
                    emptySlot.equipItemSlot = equipItemSlot;

                    equipmentData.Add(emptySlot);

                    emptySlot.equipItemSlotItemgameObj.SetActive(equipItemSlot.equipSlotType == EquipSlotType.Main);
                }
                else
                {
                    emptySlot.equipItemSlotItemgameObj = Instantiate(gatherableSO.gatherableObjectPrefab, inventoryItemsHolder);
                    emptySlot.equipItemSlotItemgameObj.transform.localPosition = Vector3.zero + new Vector3(0.3f, 0.2f, 0.3f);
                    emptySlot.equipItemSlot = equipItemSlot;

                    emptySlot.equipItemSlotItemgameObj.SetActive(equipItemSlot.equipSlotType == EquipSlotType.Main);
                }
            }
        }
        else
        {
            Debug.LogWarning(equipItemSlot.name + " Does Not Have Gatherable SO");
        }


    }

    private void EventManager_Instance_OnEquipableItemEquipped(GatherableSO item)
    {
        if (isGotTorch) return; // means already got torch No need to false it 

        isGotTorch = item == torchSO;
    }

    private void Start()
    {
        InputManager.Instance.OnTorchKeyPerformed += InputManager_Instance_OnTorchKeyPerformed;

        DisableEquipedItems();
    }

    public void DisableEquipedItems()
    {
        foreach(Transform child in equipmentsHolder)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void EnableEquipedItems()
    {
        foreach (Transform child in equipmentsHolder)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void InputManager_Instance_OnTorchKeyPerformed(object sender, System.EventArgs e)
    {
        TorchToggle();
    }

    private void TorchToggle()
    {
        if (!isGotTorch) return;

        isTorchActivated = !isTorchActivated;
        
        if(isTorchActivated)
        {
            // Has Torch in inventory or already got 
            SetActiveTorch(true);
        }
        else
        {
            SetActiveTorch(false);
        }

    }

    public void SetActiveTorch(bool active)
    {
        torchEquip.gameObject.SetActive(active);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnTorchKeyPerformed -= InputManager_Instance_OnTorchKeyPerformed;
        EventManager.Instance.OnEquipableItemEquipped -= EventManager_Instance_OnEquipableItemEquipped;
        EventManager.Instance.OnEquipSlotModified -= EventManager_OnEquipSlotModified;

    }

    [Serializable]
    public class EquipData
    {
        public EquipItemSlot equipItemSlot;
        public GameObject equipItemSlotItemgameObj;
    }
}

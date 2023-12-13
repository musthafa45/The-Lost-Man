using System.Linq;
using ProjectMiamiTestInventory;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;

    [SerializeField] private GatherableSO torchSO;
    [SerializeField] private Transform torchEquip;
    [SerializeField] private Transform equipmentsHolder;
    private bool isGotTorch = false;
    private bool isTorchActivated;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        if (EventManager.Instance == null)
        {
            Debug.LogWarning("EventManager Is Null So Cant Sub Event From Event Manager");
            return;
        }

        EventManager.Instance.OnEquipableItemEquipped += EventManager_Instance_OnEquipableItemEquipped;
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

    //private bool HasTorchInInventory()
    //{
    //    if(InventoryTest.Instance == null) return false;

    //    GatherableSO[] gatherableSOs = InventoryTest.Instance.GetGatheredObjectList().ToArray();
    //    var hasTorch = gatherableSOs.Any(s => s == torchSO);
    //    if (hasTorch)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //private void RemoveTorchFromInventory()
    //{
    //    if (isGotTorch) return;

    //    GatherableSO[] gatherableSOs = InventoryTest.Instance.GetGatheredObjectList().ToArray();
    //    var torch = gatherableSOs.Where(s => s.gatherableObjectName == torchSO.gatherableObjectName).FirstOrDefault(); ;
    //    InventoryTest.Instance.RemoveObjectFromInventory(torch);  // remove key from inventory
    //}

    private void OnDisable()
    {
        InputManager.Instance.OnTorchKeyPerformed -= InputManager_Instance_OnTorchKeyPerformed;
        EventManager.Instance.OnEquipableItemEquipped -= EventManager_Instance_OnEquipableItemEquipped;
    }
}

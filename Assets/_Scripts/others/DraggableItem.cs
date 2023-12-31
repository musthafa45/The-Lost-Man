
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private GatherableSO gatherableObjectSO;
    private Transform orginParent;


    private void Awake()
    {
        UpdateSlotImage();
    }

    public void UpdateSlotImage()
    {
        if (gatherableObjectSO != null)
        {
            itemImage.sprite = gatherableObjectSO.gatherableImageSprite;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = false;

        gatherableObjectSO = GetComponentInParent<InventorySlot>().GetGatherableObjSO();
        orginParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        ItemInfoHandlerUI.Instance.ClearInfoObject();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = InputManager.Instance.GetMousePosition();

        ItemInfoHandlerUI.Instance.ClearInfoObject();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = true;
        transform.SetParent(orginParent);
    }

    public void SetParent(Transform parent)
    {
        this.orginParent = parent;
    }

    public void SetGatherableObjSO(GatherableSO gatherableSO)
    {
        this.gatherableObjectSO = gatherableSO;
    }
    public GatherableSO GetGatherableSO()
    {
        return gatherableObjectSO;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gatherableObjectSO != null)
        {
            ItemInfoHandlerUI.Instance.SetItemAndShow(gatherableObjectSO, transform.position);
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


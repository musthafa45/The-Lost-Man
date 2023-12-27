using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image itemImage;
    private Transform orginParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = false;

        orginParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = InputManager.Instance.GetMousePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //RepairGridLayout();
        itemImage.raycastTarget = true;
        transform.SetParent(orginParent);
    }

    public void SetParent(Transform parent)
    {
        this.orginParent = parent;
    }

    private void RepairGridLayout()
    {
        transform.GetComponentInParent<GridLayoutGroup>().enabled = false;
        transform.GetComponentInParent<GridLayoutGroup>().enabled = true;
    }

}

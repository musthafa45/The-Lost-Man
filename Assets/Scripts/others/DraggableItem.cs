using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler
{
    [SerializeField] private Image selectedItemImage;
    [SerializeField] private Image draggableItemImage;
    //private Transform orginParent;

    private void Start()
    {
        selectedItemImage.gameObject.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBegin Drag called");
        draggableItemImage.raycastTarget = false;

        selectedItemImage.gameObject.SetActive(false);
        //orginParent = transform.parent;
        //transform.SetParent(transform.root);
        //transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On Drag calling");
        transform.position = InputManager.Instance.GetMousePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEnd Drag called");
        RepairGridLayout();

        draggableItemImage.raycastTarget = true;
        //transform.SetParent(orginParent);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("On Pointer Down Performed On this Object");
        selectedItemImage.gameObject.SetActive(true);
    }

    public void SetActivateSelectedVisual(bool active)
    {
        selectedItemImage.enabled = active;
    }

    private void RepairGridLayout()
    {
        transform.GetComponentInParent<GridLayoutGroup>().enabled = false;
        transform.GetComponentInParent<GridLayoutGroup>().enabled = true;
    }

}

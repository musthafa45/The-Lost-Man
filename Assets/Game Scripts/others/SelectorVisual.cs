using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorVisual : MonoBehaviour
{
    [SerializeField] private float selectorLerpSpeed = 25f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        var selectedObject = EventSystem.current.currentSelectedGameObject;

        if (selectedObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, selectedObject.transform.position, selectorLerpSpeed * Time.deltaTime);

            var selectedObjRect = selectedObject.GetComponent<RectTransform>();

            var horizontalLerp = Mathf.Lerp(rectTransform.rect.size.x, selectedObjRect.rect.size.x, selectorLerpSpeed * Time.deltaTime);
            var verticalLerp = Mathf.Lerp(rectTransform.rect.size.y, selectedObjRect.rect.size.y, selectorLerpSpeed * Time.deltaTime);

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalLerp);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalLerp);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField] private HoldPointData leftHoldPointData;
    [SerializeField] private HoldPointData rightHoldPointData;

    [SerializeField] private float throwForce = 100f;
    [SerializeField] private ForceMode throwForceMode = ForceMode.Impulse;

    private Dictionary<HoldPointData,HoldableObject> holdingObjects = new Dictionary<HoldPointData, HoldableObject>(2);
    private void OnEnable()
    {
        InputManager.Instance.OnThrowKeyPerformed += InputManager_Instance_OnThrowKeyPerformed;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnThrowKeyPerformed -= InputManager_Instance_OnThrowKeyPerformed;
    }

    private void InputManager_Instance_OnThrowKeyPerformed(object sender, EventArgs e)
    {
        HoldableObject holdableObj;

        if (leftHoldPointData.IsAccupied)
        {
            holdableObj = holdingObjects[leftHoldPointData];

            if (holdableObj != null)
            {
                if (holdableObj.TryGetComponent(out Rigidbody rb))
                {
                    holdableObj.transform.SetParent(null);
                    holdableObj.transform.gameObject.layer = LayerMask.NameToLayer("Interactable");
                    rb.isKinematic = false;
                    holdableObj.gameObject.GetComponent<Collider>().isTrigger = false;
                    rb.AddForce(holdableObj.transform.forward * throwForce,throwForceMode);
                    holdingObjects.Remove(leftHoldPointData);
                    leftHoldPointData.IsAccupied = false;
                }
            }
        }

        if(rightHoldPointData.IsAccupied)
        {
            holdableObj = holdingObjects[rightHoldPointData];

            if (holdableObj != null)
            {
                if (holdableObj.TryGetComponent(out Rigidbody rb))
                {
                    holdableObj.transform.SetParent(null);
                    holdableObj.transform.gameObject.layer = LayerMask.NameToLayer("Interactable");
                    rb.isKinematic = false;
                    holdableObj.gameObject.GetComponent<Collider>().isTrigger = false;
                    rb.AddForce(holdableObj.transform.forward * throwForce,throwForceMode);
                    holdingObjects.Remove(rightHoldPointData);
                    rightHoldPointData.IsAccupied = false;
                }
            }
        }

    }

    public void SetParent(HoldableObject holdableObject)
    {
        if (!rightHoldPointData.IsAccupied)
        {
            holdableObject.transform.SetParent(rightHoldPointData.HoldPointTransform);
            holdableObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            holdableObject.gameObject.GetComponent<Collider>().isTrigger = true;
            holdableObject.transform.localPosition = Vector3.zero;
            rightHoldPointData.IsAccupied = true;
            holdableObject.transform.gameObject.layer = LayerMask.NameToLayer("Default");
            holdableObject.transform.rotation = rightHoldPointData.HoldPointTransform.rotation;
            holdingObjects.Add(rightHoldPointData, holdableObject);
        }
        else if(!leftHoldPointData.IsAccupied && !Torch.Instance.IsActive())
        {
            holdableObject.transform.SetParent(leftHoldPointData.HoldPointTransform);
            holdableObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            holdableObject.gameObject.GetComponent<Collider>().isTrigger = true;
            holdableObject.transform.localPosition = Vector3.zero;
            leftHoldPointData.IsAccupied = true;
            holdableObject.transform.gameObject.layer = LayerMask.NameToLayer("Default");
            holdableObject.transform.rotation = leftHoldPointData.HoldPointTransform.rotation;
            holdingObjects.Add(leftHoldPointData, holdableObject);
        }
        else
        {
            Debug.LogWarning("All Holding Slots Accupied");
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (leftHoldPointData.HoldPointTransform != null && rightHoldPointData.HoldPointTransform != null)
        {
            Gizmos.DrawSphere(leftHoldPointData.HoldPointTransform.position, 0.05f);
            Gizmos.DrawSphere(rightHoldPointData.HoldPointTransform.position, 0.05f);
        }

    }

    [Serializable]
    private struct HoldPointData
    {
        public Transform HoldPointTransform;
        public bool IsAccupied;
    }
}

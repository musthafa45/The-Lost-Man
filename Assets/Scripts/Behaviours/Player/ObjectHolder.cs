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

    private Dictionary<HoldPointData,HoldableObject> holdingObjects = new(2);
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
                    ThrowObject(leftHoldPointData,holdableObj, rb);
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
                    ThrowObject(rightHoldPointData, holdableObj, rb);
                }
            }
        }

        void ThrowObject(HoldPointData holdPointData, HoldableObject holdableObj, Rigidbody rb)
        {
            holdableObj.transform.SetParent(null);
            holdableObj.transform.gameObject.layer = LayerMask.NameToLayer("Interactable");
            rb.isKinematic = false;
            holdableObj.gameObject.GetComponent<Collider>().isTrigger = false;

            rb.AddForce(Camera.main.transform.forward * holdableObj.GetHoldableObjectSO().throwForce, throwForceMode);

            holdingObjects.Remove(holdPointData);
            holdPointData.IsAccupied = false;

            Debug.Log(holdPointData.HoldPointName);
            Debug.Log(holdableObj.GetHoldableObjectSO().throwForce);
        }

    }

    public void SetParent(HoldableObject holdableObject)
    {
        if (!rightHoldPointData.IsAccupied)
        {
            Set(holdableObject,rightHoldPointData);
        }
        else if(!leftHoldPointData.IsAccupied && !Torch.Instance.IsActive())
        {
            Set(holdableObject, leftHoldPointData);
        }
        else
        {
            Debug.LogWarning("All Holding Slots Accupied");
        }

        void Set(HoldableObject holdableObject,HoldPointData holdPointData)
        {
            holdableObject.transform.SetParent(holdPointData.HoldPointTransform);
            holdableObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            holdableObject.gameObject.GetComponent<Collider>().isTrigger = true;
            holdableObject.transform.localPosition = Vector3.zero + holdableObject.GetHoldableObjectSO().holdOffset;
            holdPointData.IsAccupied = true;
            holdableObject.transform.gameObject.layer = LayerMask.NameToLayer("Default");
            holdableObject.transform.rotation = holdPointData.HoldPointTransform.rotation;
            holdingObjects.Add(holdPointData, holdableObject);
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
    private class HoldPointData
    {
        public string HoldPointName;
        public Transform HoldPointTransform;
        public bool IsAccupied;
    }
}

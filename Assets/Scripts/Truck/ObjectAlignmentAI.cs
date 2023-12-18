using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectAlignmentAI : MonoBehaviour
{
    [SerializeField] private List<AlignPointData> alignPointDatas;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != null)
        {
            if(other.gameObject.TryGetComponent<HoldableObject>(out var holdableObject))
            {
                Debug.Log("Holdable Object Entered");

                bool hasAlreadyThisObject = alignPointDatas.Any(a => a.holdableObjectTransform == holdableObject.transform);

                if (!holdableObject.GetHoldableObjectSO().canPlaceInTruck || holdableObject
                    .IsThrowedByPlayer() == false || hasAlreadyThisObject) return;

                AlignPointData availableAlignPoint = alignPointDatas.Find(a => !a.isAccupied);

                if (availableAlignPoint != null)
                {
                    holdableObject.SetIsPlacedInTruck(true);

                    holdableObject.transform.SetParent(availableAlignPoint.AlignPointTransform);
                    holdableObject.transform.localRotation = availableAlignPoint.AlignPointTransform.localRotation;

                    holdableObject.GetComponent<Collider>().isTrigger = true;
                    holdableObject.GetComponent<Rigidbody>().isKinematic = true;

                    availableAlignPoint.isAccupied = true;
                    availableAlignPoint.holdableObjectTransform = holdableObject.transform;
                    holdableObject.transform.localPosition = Vector3.zero;

                }
            }
        }
    }

    public void RemovePlacedObject(HoldableObject holdableObject)
    {
        Debug.Log("Holdable Object Exited");

        AlignPointData availableAlignPoint = alignPointDatas.Find(a => a.holdableObjectTransform == holdableObject.transform);

        if (availableAlignPoint != null)
        {
            holdableObject.SetIsPlacedInTruck(false);

            holdableObject.GetComponent<Collider>().isTrigger = false;
            holdableObject.GetComponent<Rigidbody>().isKinematic = false;

            availableAlignPoint.isAccupied = false;
            availableAlignPoint.holdableObjectTransform = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        foreach (AlignPointData alignPointData in alignPointDatas)
        {
            Gizmos.DrawSphere(alignPointData.AlignPointTransform.position, 0.1f);
        }
    }

    [Serializable]
    public class AlignPointData
    {
        public Transform AlignPointTransform;
        [HideInInspector] public Transform holdableObjectTransform;
        public bool isAccupied = false;
    }

}

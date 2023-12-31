using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapController : MonoBehaviour
{
    [SerializeField] private Transform seatTransform;

    private bool isPlayerInside = false;
    private Transform playerTransform;
    private TruckDoor truckDoor;

    private void OnEnable()
    {
        EventManager.Instance.OnPlayerTryGetInTruck += EventManager_Instance_OnPlayerTryInteractTruckDoor;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnPlayerTryGetInTruck -= EventManager_Instance_OnPlayerTryInteractTruckDoor;
    }

    private void EventManager_Instance_OnPlayerTryInteractTruckDoor(Transform playerTransform, TruckDoor truckDoor)
    {
        this.playerTransform = playerTransform;
        this.truckDoor = truckDoor;

        if(!isPlayerInside)
        {
            GetPlayerInTruck(playerTransform);

            EventManager.Instance.InvokePlayerGetsInTruck();

            isPlayerInside = true;
        }
        else
        {
            LeavePlayerOutSideTruck(playerTransform,truckDoor);

            EventManager.Instance.InvokePlayerGetsOutTruck();
        }
       
    }

    private void LeavePlayerOutSideTruck(Transform playerTransform,TruckDoor truckDoor)
    {
        Transform exitPointTr = truckDoor.GetLeaveWayTransform();

        playerTransform.SetParent(null);
        playerTransform.position = exitPointTr.position;
        playerTransform.rotation = exitPointTr.rotation;


        EnablePlayerColliders(playerTransform);
        isPlayerInside = false;
    }

    private void EnablePlayerColliders(Transform playerTransform)
    {
        CapsuleCollider[] colliders = playerTransform.GetComponents<CapsuleCollider>();
        foreach (CapsuleCollider collider in colliders)
        {
            collider.enabled = true;
        }

        FirstPersonController.Instance.enabled = true;
        playerTransform.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void GetPlayerInTruck(Transform playerTransform)
    {
        playerTransform.SetParent(seatTransform);
        playerTransform.localPosition = Vector3.zero;
        playerTransform.localRotation = Quaternion.identity;


        DisablePlayerColliders(playerTransform);
    }

    private void DisablePlayerColliders(Transform playerTransform)
    {
        CapsuleCollider[] colliders = playerTransform.GetComponents<CapsuleCollider>();
        foreach (CapsuleCollider collider in colliders)
        {
            collider.enabled = false;
        }

        FirstPersonController.Instance.enabled = false;
        playerTransform.GetComponent<Rigidbody>().isKinematic = true;
    }

}

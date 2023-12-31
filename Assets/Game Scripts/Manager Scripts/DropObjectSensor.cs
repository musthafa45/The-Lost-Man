using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropObjectSensor : MonoBehaviour
{
    [SerializeField] private LayerMask GatherableObjectLayer;

    [SerializeField] private DropSpawnPointData[] dropSpawnPointDataArray;
    [SerializeField] private Transform dropParentTransform;

    private void OnEnable()
    {
        EventManager.Instance.OninventoryClosed += EventManager_Instance_OninventoryClosed;
    }
    private void OnDisable()
    {
        ClearDropSpawnPointData();
        EventManager.Instance.OninventoryClosed -= EventManager_Instance_OninventoryClosed;
    }
    private void EventManager_Instance_OninventoryClosed(object sender, EventArgs e)
    {
        ClearDropSpawnPointData();
    }

    private void RayCheck(DropSpawnPointData spawnPointData)
    {
        float rayDistance = 2f;
        Ray ray = new Ray(spawnPointData.SpawnPointTransform.position, -spawnPointData.SpawnPointTransform.up);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, GatherableObjectLayer))
        {
            if (hit.collider != null)
            {
                //Debug.Log("Hitted On Gatherable Object " + hit.collider.gameObject.name);
                if (!spawnPointData.IsHittingObject)
                {
                    spawnPointData.IsHittingObject = true;
                }
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
            }
        }
        else
        {
            if (spawnPointData.IsHittingObject)
            {
                spawnPointData.IsHittingObject = false;
            }
        }
    }
    public Vector3 GetRandomDropPoint()
    {
        var spawnPointData = dropSpawnPointDataArray.Where(dropPoint => !dropPoint.IsUsed && !dropPoint.IsHittingObject).FirstOrDefault();

        if (spawnPointData != null)
        {
            spawnPointData.IsUsed = true;
            return spawnPointData.SpawnPointTransform.position;
        }
        else
        {
            Debug.LogError("No randomPoint found ");
            return Vector3.zero;
        }

    }

    private void ClearDropSpawnPointData()
    {
        foreach (DropSpawnPointData spawnPointData in dropSpawnPointDataArray)
        {
            spawnPointData.IsUsed = false;
        }
    }

    private void FixedUpdate()
    {
        foreach (var spawnPointData in dropSpawnPointDataArray)
        {
            RayCheck(spawnPointData);
        }
    }


    private void OnDrawGizmos()
    {
        if (dropParentTransform == null) return;
        Gizmos.color = Color.magenta;
        foreach (var dropSpawnPointData in dropSpawnPointDataArray)
        {
            if (dropSpawnPointData.SpawnPointTransform != null)
                Gizmos.DrawSphere(dropSpawnPointData.SpawnPointTransform.position, 0.05f);
        }
    }


    [Serializable]
    public class DropSpawnPointData
    {
        public Transform SpawnPointTransform;
        public bool IsUsed = false;
        public bool IsHittingObject = false;
    }
}

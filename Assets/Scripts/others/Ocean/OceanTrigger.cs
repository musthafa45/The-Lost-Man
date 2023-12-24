using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanTrigger : MonoBehaviour
{
    [SerializeField] private float oceanTriggerRadius = 20f;
    private Transform[] oceanTriggerPoints;
    private Transform playerTransform = null;

    private void Awake()
    {
        Transform[] childTransforms = transform.GetComponentsInChildren<Transform>();

        // Exclude the parent transform and resize the array
        oceanTriggerPoints = new Transform[childTransforms.Length - 1];
        Array.Copy(childTransforms, 1, oceanTriggerPoints, 0, oceanTriggerPoints.Length);

        playerTransform = FindObjectOfType<FirstPersonController>().transform;
    }

    private void Update()
    {
        if(playerTransform != null)
        {
            foreach(Transform oceanTriggerTransform in oceanTriggerPoints)
            {
                float playerToOceanDis = Vector3.Distance(oceanTriggerTransform.position, playerTransform.position);
                if(playerToOceanDis < oceanTriggerRadius)
                {
                    Debug.Log("Player Near To Ocean");
                }
            }
        }
    }


}

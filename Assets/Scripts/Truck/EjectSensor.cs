using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectSensor : MonoBehaviour
{
    Ray ray;
    [SerializeField] private LayerMask groundlayer;
    private bool playerKicked;

    private void FixedUpdate()
    {
        ray.origin = transform.position;
        ray.direction = transform.up * 2f;

        if(Physics.Raycast(ray,out RaycastHit hitInfo,groundlayer))
        {
            Debug.DrawRay(ray.origin,ray.direction,Color.green);

            if(hitInfo.collider != null && !playerKicked)
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
                playerKicked = true;
                EventManager.Instance.InvokeTruckFlipped();
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    private Rigidbody rb;
    public bool isHitted; // Accessed By Object Holder As well
    public bool isThrowed;// Accessed By Object Holder As well
    [SerializeField] private float airTime = 10f;
    [SerializeField] private float airTimeMax = 10f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.TryGetComponent(out FirstPersonController _) && !isHitted)
        {
            rb.isKinematic = true;

            Invoke(nameof(EnableGravity),5f);

            isHitted = true;
        }
    }
    private void Update()
    {
        if(isThrowed)
        {
            airTime -= Time.deltaTime;
            if(airTime < 0)
            {
                Debug.LogWarning("Arrow Out Of World So Destroyed");
                airTime = airTimeMax;
                Destroy(gameObject);
            }
        }
        else
        {
            airTime = airTimeMax;
        }
    }

    private void EnableGravity()
    {
        if (isThrowed)
        {
            Debug.Log("Called");
            rb.isKinematic = false;
            isThrowed = false;
        }
           
    }
}

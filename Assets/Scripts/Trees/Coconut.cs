using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent(out HoldableObject holdableObject))
        {
            if(holdableObject.GetHoldableObjectSO().Name == "Stone")
            {
                float hitObjForce = holdableObject.GetComponent<Rigidbody>().velocity.y;
                health -= hitObjForce * 10f;
                transform.DOShakePosition(0.3f,1f,5);
                Debug.Log(holdableObject.GetComponent<Rigidbody>().velocity);

                if(health < 0)
                {
                  GetComponent<Rigidbody>().isKinematic = false;
                }
            }
          
        }

    }
}

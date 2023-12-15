using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    [SerializeField] private float health = 30f;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent(out HoldableObject holdableObject))
        {
            if(holdableObject.GetHoldableObjectSO().Name == "Stone")
            {
                float hitObjForce = Mathf.FloorToInt(Mathf.Abs(holdableObject.GetComponent<Rigidbody>().velocity.magnitude));
                health -= hitObjForce;
                transform.DOShakePosition(0.3f,1f,5);
                Debug.Log("Stone Hit Force ="+hitObjForce + " Coconut Health ="+ health);

                if(health <= 0)
                {
                  GetComponent<Rigidbody>().isKinematic = false;
                }
            }
          
        }

    }
}

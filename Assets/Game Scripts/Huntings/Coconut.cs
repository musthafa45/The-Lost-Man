using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    [SerializeField] private float health = 30f;
    [SerializeField] private GatherableSO gatherableObjectSO;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent(out HoldableObject holdableObject))
        {
            string hitObjName = holdableObject.GetHoldableObjectSO().Name;
            if (hitObjName == "Stone" || hitObjName == "Pole")
            {
                float hitObjForce = Mathf.FloorToInt(Mathf.Abs(holdableObject.GetComponent<Rigidbody>().velocity.magnitude));
                health -= hitObjForce;
                transform.DOShakePosition(0.3f,1f,5);
                Debug.Log("Stone Hit Force ="+hitObjForce + " Coconut Health ="+ health);

                if(health <= 0)
                {
                    rb.isKinematic = false;
                    Invoke(nameof(AddGatherableObjectCS),2f);
                    this.gameObject.layer = LayerMask.NameToLayer("Interactable");
                }
            }
          
        }

    }

    private void AddGatherableObjectCS()
    {
        gameObject.AddComponent<GatherableObject>().SetGatherableObjectSO(gatherableObjectSO);
    }
}

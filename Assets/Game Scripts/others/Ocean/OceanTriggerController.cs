using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OceanTriggerController : MonoBehaviour
{
    private OceanTrigger[] oceanTriggers = new OceanTrigger[3];
    [SerializeField] private Transform playerTransform;
    private void Start()
    {
        oceanTriggers = transform.GetComponentsInChildren<OceanTrigger>();

        HandleOceanTriggers();
    }
    private void Update()
    {
        HandleOceanTriggers();
    }

    private void HandleOceanTriggers()
    {
        var oceanTrigger = oceanTriggers.OrderBy(o => Vector3.Distance(o.transform.position, playerTransform.position)).FirstOrDefault();

        foreach (OceanTrigger o in oceanTriggers)
        {
            if (o != oceanTrigger)
            {
                o.gameObject.SetActive(false);
            }
            else
            {
                o.gameObject.SetActive(true);
            }
        }
    }
}

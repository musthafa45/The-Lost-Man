using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuelTank : MonoBehaviour, IInteractable
{
    [SerializeField] private HoldableObjectSO gasolineHoldableSO;
    private FuelSystem fuelSystem;

    private void Awake()
    {
        fuelSystem = FindObjectOfType<FuelSystem>();
    }
    public void Interact(Transform interactorTransform)
    {
        ObjectHolder objectHolder = FindObjectOfType<ObjectHolder>();

        HoldableObject[] playerHoldingObjs = objectHolder.GetHoldingObjectsSO();

        bool hasGasCan = playerHoldingObjs.Any(h => h.GetHoldableObjectSO() == gasolineHoldableSO);

        if (hasGasCan)
        {
            fuelSystem.AddFuel(100f);
            var fuel = playerHoldingObjs.Where(h => h.GetHoldableObjectSO() == gasolineHoldableSO).FirstOrDefault();
            Destroy(fuel.gameObject);
            objectHolder.RemoveHoldabe(fuel);
        }
    }
}

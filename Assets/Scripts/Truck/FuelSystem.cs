using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSystem : MonoBehaviour
{
    [SerializeField] private float currentFuelAmount = 0f;
    [SerializeField] private float fuelAmountMax = 100f;
    [SerializeField] private float fuelDrainSpeed = 0.2f;
    private TruckController truckController;

    public void AddFuel(float addFuelAmount)
    {
        currentFuelAmount += addFuelAmount;
    }

    private void Awake()
    {
        currentFuelAmount = fuelAmountMax;
        truckController = GetComponent<TruckController>();
    }

    private void Update()
    {
        if(truckController.IsMoving())
        {
            float fuelToDrain = fuelDrainSpeed * Time.deltaTime;
            currentFuelAmount = Mathf.Clamp(currentFuelAmount - fuelToDrain, 0, fuelAmountMax);

            truckController.SetCanMove(currentFuelAmount > 0);

        }
    }
}

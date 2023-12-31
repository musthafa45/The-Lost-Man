using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthReduceSpeed = 1f;
    private readonly float minHealth = 0f;
    private bool canReduceHealth;

    private void Awake()
    {
        health = maxHealth;
    }
    private void OnEnable()
    {
        if (EventManager.Instance == null)
        {
            Debug.LogWarning("EventManager Is Null So Cant Sub Event From Event Manager");
            return;
        }
           
        EventManager.Instance.OnStaminaFinished += (sender, e) => canReduceHealth = true;
        EventManager.Instance.OnStaminaTopUpped += (sender, e) => canReduceHealth = false;
    }

    private void Update()
    {
        if (canReduceHealth)
        {
            DecreaseHealth(Time.deltaTime * healthReduceSpeed);
        }
    }
    public float GetHealth()
    {
        return health;
    }

    public void AddHealth(float amount, out float exessHealth) // Over Flow Limit Added
    {
        float newHealth = health + amount;
       
        if(newHealth > maxHealth)
        {
            float extraHealth = newHealth - maxHealth;
            exessHealth = extraHealth;

            health = maxHealth;
        }
        else
        {
            health = newHealth;
            exessHealth = 0;
        }
    }

    public void DecreaseHealth(float amount)  // Over Flow Limit Added
    {
        float newHealth = health - amount;

        if(newHealth <= minHealth)
        {
            health = minHealth;
        }
        else
        {
            health -= amount;
        }
       
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}

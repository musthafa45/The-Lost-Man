using System;
using UnityEngine;

public class PlayerStaminaSystem : MonoBehaviour
{
    [SerializeField] private float staminaDecreaseSpeed = 0.5f;
    [SerializeField] private float currentStaminaLevel = 0f;
    private readonly float staminaAmountMax = 100f;
    private readonly float staminaAmountMin = 0f;

    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private HealthSystem playerHealthSystem;
    private bool isWalking;
    private bool isSprinting;
    private bool isJumping;

    private float walkingFraction = 0f;
    private float sprintingFraction = 0f;
    private float jumpingFraction = 0f;

    [SerializeField] private float walkingFractionSpeed = 0.5f;
    [SerializeField] private float sprintingFractionSpeed = 0.2f;
    [SerializeField] private float jumpingFractionSpeed = 0.15f;

    [SerializeField]
    [Range(0f, 100f)] private float staminaSpeedAffectPoint = 20; // Below 20% Stamina Player Start Losing Speed
    private float playerNormalWalkSpeed;
    private float playerNormalSprintSpeed;
    private float playerNormalJumpSpeed;

    private bool isStaminaFinishedCalled = false;
    private void Awake()
    {
        currentStaminaLevel = staminaAmountMax;

        playerNormalWalkSpeed = firstPersonController.walkSpeed;
        playerNormalSprintSpeed = firstPersonController.sprintSpeed;
        playerNormalJumpSpeed = firstPersonController.jumpPower;
    }
    private void OnEnable()
    {
        EventManager.Instance.OnHealableItemUsed += EventManager_Instance_OnHealableItemUsed;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnHealableItemUsed -= EventManager_Instance_OnHealableItemUsed;
    }

    private void EventManager_Instance_OnHealableItemUsed(GatherableSO obj)
    {
        if (playerHealthSystem.GetHealth() < playerHealthSystem.GetMaxHealth())
        {
            playerHealthSystem.AddHealth(obj.value, out float extraHealth);
            AddStamina(extraHealth);
        }
        else
        {
            AddStamina(obj.value);
        }
    }

    public float GetCurrentStamina()
    {
        return currentStaminaLevel;
    }

    public float GetStaminaMax()
    {
        return staminaAmountMax;
    }

    private void Update()
    {
        CalculateStamina();

        HandleImpactPlayerMoves();
    }

    private void HandleImpactPlayerMoves()
    {
        if (currentStaminaLevel <= staminaSpeedAffectPoint)
        {
            if (currentStaminaLevel <= 20f && currentStaminaLevel >= 5f)
            {
                Debug.Log("Player Stamina Decreased By : 5 To 20");
                firstPersonController.walkSpeed = 3f;
                firstPersonController.sprintSpeed = 5f;
                firstPersonController.jumpPower = 3f;

            }

            if (currentStaminaLevel <= 5f)
            {
                Debug.Log("Player Stamina Decreased Below : 5 ");
                firstPersonController.walkSpeed = 2f;
                firstPersonController.enableSprint = false;
                firstPersonController.enableJump = false;
            }
        }
        else
        {
            firstPersonController.walkSpeed = playerNormalWalkSpeed;
            firstPersonController.sprintSpeed = playerNormalSprintSpeed;
            firstPersonController.jumpPower = playerNormalJumpSpeed;

            firstPersonController.enableSprint = true;
            firstPersonController.enableJump = true;
        }

        if (currentStaminaLevel <= staminaAmountMin && !isStaminaFinishedCalled)
        {
            Debug.Log("Player dead due to No Stamina");

            EventManager.Instance.InvokeStaminaFinished();
            isStaminaFinishedCalled = true;
        }
    }

    private void CalculateStamina()
    {
        if (firstPersonController != null)
        {
            isWalking = firstPersonController.isWalking;
            isSprinting = firstPersonController.isSprinting;
            isJumping = !firstPersonController.isGrounded;

            walkingFraction = isWalking ? walkingFractionSpeed : 0f;
            sprintingFraction = isSprinting ? sprintingFractionSpeed : 0f;
            jumpingFraction = isJumping ? jumpingFractionSpeed : 0f;
        }

        // Calculate overall stamina reduction rate by multiplying the fractions
        float reductionRate = 1f - (1f - walkingFraction) * (1f - sprintingFraction) * (1f - jumpingFraction) * staminaDecreaseSpeed;

        currentStaminaLevel -= reductionRate * Time.deltaTime;

        currentStaminaLevel = Mathf.Clamp(currentStaminaLevel, staminaAmountMin, staminaAmountMax);
    }

    public void AddStamina(float staminaAmount)
    {
        currentStaminaLevel += staminaAmount;

        EventManager.Instance.InvokeStaminaTopUpped();
        isStaminaFinishedCalled = false;
        Debug.Log("Stamina Added");
    }
}

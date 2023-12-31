using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerProfileUI : MonoBehaviour
{
    [SerializeField] private Image playerStaminaBarImage;
    private HealthSystem playerHealthSystem;
    private PlayerStaminaSystem playerStaminaSystem;
    [SerializeField] private float barUpdateDelay = 0.3f;
    private bool isUpdatingBar = false; // Flag to prevent overlapping animations

    [SerializeField] private Image playerHealthFadeIndigateImage;

    private void Awake()
    {
        playerHealthSystem = FindObjectOfType<HealthSystem>();
        playerStaminaSystem = FindObjectOfType<PlayerStaminaSystem>();
    }

    private void Update()
    {
        // Update Ui Stamina Bar Image or Indigation Prop
        float targetFillAmountStamina = playerStaminaSystem.GetCurrentStamina() / playerStaminaSystem.GetStaminaMax();

        if (playerStaminaBarImage != null)
        {
            if (!isUpdatingBar)
            {
                isUpdatingBar = true;

                UpdateStaminaBoostedUi(targetFillAmountStamina, barUpdateDelay);
            }

            playerStaminaBarImage.color = Color.Lerp(Color.red,Color.green, targetFillAmountStamina);
        }
        else
        {
            Debug.LogError("No References For Ui Stamina Indication Bar Image");
        }

        // Update Ui health Image or Indigation Prop
        float targetFillAmountHealth = playerHealthSystem.GetHealth() / playerHealthSystem.GetMaxHealth();

        if (playerHealthFadeIndigateImage != null)
        {
            playerHealthFadeIndigateImage.color = Color.Lerp(Color.red, Color.green, targetFillAmountHealth);
        }
        else
        {
            Debug.LogError("No References For Health Ui Indication Image");
        }

    }

    private void UpdateStaminaBoostedUi(float targetFillAmount, float duration)
    {
        playerStaminaBarImage.DOFillAmount(targetFillAmount, duration)
            .OnComplete(() => {
                isUpdatingBar = false;
            });

    }
}

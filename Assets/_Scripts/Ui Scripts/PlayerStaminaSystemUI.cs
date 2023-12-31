using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaSystemUI : MonoBehaviour
{
    [SerializeField] private Image indigationImageStamina;
    [SerializeField] private TextMeshProUGUI playerStaminaText;
    private PlayerStaminaSystem playerStaminaSystem;
    [SerializeField] private float barUpdateDelay = 0.3f;
    private bool isUpdatingBar = false; // Flag to prevent overlapping animations

    private void Awake()
    {
       playerStaminaSystem = FindObjectOfType<PlayerStaminaSystem>();
    }
    private void Update()
    {
        if (indigationImageStamina != null)
        {
            if (!isUpdatingBar)
            {
                isUpdatingBar = true;
                float targetFillAmount = playerStaminaSystem.GetCurrentStamina() / playerStaminaSystem.GetStaminaMax();
                UpdateStaminaBoostedUi(targetFillAmount, barUpdateDelay);
            }
        }
        else
        {
            Debug.LogError("No References For Ui Indication Image");
        }

        if (playerStaminaText != null)
        {
            playerStaminaText.text = "Player Stamina Level" + ":" + Mathf.RoundToInt(playerStaminaSystem.GetCurrentStamina());
        }
        else
        {
            Debug.LogError("No References For Ui Text");
        }
    }

    private void UpdateStaminaBoostedUi(float targetFillAmount, float duration)
    {
        indigationImageStamina.DOFillAmount(targetFillAmount, duration)
            .OnComplete(() => {
                isUpdatingBar = false;
            });

    }
}

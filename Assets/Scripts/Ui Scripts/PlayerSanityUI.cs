using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSanityUI : MonoBehaviour
{
    [SerializeField] private Image indigationImage;
    [SerializeField] private TextMeshProUGUI playerCurrentFearLevelText;
    private PlayerFearSystem playerFearSystem;

    private void Awake()
    {
        playerFearSystem = FindObjectOfType<PlayerFearSystem>();
    }
    private void LateUpdate()
    {
        if (indigationImage != null)
        {
            indigationImage.fillAmount = playerFearSystem.GetFearLevel() / playerFearSystem.GetFearMax();
        }
        else
        {
            Debug.LogError("No References For Ui Indigation Image");
        }


        if (playerCurrentFearLevelText != null)
        {
            playerCurrentFearLevelText.text = "Player Fear Level" + ":" + Mathf.RoundToInt(playerFearSystem.GetFearLevel());
        }
        else
        {
            Debug.LogError("No References For Ui Text");
        }
    }
}



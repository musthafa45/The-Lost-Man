using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseItemUI : MonoBehaviour
{
    public static UseItemUI Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        HideUseItemUI();
    }

    [SerializeField] private TextMeshProUGUI useItemTextGui;
    public void ShowUseItemUI()
    {
        useItemTextGui.gameObject.SetActive(true);
    }

    public void HideUseItemUI()
    {
        useItemTextGui.gameObject.SetActive(false);    
    }
}

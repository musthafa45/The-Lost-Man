using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameTextMeshProGui;
    [SerializeField] private TextMeshProUGUI itemDescriptionTextMeshProGui;

    public void SetUpInfoUiProps(string itemName, string description)
    {
        itemNameTextMeshProGui.text = itemName;
        itemDescriptionTextMeshProGui.text = description;
    }
}

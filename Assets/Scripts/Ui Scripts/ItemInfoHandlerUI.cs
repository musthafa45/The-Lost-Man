using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoHandlerUI : MonoBehaviour
{
    public static ItemInfoHandlerUI Instance { get; private set; }
    private GameObject infoPrefabSpawned = null;

    private void Awake()
    {
        Instance = this;
    }
    public void SetItemAndShow(GatherableSO item,Vector3 position)
    {
        DestroyInfoSpawned();

        infoPrefabSpawned = Instantiate(Prefabs.Instance.GetItemInfoSingleUiPrefab(), transform);
        infoPrefabSpawned.transform.position = position + Vector3.left * 200f + Vector3.up * 150f;
        if (infoPrefabSpawned.TryGetComponent(out ItemInfoSingleUI itemInfoSingleUI))
        {
            itemInfoSingleUI.SetUpInfoUiProps(item.gatherableObjectName, item.description[0]);
        }
    }

    private void DestroyInfoSpawned()
    {
        if (infoPrefabSpawned != null)
        {
            Destroy(infoPrefabSpawned);
        }
    }

    public void ClearInfoObject()
    {
        DestroyInfoSpawned();
    }
}

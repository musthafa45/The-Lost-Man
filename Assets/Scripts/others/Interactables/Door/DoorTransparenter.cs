using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTransparenter : MonoBehaviour
{
    [SerializeField] private Material doorTransparentMaterial;
    [SerializeField] private Material doorNormalMaterial;
    private bool isPlayerInside;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MicSoundDetector soundDetector;
    private void OnEnable()
    {
        EventManager.Instance.OnPlayerOpensDoor += EventManager_Instance_OnPlayerOpensDoor;
        EventManager.Instance.OnPlayerCloseDoor += EventManager_Instance_OnPlayerCloseDoor;

        soundDetector.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        EventManager.Instance.OnPlayerOpensDoor -= EventManager_Instance_OnPlayerOpensDoor;
        EventManager.Instance.OnPlayerCloseDoor -= EventManager_Instance_OnPlayerCloseDoor;
    }
    private void EventManager_Instance_OnPlayerOpensDoor(object sender, EventArgs e)
    {
        meshRenderer.material = doorNormalMaterial;
        soundDetector.gameObject.SetActive(false);
    }


    private void EventManager_Instance_OnPlayerCloseDoor(object sender, EventArgs e)
    {
        if (isPlayerInside)
        {
            meshRenderer.material = doorTransparentMaterial;
            EventManager.Instance.InvokePlayerEnteredAnyOutHouse();
            soundDetector.gameObject.SetActive(true);
        }
        else
        {
            meshRenderer.material = doorNormalMaterial;
            EventManager.Instance.InvokePlayerExitedAnyOutHouse();
            soundDetector.gameObject.SetActive(false);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }


}

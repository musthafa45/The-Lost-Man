using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicSoundUI : MonoBehaviour
{
    [SerializeField] private Image micSoundBar;
    [SerializeField] private float lerpSpeed = 5f; // Adjust the lerp speed as needed
    [SerializeField] private Transform micSoundBarUi;
    private float targetFillAmount;
    private float currentFillAmount;

    private float micSoudData;
    private bool isSubscribed = false;
    private void OnEnable()
    {
        EventManager.OnAnyOutHousePlayerEntered += (sender,e) => { ShowMicUI(); };
        EventManager.OnAnyOutHousePlayerExited += EventManager_OnAnyOutHousePlayerExited;
        EventManager.Instance.OnPlayerOpensDoor += EventManager_Instance_OnPlayerOpensDoor;
        EventManager.Instance.OnPlayerCloseDoor += EventManager_Instance_OnPlayerCloseDoor;
        EventManager.OnAnyGetMicSoundData += EventManager_OnAnyGetMicSoundData;
    }

    private void EventManager_OnAnyOutHousePlayerExited(object sender, System.EventArgs e)
    {
          HideMicUi();
          UnsubscribeFromOutHouseEvents(); // Unsubscribe from the out-house events when player exits
        
    }

    private void EventManager_OnAnyGetMicSoundData(float micSoundData)
    {
        this.micSoudData = micSoundData;
    }

    private void OnDisable()
    {
        EventManager.OnAnyOutHousePlayerEntered -= EventManager_OnAnyOutHouseHasPlayer;
        EventManager.Instance.OnPlayerOpensDoor -= EventManager_Instance_OnPlayerOpensDoor;
        EventManager.Instance.OnPlayerCloseDoor -= EventManager_Instance_OnPlayerCloseDoor;
        EventManager.OnAnyGetMicSoundData -= EventManager_OnAnyGetMicSoundData;

        // Ensure to unsubscribe from the OnAnyOutHouse events when disabled
        if (isSubscribed)
        {
            EventManager.OnAnyOutHousePlayerEntered -= EventManager_OnAnyOutHouseHasPlayer;
            EventManager.OnAnyOutHousePlayerExited -= EventManager_OnAnyOutHousePlayerExited;
            isSubscribed = false;
        }
    }

    private void EventManager_OnAnyOutHouseHasPlayer(object sender, System.EventArgs e)
    {
        ShowMicUI();
        SubscribeToOutHouseEvents(); // Subscribe to the out-house events when player enters
    }
    private void EventManager_Instance_OnPlayerOpensDoor(object sender, System.EventArgs e)
    {
        HideMicUi();
    }

    private void EventManager_Instance_OnPlayerCloseDoor(object sender, System.EventArgs e)
    {
        HideMicUi();
    }

    private void SubscribeToOutHouseEvents()
    {
        if (!isSubscribed)
        {
            EventManager.OnAnyOutHousePlayerEntered += EventManager_OnAnyOutHouseHasPlayer;
            EventManager.OnAnyOutHousePlayerExited += EventManager_OnAnyOutHousePlayerExited;
            isSubscribed = true;
        }
    }

    private void UnsubscribeFromOutHouseEvents()
    {
        if (isSubscribed)
        {
            EventManager.OnAnyOutHousePlayerEntered -= EventManager_OnAnyOutHouseHasPlayer;
            EventManager.OnAnyOutHousePlayerExited -= EventManager_OnAnyOutHousePlayerExited;
            isSubscribed = false;
        }
    }


    private void Start()
    {
        currentFillAmount = micSoundBar.fillAmount;
        targetFillAmount = currentFillAmount;
    }

    private void Update()
    {
        float newFillAmount = /*MicSoundDetector.Instance.GetOutputData()*/ micSoudData;

        // Update the target fill amount only if it changes
        if (targetFillAmount != newFillAmount)
        {
            targetFillAmount = newFillAmount;
        }

        // Interpolate the fill amount towards the target value
        currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
        micSoundBar.fillAmount = currentFillAmount;
    }

    private void ShowMicUI()
    {
        micSoundBarUi.gameObject.SetActive(true);
    }

    private void HideMicUi()
    {
        micSoundBarUi.gameObject.SetActive(false);
    }
}

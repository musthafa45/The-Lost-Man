using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour, IInteractable
{
    [SerializeField] private bool canPlaySong = true;
    private bool isPlaying = false;
    private bool IsStartedPlaying = false; // Avoid To Multiple Invokes
    private AudioSource audioSource;
    [SerializeField]private float volume = 0.3f;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
    }
    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Radio");

        ToggleRadio();
    }

    private void ToggleRadio()
    {
        if (!canPlaySong) return;

        isPlaying = !isPlaying;

        if(isPlaying && !IsStartedPlaying)
        {
            IsStartedPlaying = true;
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
            IsStartedPlaying = false;
        }
    }
}

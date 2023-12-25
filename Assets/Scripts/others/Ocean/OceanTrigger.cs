using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanTrigger : MonoBehaviour
{
    [SerializeField] private float audioLerpSpeed = 5f;
    [SerializeField] private float soundIntencityThreshold = 2f;
    [SerializeField] private float islandCenterToBeachRadius = 200f;
    [SerializeField] private Transform playerTransform;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        audioSource.Play();
    }

    private void Update()
    {
        if(playerTransform != null)
        {
            float playerToOceanDis = Vector3.Distance(transform.position, playerTransform.position);

            audioSource.volume = Mathf.Lerp(audioSource.volume,playerToOceanDis / islandCenterToBeachRadius / soundIntencityThreshold, audioLerpSpeed);

            //if (playerToOceanDis > islandCenterToBeachRadius)
            //{
            //    Debug.Log("Player Near To Ocean");
            //    if (!audioSource.isPlaying)
            //    {
            //        audioSource.Play();
            //        Debug.Log("Ocean Sound Started Play");
            //    }

            //}
            //else
            //{
            //    if (audioSource.isPlaying)
            //    {
            //        audioSource.Stop();
            //        Debug.Log("Ocean Sound Stopped Play");
            //    }
            //}

        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("Ocean Sound Started Play");
        }
    }
    private void OnDrawGizmos()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireSphere(transform.position, islandCenterToBeachRadius);
        }
    }

}

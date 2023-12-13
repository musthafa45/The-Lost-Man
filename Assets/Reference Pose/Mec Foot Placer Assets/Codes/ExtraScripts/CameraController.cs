using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

    [SerializeField] private Transform followHandTransform;
    [SerializeField] private float followCameraSpeed = 4f;

    private Transform playerTransform = null;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0,0.5f,0);
    private void Start()
    {
        playerTransform = FirstPersonController.Instance.transform;
    }
    private void Update()
    {
        if (followHandTransform != null && Torch.Instance.IsActive())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation , followHandTransform.rotation, followCameraSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = followHandTransform.rotation;
        }

        if(playerTransform != null)
        {
            transform.position = playerTransform.position + cameraOffset;
        }
    }
}

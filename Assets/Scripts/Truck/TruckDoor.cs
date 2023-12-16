using DG.Tweening;
using System;
using UnityEngine;

public class TruckDoor : MonoBehaviour, IInteractable
{
    private bool isFading = false;

    [SerializeField] private Vector3 doorOpenRot;
    [SerializeField] private Transform doorExitPointTr;

    private bool isDoorOpened = false;
    private Transform playerTransform = null;
    private void Start()
    {
        FadeController.Instance.SetSortingOrder(1);
        FadeController.Instance.FadeIn(0.5f, Color.black);
    }
    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Car Drive Called");
        playerTransform = interactorTransform;
        FirstPersonController.Instance.enabled = false;
        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isDoorOpened = !isDoorOpened;

        if(isDoorOpened)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        transform.DOLocalRotate(doorOpenRot, 0.3f).OnComplete(() =>
        {
            if (!isFading)
            {
                isFading = true;
                FadeController.Instance.FadeOut(0.5f, () =>
                {
                    Debug.Log("Fade Out Finished");
                    isFading = false;

                    FadeController.Instance.FadeIn(1.5f);

                    ToggleDoor();
                });
            }
        });
    }
    private void CloseDoor()
    {
        transform.DOLocalRotate(Vector3.zero, 0.3f).OnComplete(() =>
        {
            EventManager.Instance.InvokePlayerTryGetInTruck(playerTransform,this);
        });
    }

    public Transform GetLeaveWayTransform()
    {
        return doorExitPointTr;
    }
}

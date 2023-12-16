using DG.Tweening;
using UnityEngine;

public class TuckBackDoor : MonoBehaviour, IInteractable
{
    private bool isBackDoorOpen = false;

    private void Start()
    {
        CloseBackDoor();
    }
    public void Interact(Transform interactorTransform)
    {
        ToggleBackDoor();
    }

    private void ToggleBackDoor()
    {
        isBackDoorOpen = !isBackDoorOpen;

        if(isBackDoorOpen )
        {
            OpenBackDoor();
        }
        else
        {
            CloseBackDoor();
        }
    }

    private void CloseBackDoor()
    {
        transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.3f);
    }

    private void OpenBackDoor()
    {
        transform.DOLocalRotate(new Vector3(-90f, 0f, 0f), 0.3f);
    }
}

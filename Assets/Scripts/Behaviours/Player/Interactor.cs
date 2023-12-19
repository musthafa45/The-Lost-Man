using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    #region Interaction Variables
    [SerializeField] private Transform cameraTransform;                                     // fps Camera ref 
    [SerializeField] private float interactRange = 2f;         // interation range can be Modify
    [SerializeField] private LayerMask interactLayerMask;      // interaction Layer
    #endregion

    [SerializeField] private Image crossHairImage;
    private Vector3 cameraRayHitPoint;

    private void Start()
    {
        InputManager.Instance.inputActions.Player.InteractionKey.performed += InteractionKey_performed;
    }

    private void InteractionKey_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Fader.instance.DoFadeToBlack(() => Fader.instance.DoFadeToNormal());
        //FirstPersonController.Instance.PlayerYawControl();
        CastRay();
    }
    private void FixedUpdate()
    {
        CastRayUpdate();
    }

    // this method Just Detect Object with Validation Only Calls By Input
    private void CastRay() 
    {
        
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactRange, interactLayerMask)) return;

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(transform);
            }
        }
    }
    private void CastRayUpdate()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit2, 999f))
        {
            if(hit2.collider != null)
            {
                cameraRayHitPoint = hit2.point;
            }
            else
            {
                cameraRayHitPoint = cameraTransform.forward * 999f;
            }
        }

        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactRange, interactLayerMask))
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactRange,Color.green);
            if (crossHairImage.color != Color.white)
                crossHairImage.color = Color.white;
            return;
        }

        if (hit.collider != null)
        {
            crossHairImage.color = Color.red;
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactRange, Color.red);
        }

    }

    public Vector3 GetCameraRayHitPoint()
    {
        return cameraRayHitPoint;
    }
    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.InteractionKey.performed -= InteractionKey_performed;
    }

}

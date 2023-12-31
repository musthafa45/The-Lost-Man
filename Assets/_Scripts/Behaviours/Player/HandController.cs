//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace ProjectMiami
//{
//    public class HandController : MonoBehaviour
//    {
//        [SerializeField] private FirstPersonController fpsController;

//        [SerializeField] private Transform followCameraTransform;
//        [SerializeField] private float followCameraSpeed = 4f;

//        private void Update()
//        {
//            float cameraPitchX = fpsController.GetCameraPitchX();
//            float cameraPitchY = fpsController.GetCameraPitchY();

//            if (Torch.Instance.IsActive())
//            {
//                transform.localRotation = Quaternion.Euler(cameraPitchX, -cameraPitchY  , 0);
//            }
//            else
//            {
//                transform.localRotation = Quaternion.Euler(cameraPitchX, 0, 0);
//            }

//        }

//    }
//}
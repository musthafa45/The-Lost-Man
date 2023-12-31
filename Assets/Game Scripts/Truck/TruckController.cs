using System;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private Transform steeringWheelTransform;
    [SerializeField] private float steeringWheelRotAngle = 90f;  // 90 to -90

    private bool isMoving = false;
    private bool canMove = true;
    private bool isPlayerInside = false;

    private void OnEnable()
    {
        EventManager.Instance.OnPlayerGetsInTruck += (sender,e) =>
        {
            isPlayerInside = true;
        };


        EventManager.Instance.OnPlayerGetsOutTruck += (sender, e) =>
        {
            isPlayerInside = false;
        };

    }

    private void FixedUpdate()
    {
        if(isPlayerInside)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            UpdateSteeringWheel();
        }
        else
        {
            HandleHandBrake();
        }
    }

    private void UpdateSteeringWheel()
    {
        float rotationAmount = Mathf.Clamp(horizontalInput * steeringWheelRotAngle, -steeringWheelRotAngle, steeringWheelRotAngle);
        steeringWheelTransform.localRotation = Quaternion.Euler(0f, 0f, -rotationAmount);
    }


    private void HandleHandBrake()
    {
        frontRightWheelCollider.brakeTorque = Mathf.Infinity;
        frontLeftWheelCollider.brakeTorque = Mathf.Infinity;
        rearLeftWheelCollider.brakeTorque = Mathf.Infinity;
        rearRightWheelCollider.brakeTorque = Mathf.Infinity;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBreaking = Input.GetKey(KeyCode.Space);

        isMoving = horizontalInput != 0 || verticalInput != 0 ; 
    }

    private void HandleMotor()
    {
        if (canMove)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        else
        {
            ApplyBreaking();
        }

        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        if (!canMove) // Stop instantly if the truck can't move
        {
            frontRightWheelCollider.brakeTorque = Mathf.Infinity;
            frontLeftWheelCollider.brakeTorque = Mathf.Infinity;
            rearLeftWheelCollider.brakeTorque = Mathf.Infinity;
            rearRightWheelCollider.brakeTorque = Mathf.Infinity;
        }
        else // Apply regular braking force
        {
            frontRightWheelCollider.brakeTorque = currentbreakForce;
            frontLeftWheelCollider.brakeTorque = currentbreakForce;
            rearLeftWheelCollider.brakeTorque = currentbreakForce;
            rearRightWheelCollider.brakeTorque = currentbreakForce;
        }
    }


    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
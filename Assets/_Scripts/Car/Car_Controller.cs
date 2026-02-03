using System;
using UnityEngine;

public enum DriveType { FrontWheelDrive, BackWheelDrive, AllWheelDrive }

[RequireComponent(typeof(Rigidbody))]
public class Car_Controller : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public bool activateCar;

    private PlayerControls controls;
    private float moveInput;
    private float steerInput;

    public float speed;

    [Range(30, 60)]
    [SerializeField] private float turnSensetivity;
    [Header("Car Settings")]
    [SerializeField] private DriveType driveType;
    [SerializeField] private Transform centerOfMass;
    [Range(350, 1000)]
    [SerializeField] private float carMass = 400;
    [Range(20, 80)]
    [SerializeField] private float wheelsMass = 30;
    [Range(.5f, 2f)]
    [SerializeField] private float frontWheelTraction = 1;
    [Range(.5f, 2f)]
    [SerializeField] private float backWheelTraction = 1;

    [Header("Engine Settings")]
    [SerializeField] private float currentSpeed;
    [Range(7, 12)]
    [SerializeField] private float maxSpeed = 7;
    [Range(.5f, 10)]
    [SerializeField] private float accleerationSpeed = 2;
    [Range(1500, 6000)]
    [SerializeField] private float motorForce = 1500f;

    [Header("Brakes Settings")]
    [Range(0, 10)]
    [SerializeField] private float frontBrakeSensetivity = 5;
    [Range(0, 10)]
    [SerializeField] private float backBrakeSensetivity = 5;
    [Range(4000, 6000)]
    [SerializeField] private float brakePower = 5000;

    bool isBraking;

    [Header("Drift Settings")]
    [Range(0, 1)]
    [SerializeField] private float frontDriftFactor = .5f;
    [Range(0, 1)]
    [SerializeField] private float backDriftFactor = .5f;
    [Range(0, 1)]
    [SerializeField] private float driftDuration = 1;
    private float driftTimer;
    private bool isDrifting;

    private Car_Wheel[] wheels;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<Car_Wheel>(true);

        controls = ControlsManager.instance.controls;
        //ControlsManager.instance.SwitchToCarControl();

        ActivatedCar(false);

        AssignInputEvents();
        SetupDefaultValue();
    }

    private void Update()
    {
        if (!activateCar)
            return;

        speed = rb.linearVelocity.magnitude;

        // Stop drift
        driftTimer -= Time.deltaTime;
        if (driftTimer < 0)
            isDrifting = false;
    }

    private void FixedUpdate()
    {
        if (!activateCar)
            return;

        ApplyAnimateWheels();
        ApplyDrive();
        ApplySteering();
        ApplyBrakes();
        ApplySpeedLimit();

        if (isDrifting)
            ApplyDrift();
        else
            StopDrift();

    }

    public void ActivatedCar(bool active)
    {
        activateCar = active;

        if (active)
            rb.constraints = RigidbodyConstraints.None;
        else
            rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void SetupDefaultValue()
    {
        rb.centerOfMass = centerOfMass.localPosition;
        rb.mass = carMass;

        foreach (var wheel in wheels)
        {
            wheel.cd.mass = wheelsMass;

            if (wheel.axelType == AxelType.Front)
                wheel.SetDefaultStiffness(frontWheelTraction);

            if (wheel.axelType == AxelType.Back)
                wheel.SetDefaultStiffness(backWheelTraction);
        }
    }

    private void ApplyDrive()
    {
        currentSpeed = moveInput * accleerationSpeed * Time.fixedDeltaTime;

        float motorTorqueValue = motorForce * currentSpeed;

        foreach (var wheel in wheels)
        {
            if (driveType == DriveType.FrontWheelDrive)
            {
                if (wheel.axelType == AxelType.Front)
                    wheel.cd.motorTorque = motorTorqueValue;
            }
            else if (driveType == DriveType.BackWheelDrive)
            {
                if (wheel.axelType == AxelType.Back)
                    wheel.cd.motorTorque = motorTorqueValue;
            }
            else
                wheel.cd.motorTorque = motorTorqueValue;
        }
    }

    private void ApplySpeedLimit()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    private void ApplySteering()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Front)
            {
                float targetSteerAngle = steerInput * turnSensetivity;

                wheel.cd.steerAngle = Mathf.Lerp(wheel.cd.steerAngle, targetSteerAngle, .5f);
            }
        }
    }

    private void ApplyAnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rotation;
            Vector3 position;

            wheel.cd.GetWorldPose(out position, out rotation);

            if (wheel.model != null)
            {
                wheel.model.transform.position = position;
                wheel.model.transform.rotation = rotation;
            }
        }
    }

    private void ApplyBrakes()
    {
        foreach (var wheel in wheels)
        {
            bool frontWheel = wheel.axelType == AxelType.Front;
            float brakeSensetivity = frontWheel ? frontBrakeSensetivity : backBrakeSensetivity;

            float newBrakeTorque = brakePower * brakeSensetivity * Time.fixedDeltaTime;
            float currentBrakeTorque = isBraking ? newBrakeTorque : 0;
            wheel.cd.brakeTorque = currentBrakeTorque;
        }
    }

    private void ApplyDrift()
    {
        foreach (var wheel in wheels)
        {
            bool frontWheel = wheel.axelType == AxelType.Front;
            float driftFactor = frontWheel ? frontDriftFactor : backDriftFactor;

            WheelFrictionCurve sidewayFriction = wheel.cd.sidewaysFriction;

            sidewayFriction.stiffness *= (1 - driftFactor);
            wheel.cd.sidewaysFriction = sidewayFriction;
        }
    }

    private void StopDrift()
    {
        foreach (var wheel in wheels)
        {
            wheel.RestoreDefaultSideStiffness();
        }
    }

    private void AssignInputEvents()
    {
        controls.Car.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();

            moveInput = input.y;
            steerInput = input.x;
        };

        controls.Car.Move.canceled += ctx =>
        {
            moveInput = 0;
            steerInput = 0;
        };

        controls.Car.Brake.performed += ctx =>
        {
            isBraking = true;
            isDrifting = true;
            driftTimer = driftDuration;
        };

        controls.Car.Brake.canceled += ctx => isBraking = false;

        controls.Car.CarExit.performed += ctx => GetComponent<Car_Interaction>().GetOutOfTheCar();
    }

    [ContextMenu("Focus camera and enable")]
    public void TestThisCar()
    {
        activateCar = true;
        ThirdPersonCameraController.instance.ChangeCameraTarget(transform);
    }
}

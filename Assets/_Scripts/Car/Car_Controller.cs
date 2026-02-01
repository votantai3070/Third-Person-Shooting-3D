using System;
using UnityEngine;

public class Car_Controller : MonoBehaviour
{
    private PlayerControls controls;
    private Rigidbody rb;
    private float moveInput;
    private float steerInput;

    public float speed;

    [Range(30, 60)]
    [SerializeField] private float turnSensetivity;
    [Header("Car Settings")]
    [SerializeField] private Transform centerOfMass;

    [Header("Engine Settings")]
    [SerializeField] private float currentSpeed;
    [Range(7, 12)]
    [SerializeField] private float maxSpeed;
    [Range(.5f, 5)]
    [SerializeField] private float accleerationSpeed;
    [Range(1500, 3000)]
    [SerializeField] private float motorForce = 1500f;

    [Header("Brakes Settings")]
    [Range(4, 10)]
    [SerializeField] private float brakeSensetivity;
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

    private Car_Wheel[] wheels;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        controls = ControlsManager.instance.controls;
        ControlsManager.instance.SwitchToCarControl();

        wheels = GetComponentsInChildren<Car_Wheel>(true);

        AssignInputEvents();
    }

    private void Update()
    {
        speed = rb.linearVelocity.magnitude;

        // Stop drift
        driftTimer -= Time.deltaTime;
        if (driftTimer < 0)
            isBraking = false;
    }

    private void FixedUpdate()
    {
        ApplyAnimateWheels();
        ApplyDrive();
        ApplySteering();
        ApplyBrakes();
        AppltSpeedLimit();

        if (isBraking)
            ApplyDrift();
        else
            StopDrift();

    }


    private void ApplyDrive()
    {
        currentSpeed = moveInput * accleerationSpeed * Time.fixedDeltaTime;

        float motorTorqueValue = motorForce * currentSpeed;

        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Back)
                wheel.cd.motorTorque = motorTorqueValue;
        }
    }

    private void AppltSpeedLimit()
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
        float newBrakeTorque = brakePower * brakeSensetivity * Time.fixedDeltaTime;
        float currentBrakeTorque = isBraking ? newBrakeTorque : 0;

        foreach (var wheel in wheels)
        {
            if (wheel.axelType == AxelType.Back)
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
            driftTimer = driftDuration;
        };


        controls.Car.Brake.canceled += ctx => isBraking = false;
    }
}

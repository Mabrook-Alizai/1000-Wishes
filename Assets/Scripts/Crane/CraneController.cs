using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.ComponentModel;

public class CraneController : MonoBehaviour
{
    private CraneInputActions craneInputActions;

    [Header("References")]
    [SerializeField] private Transform craneGameObject;
    [SerializeField] private CinemachineVirtualCamera cinemachineCraneCamera;

    [Header("Angles")]
    [SerializeField] private float pitchAngle;
    [SerializeField] private float yawAngle;
    [SerializeField] private float rollAngle;

    [Header("Maneuvering Paramaters")]
    private float craneSpeed = 5f;
    private float pitch;
    private float yaw;
    private float roll;

    [Header("Rolling Parameters")]
    [SerializeField] private float rollSmoothness;
    private float currentRollAngle;

    [Header("WindZoneParameters")]
    [SerializeField] private float turbulenceModifier = 2f;
    [SerializeField] private float windSpeedModifier = 2f;
    [SerializeField] private float swayFrequencyX = 2f;
    [SerializeField] private float swayFrequencyY = 2f;
    [SerializeField] private float originalSpeed = 8f;
    private bool isInWindZone;
    private float swayX;
    private float swayY;

    [Header("Booster Parameters")]
    [SerializeField] private float speedBoosterMultiplier = 2f;

    [Header("Camera Boost Parameters")]
    [SerializeField] private float originalFOV = 60f;
    [SerializeField] private float boostFOV = 75f;
    [SerializeField] private float transitionSpeed = 2f;

    private float horizontalInput;
    private float verticalInput;
    

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        craneInputActions = new CraneInputActions();
    }

    private void OnEnable()
    {
        CollisionDetector.OnEnteringWindZone += CollisionDetector_OnEnteringWindZone;
        CollisionDetector.OnExitingWindZone += CollisionDetector_OnExitingWindZone;
        craneInputActions.CraneMovement.Enable();
    }

    private void OnDisable()
    {
        CollisionDetector.OnEnteringWindZone -= CollisionDetector_OnEnteringWindZone;
        CollisionDetector.OnExitingWindZone -= CollisionDetector_OnExitingWindZone;
    }

    private void Start()
    {
        craneSpeed = originalSpeed;
        isInWindZone = false;
    }

    private void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        transform.position += transform.forward * craneSpeed * Time.deltaTime;

        float currentSpeed = originalSpeed;
        float targetFOV = originalFOV;
        if (craneInputActions.CraneMovement.Boost.IsPressed())
        {
            currentSpeed = originalSpeed * speedBoosterMultiplier;
            targetFOV = boostFOV;
        }

        // Camera FOV changer, when boosted
        cinemachineCraneCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineCraneCamera.m_Lens.FieldOfView, targetFOV, transitionSpeed * Time.deltaTime);

        if (isInWindZone)
        {
            craneSpeed = currentSpeed / windSpeedModifier;

            swayX = Mathf.Sin(Time.time * swayFrequencyX) * turbulenceModifier;
            swayY = Mathf.Cos(Time.time * swayFrequencyY) * turbulenceModifier;

            transform.position += (transform.right * swayX + transform.up * swayY) * craneSpeed * Time.deltaTime;
        }
        else
        {
            craneSpeed = currentSpeed;
        }

        pitch += pitchAngle * verticalInput * Time.deltaTime;
        yaw += yawAngle * horizontalInput * Time.deltaTime;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        currentRollAngle = -horizontalInput * rollAngle;
        roll = Mathf.Lerp(roll, currentRollAngle, rollSmoothness * Time.deltaTime);
        craneGameObject.localRotation = Quaternion.Euler(0, 0, roll);
    }    

    private void CollisionDetector_OnEnteringWindZone()
    {
        isInWindZone = true;
        
    }

    private void CollisionDetector_OnExitingWindZone()
    {
        isInWindZone = false;
    }
}

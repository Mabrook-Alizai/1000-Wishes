using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    [SerializeField] private Transform craneGameObject;

    [Header("Angles")]
    [SerializeField] private float pitchAngle;
    [SerializeField] private float yawAngle;
    [SerializeField] private float rollAngle;

    [Header("Paramaters")]
    [SerializeField] private float craneSpeed = 5f;
    private float pitch;
    private float yaw;
    private float roll;

    [Header("Rolling Parameters")]
    [SerializeField] private float rollSmoothness;
    private float currentRollAngle;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.position += transform.forward * craneSpeed * Time.deltaTime;

        pitch += pitchAngle * verticalInput * Time.deltaTime;
        yaw += yawAngle * horizontalInput * Time.deltaTime;

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        currentRollAngle = -horizontalInput * rollAngle;
        roll = Mathf.Lerp(roll, currentRollAngle, rollSmoothness * Time.deltaTime);
        craneGameObject.localRotation = Quaternion.Euler(0, 0, roll);
    }
}

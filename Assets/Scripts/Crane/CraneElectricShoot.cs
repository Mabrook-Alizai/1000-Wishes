using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneElectricShoot : MonoBehaviour
{
    [SerializeField] private Transform craneShootPoint;
    [SerializeField] private float shootDistance = 30f;
    [SerializeField] private float chargeRate = 20f;

    private CraneInputActions craneInputActions;

    private void Awake()
    {
        craneInputActions = new CraneInputActions();
    }

    private void OnEnable()
    {
        craneInputActions.CraneMovement.Enable();
    }

    private void OnDisable()
    {
        craneInputActions.CraneMovement.Disable();
    }

    private void Update()
    {
        if (craneInputActions.CraneMovement.Shoot.IsPressed())
        {
            ShootElectricRay();
        }
    }
    
    private void ShootElectricRay()
    {
        Debug.DrawRay(craneShootPoint.position, craneShootPoint.forward * shootDistance, Color.red);

        if(Physics.Raycast(craneShootPoint.transform.position, craneShootPoint.forward, out RaycastHit hitInfo, shootDistance))
        {
            if(hitInfo.collider.TryGetComponent<PressurePlateTerminal>(out PressurePlateTerminal terminal))
            {
                terminal.RecieveCharge(chargeRate * Time.deltaTime);
            }
        }
    }
}

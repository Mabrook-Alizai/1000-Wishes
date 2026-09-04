using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGrabber : MonoBehaviour
{
    [SerializeField] private Transform rockHoldPoint;
    [SerializeField] private Transform planeTransform;

    private CraneInputActions craneInputActions;
    private GameObject currentlyHeldRock;

    [SerializeField] private float throwForce = 2f;
    [SerializeField] private float upwardModifier = 2f;

    public bool canGrab;
    public GameObject rockInZone;

    private void Awake()
    {
        craneInputActions = new CraneInputActions();
        craneInputActions.CraneMovement.Enable();
    }

    private void Update()
    {
        if(canGrab && craneInputActions.CraneMovement.Grab.triggered)
        {
            if (currentlyHeldRock != null) ReleaseRock();
            else if (currentlyHeldRock == null) GrabRock(rockInZone);
        }

        if(craneInputActions.CraneMovement.Throw.triggered && currentlyHeldRock != null)
        {
            ThrowRock();
        }

    }

    public void GrabRock(GameObject targetGrabbableRock)
    {
        currentlyHeldRock = targetGrabbableRock;
        currentlyHeldRock.transform.SetParent(rockHoldPoint, true);

        if(currentlyHeldRock.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        currentlyHeldRock.transform.localPosition = Vector3.zero;
    }

    private void ReleaseRock()
    {
        currentlyHeldRock.transform.SetParent(null);

        if(currentlyHeldRock.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        currentlyHeldRock = null;
    }

    private void ThrowRock()
    {
        currentlyHeldRock.transform.SetParent(null);

        if (currentlyHeldRock.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;

            Vector3 throwDirection = planeTransform.forward * throwForce;

            throwDirection += planeTransform.up * upwardModifier;

            rb.AddForce(throwDirection, ForceMode.Impulse);
        }

        currentlyHeldRock = null;

    }
}

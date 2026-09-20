using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    [SerializeField] private Transform itemHoldPoint;
    [SerializeField] private Transform planeTransform;

    private CraneInputActions craneInputActions;
    private GameObject currentlyHeldItem;

    [SerializeField] private float throwForce = 2f;
    [SerializeField] private float upwardModifier = 2f;

    public bool canGrab;
    public GameObject itemInZone;

    private void Awake()
    {
        craneInputActions = new CraneInputActions();
        craneInputActions.CraneMovement.Enable();
    }

    private void OnEnable()
    {
        MagnetActivator.OnMagnetDeactivated += MagnetActivator_OnMagnetDeactivated;
    }

    private void Update()
    {
        if(canGrab && craneInputActions.CraneMovement.Grab.triggered)
        {
            if (currentlyHeldItem != null) ReleaseItem();
            else if (currentlyHeldItem == null) GrabItem(itemInZone);
        }

        if(craneInputActions.CraneMovement.Throw.triggered && currentlyHeldItem != null)
        {
            ThrowItem();
        }

    }

    public void GrabItem(GameObject targetGrabbableItem)
    {
        currentlyHeldItem = targetGrabbableItem;
        currentlyHeldItem.transform.SetParent(itemHoldPoint, true);

        if(currentlyHeldItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        currentlyHeldItem.transform.localPosition = Vector3.zero;
    }

    private void ReleaseItem()
    {
        currentlyHeldItem.transform.SetParent(null);

        if(currentlyHeldItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        currentlyHeldItem = null;
    }

    private void ThrowItem()
    {
        currentlyHeldItem.transform.SetParent(null);

        if (currentlyHeldItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;

            Vector3 throwDirection = planeTransform.forward * throwForce;

            throwDirection += planeTransform.up * upwardModifier;

            rb.AddForce(throwDirection, ForceMode.Impulse);
        }

        currentlyHeldItem = null;

    }

    private void MagnetActivator_OnMagnetDeactivated()
    {
        if(currentlyHeldItem != null)
        {
            ReleaseItem();
        }
    }
}

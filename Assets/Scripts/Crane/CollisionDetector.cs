using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public static event Action OnCheckpointCrossed;
    public static event Action OnWishCollected;
    public static event Action OnMagnetCollected;
    public static event Action OnEnteringWindZone;
    public static event Action OnExitingWindZone;
    public static event Action OnElectricChargeCollected;

    private const string WISH = "Wish";
    private const string OBSTACLE = "Obstacle";
    private const string CHECKPOINT = "Checkpoint";
    private const string ROCK = "Rock";
    private const string MAGNET = "Magnet";
    private const string WIND_ZONE = "WindZone";
    private const string BATTERY = "Battery";
    private const string ELECTRIC_CHARGE = "ElectricCharge";

    [SerializeField] private CraneController craneController;
    [SerializeField] private ItemGrabber itemGrabber;
    private MagnetActivator magnetActivator;

    private CraneInputActions craneInputActions;
    

    private void Awake()
    {
        magnetActivator = GetComponent<MagnetActivator>();
        craneInputActions = new CraneInputActions();
        craneInputActions.CraneMovement.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(WISH))
        {
            Debug.Log("Wish Collected");
            OnWishCollected?.Invoke();
            other.gameObject.SetActive(false);
        }

        else if (other.CompareTag(OBSTACLE)){
            Debug.Log("Crashed");
            craneController.enabled = false;
        }

        else if (other.CompareTag(CHECKPOINT))
        {
            Debug.Log("Restoring Area ...");
            OnCheckpointCrossed?.Invoke();
        }

        else if (other.CompareTag(ROCK) || other.CompareTag(BATTERY))
        {
            if (magnetActivator.isMagnetActive) 
            { 
                itemGrabber.canGrab = true;
            }
            else
            {
                itemGrabber.canGrab = false;
            }

            itemGrabber.itemInZone = other.transform.root.gameObject;
        }

        else if (other.CompareTag(MAGNET))
        {
            other.gameObject.SetActive(false);
            OnMagnetCollected?.Invoke();
        }

        else if (other.CompareTag(WIND_ZONE))
        {
            Debug.Log("Entered Wind Zone");
            OnEnteringWindZone?.Invoke();
        }

        else if (other.CompareTag(ELECTRIC_CHARGE))
        {
            OnElectricChargeCollected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ROCK) || other.CompareTag(BATTERY))
        {
            itemGrabber.canGrab = false;
            itemGrabber.itemInZone = null;
        }

        else if (other.CompareTag(WIND_ZONE))
        {
            Debug.Log("Exit windzone");
            OnExitingWindZone?.Invoke();
        }
    }
}

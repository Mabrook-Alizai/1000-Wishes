using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public static event Action OnCheckpointCrossed;
    public static event Action OnWishCollected;
    public static event Action OnMagnetCollected;

    private const string WISH = "Wish";
    private const string OBSTACLE = "Obstacle";
    private const string CHECKPOINT = "Checkpoint";
    private const string ROCK = "Rock";
    private const string MAGNET = "Magnet";

    [SerializeField] private CraneController craneController;
    [SerializeField] private RockGrabber rockGrabber;
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

        else if (other.CompareTag(ROCK))
        {
            if (magnetActivator.isMagnetActive) rockGrabber.canGrab = true;
            else rockGrabber.canGrab = false;

            rockGrabber.rockInZone = other.gameObject;
        }

        else if (other.CompareTag(MAGNET))
        {
            other.gameObject.SetActive(false);
            OnMagnetCollected?.Invoke();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public static event Action OnCheckpointCrossed;
    public static event Action OnWishCollected;

    private const string WISH = "Wish";
    private const string OBSTACLE = "Obstacle";
    private const string CHECKPOINT = "Checkpoint";
    private const string ROCK = "Rock";

    [SerializeField] private CraneController craneController;
    [SerializeField] private RockGrabber rockGrabber;

    private CraneInputActions craneInputActions;
    

    private void Awake()
    {
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
            rockGrabber.canGrab = true;
            rockGrabber.rockInZone = other.gameObject;
        }
    }
}

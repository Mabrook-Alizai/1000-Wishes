using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    private CraneInputActions craneInputActions;
    [SerializeField] private CraneController craneController;

    private void Start()
    {
        craneInputActions = new CraneInputActions();
        craneInputActions.CraneMovement.Enable();
        craneController.enabled = false;
        gameObject.SetActive(true);

    }

    private void Update()
    {
        if (craneInputActions.CraneMovement.Start.triggered)
        {
            gameObject.SetActive(false);
            craneController.enabled = true;
        }
    }
}

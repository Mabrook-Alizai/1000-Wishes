using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneParticleEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem windTrails;
    [SerializeField] private ParticleSystem boostParticles;

    private CraneInputActions craneInputActions;

    private void Awake()
    {
        craneInputActions = new CraneInputActions();
    }

    private void OnEnable()
    {
        craneInputActions.CraneMovement.Enable();
        StartUI.OnCraneControllerEnabled += StartUI_OnCraneControllerEnabled;
    }

    private void OnDisable()
    {
        StartUI.OnCraneControllerEnabled -= StartUI_OnCraneControllerEnabled;
    }

    private void StartUI_OnCraneControllerEnabled()
    {
        windTrails.Play();
    }

    private void Update()
    {
        if (craneInputActions.CraneMovement.Boost.IsPressed())
        {
            if (!boostParticles.isPlaying)
            {
                boostParticles.Play();
            }
            
        }
        else
        {
            if(boostParticles.isPlaying)
            {
                boostParticles.Stop();
            }
        }
    }

}

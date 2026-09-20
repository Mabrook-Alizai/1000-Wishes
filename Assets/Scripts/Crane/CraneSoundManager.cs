using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;

public class CraneSoundManager : MonoBehaviour
{
    [Header("Audio Source References")]
    [SerializeField] private AudioSource craneWindAudioSource;
    [SerializeField] private AudioSource craneMagnetEffectAudioSource;

    [Header("Audio Clips References")]
    [SerializeField] private AudioClip magnetEffectSFX;

    private void OnEnable()
    {
        StartUI.OnCraneControllerEnabled += StartUI_OnCraneControllerEnabled;
        CollisionDetector.OnMagnetCollected += CollisionDetector_OnMagnetCollected;
    }

    

    private void OnDisable()
    {
        StartUI.OnCraneControllerEnabled -= StartUI_OnCraneControllerEnabled;
        CollisionDetector.OnMagnetCollected -= CollisionDetector_OnMagnetCollected;
    }

    private void StartUI_OnCraneControllerEnabled()
    {
        craneWindAudioSource.Play();
    }

    private void CollisionDetector_OnMagnetCollected()
    {
        craneMagnetEffectAudioSource.PlayOneShot(magnetEffectSFX);
    }
}

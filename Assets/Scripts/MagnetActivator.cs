using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MagnetActivator : MonoBehaviour
{
    public static event Action OnMagnetDeactivated;

    [Header("MagnetVFX Gameobject ref")]
    [SerializeField] private VisualEffect magnetVFX;

    public bool isMagnetActive;

    [SerializeField] private float magnetActiveWaitTime = 10f;
    private Coroutine magnetActiveTime;
    

    private void OnEnable()
    {
        CollisionDetector.OnMagnetCollected += CollisionDetector_OnMagnetCollected;
    }

    private void OnDisable()
    {
        CollisionDetector.OnMagnetCollected -= CollisionDetector_OnMagnetCollected;
    }

    private void Start()
    {
        isMagnetActive = false;
        magnetVFX.gameObject.SetActive(false);
    }

    private void CollisionDetector_OnMagnetCollected()
    {
        magnetVFX.gameObject.SetActive(true);

        if(magnetActiveTime != null)
        {
            StopCoroutine(magnetActiveTime);
        }

        magnetActiveTime = StartCoroutine(MagnetActiveWait());
        
    }

    private IEnumerator MagnetActiveWait()
    {
        isMagnetActive = true;
        yield return new WaitForSeconds(magnetActiveWaitTime);
        isMagnetActive = false;
        magnetVFX.gameObject.SetActive(false);

        OnMagnetDeactivated?.Invoke();

        magnetActiveTime = null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PressurePlateActivator : MonoBehaviour
{
    private const string BATTERY = "Battery";
    private const string BUTTON_PRESSED = "ButtonPressed";

    public static event Action OnBatterySnapped;

    [SerializeField] private Transform pressurePlateHoldPoint;
    [SerializeField] private Animator pressurePlateButtonAnimator;
    [SerializeField] private float snapSoomth = 2f;

    private Coroutine batterySnapping;
    private bool isPlaced = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlaced && other.CompareTag(BATTERY))
        {
            pressurePlateButtonAnimator.SetTrigger(BUTTON_PRESSED);

            if(batterySnapping != null)
            {
                StopCoroutine(batterySnapping);
            }
            batterySnapping = StartCoroutine(BatterySnapRoutine(other.gameObject));
        }
    }

    private IEnumerator BatterySnapRoutine(GameObject batteryGameObject)
    {
        isPlaced = true;

        if(batteryGameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        while(Vector3.Distance(batteryGameObject.transform.position, pressurePlateHoldPoint.transform.position) > 0.01f)
        {
            batteryGameObject.transform.position = Vector3.Lerp(batteryGameObject.transform.position, pressurePlateHoldPoint.transform.position, snapSoomth * Time.deltaTime);
            batteryGameObject.transform.rotation = Quaternion.Lerp(batteryGameObject.transform.rotation, pressurePlateHoldPoint.transform.rotation, snapSoomth * Time.deltaTime);

            yield return null;
        }

        batteryGameObject.transform.position = pressurePlateHoldPoint.transform.position;
        batteryGameObject.transform.rotation = pressurePlateHoldPoint.transform.rotation;

        Collider[] colliders = batteryGameObject.GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders)
        {
            if (collider.isTrigger)
            {
                collider.enabled = false;
            }
        }

        batterySnapping = null;
        Debug.Log("Snapped to place");
        OnBatterySnapped?.Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTerminal : MonoBehaviour
{
    public static event Action OnTerminalCharged;

    private float maxCharge = 100f;
    private float currentCharge = 0f;
    private bool isCharged;

    public void RecieveCharge(float chargeAmount)
    {
        if (isCharged) return;

        currentCharge += chargeAmount;

        Debug.Log("Charge Amount " + currentCharge);

        if(currentCharge >= maxCharge)
        {
            currentCharge = maxCharge;
            isCharged = true;
            OnTerminalCharged?.Invoke();
            Debug.Log("Charged");
        }
    }
}

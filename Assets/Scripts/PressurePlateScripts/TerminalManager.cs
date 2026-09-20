using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    private bool terminalCharged;
    private bool batterySnapped;

    private void Start()
    {
        terminalCharged = false;
        batterySnapped = false;
    }

    private void OnEnable()
    {
        PressurePlateActivator.OnBatterySnapped += PressurePlateActivator_OnBatterySnapped;
        PressurePlateTerminal.OnTerminalCharged += PressurePlateTerminal_OnTerminalCharged;
    }

    private void OnDisable()
    {
        PressurePlateActivator.OnBatterySnapped -= PressurePlateActivator_OnBatterySnapped;
        PressurePlateTerminal.OnTerminalCharged -= PressurePlateTerminal_OnTerminalCharged;
    }

    private void Update()
    {
        if(terminalCharged && batterySnapped)
        {
            Debug.Log("Opening Gate in 3 2 1");
        }
    }

    private void PressurePlateTerminal_OnTerminalCharged()
    {
        terminalCharged = true;
    }

    private void PressurePlateActivator_OnBatterySnapped()
    {
        batterySnapped = true;
    }
}

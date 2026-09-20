using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneElectricChargeManager : MonoBehaviour
{
    private float maxEnergy = 100f;
    private float currentEnergy = 0f;

    private void OnEnable()
    {
        CollisionDetector.OnElectricChargeCollected += CollisionDetector_OnElectricChargeCollected;
    }

    private void CollisionDetector_OnElectricChargeCollected()
    {
        
    }


}

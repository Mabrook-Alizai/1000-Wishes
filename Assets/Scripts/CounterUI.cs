using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wishCounterText;

    private int wishCounter = 0;

    private void OnEnable()
    {
        CollisionDetector.OnWishCollected += CollisionDetector_OnWishCollected;
        CollisionDetector.OnCheckpointCrossed += CollisionDetector_OnCheckpointCrossed;
    }

    private void CollisionDetector_OnCheckpointCrossed()
    {
        
    }

    private void CollisionDetector_OnWishCollected()
    {
        wishCounter++;
        wishCounterText.text = "Wishes: " + wishCounter;
    }
}

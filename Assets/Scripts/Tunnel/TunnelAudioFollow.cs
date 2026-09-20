using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelAudioFollow : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform craneTransform;
    [SerializeField] private Collider tunnelCollider;

    private void Update()
    {
        transform.position = tunnelCollider.ClosestPoint(craneTransform.position);
    }
}

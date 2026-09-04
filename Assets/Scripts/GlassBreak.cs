using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    private const string ROCK = "Rock";

    [Header("References")]
    [SerializeField] private GameObject glassSolid;
    [SerializeField] private GameObject glassShatter;
    [SerializeField] private Rigidbody[] rigidbodies;
    [SerializeField] private AudioClip glassBreaking;
    [SerializeField] private AudioSource glassBreakingSource;

    [Header("Shattering Parameters")]
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;

    private bool glassShattered;

    private Coroutine glassShardsDisappear;

    private void Start()
    {
        glassBreakingSource = GetComponent<AudioSource>();

        glassSolid.SetActive(true);

        foreach(Rigidbody rb in rigidbodies)
        {
            rb.gameObject.SetActive(false);
            rb.isKinematic = true;
        }

        glassShattered = false;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ROCK))
        {
            ShatterGlass();
        }
    }

    private void ShatterGlass()
    {
        glassShattered = true;

        Vector3 hitPos = glassSolid.transform.position;
        glassSolid.SetActive(!glassShattered);

        // disable the empty gameobject collider
        if(glassShatter.TryGetComponent<BoxCollider>(out BoxCollider bc))
        {
            bc.enabled = false;
        }

        foreach(Rigidbody rb in rigidbodies)
        {
            rb.gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, hitPos, explosionRadius);
        }

        glassBreakingSource.PlayOneShot(glassBreaking);

        if(glassShardsDisappear == null)
        {
            StartCoroutine(ShardDisappear());
        }
        if(glassShardsDisappear != null)
        {
            StopCoroutine(glassShardsDisappear);
        }
    }

    private IEnumerator ShardDisappear()
    {
        yield return new WaitForSeconds(5f);
        foreach(Rigidbody rb in rigidbodies)
        {
            rb.gameObject.SetActive(false);
        }
    }
}

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

    private bool glassHit;
    private bool glassShattered;
    private bool canHit;

    private Coroutine glassShardsDisappear;
    private Coroutine glassHitCooldown;

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
        glassHit = false;
        canHit = true;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;

        if (other.CompareTag(ROCK))
        {
            if(!glassHit && !glassShattered)
            {
                glassHit = true;
                GlassCrack();
                if(glassHitCooldown != null)
                {
                    StopCoroutine(glassHitCooldown);
                }
                glassHitCooldown = StartCoroutine(GlassHitCoolDown());
            }
            else if (glassHit)
            {
                ShatterGlass();
            }
        }
    }

    private void GlassCrack()
    {
        glassSolid.SetActive(false);

        foreach(Rigidbody rb in rigidbodies)
        {
            rb.gameObject.SetActive(true);
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

        if(glassShardsDisappear != null)
        {
            StopCoroutine(glassShardsDisappear);
        }

        glassShardsDisappear = StartCoroutine(ShardDisappear());
    }

    private IEnumerator ShardDisappear()
    {
        yield return new WaitForSeconds(5f);
        foreach(Rigidbody rb in rigidbodies)
        {
            rb.gameObject.SetActive(false);
        }
        glassShardsDisappear = null;
    }

    private IEnumerator GlassHitCoolDown()
    {
        canHit = false;
        yield return new WaitForSeconds(.3f);
        canHit = true;
        glassHitCooldown = null;
    }
}

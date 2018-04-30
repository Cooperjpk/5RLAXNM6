using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool interactableReady = true;
    public bool startActive = false;

    public Rigidbody rbody;

    [Header("Lifetime")]
    public float lifetime;
    [SerializeField]
    public float timeStamp;
    [SerializeField]
    public float zeroTime;

    [Header("Targetting")]
    public string targetTag = "Character";
    public string enviroTag = "Environment";

    [Header("Fire")]
    public GameObject onFireParticle;
    public float onFireParticleTime;
    public AudioClip onFireClip;
    public float onFireClipVolume;

    [Header("Destruction")]
    public bool destructUponTarget;
    public bool destructUponEnviro;
    public GameObject onDeathParticle;
    public float onDeathParticleTime;
    public AudioClip onDeathClip;
    public float onDeathClipVolume;

    [Header("SubInteractables")]
    public GameObject[] subInteractables;
    public Transform[] subIntTransform;

    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rbody = GetComponent<Rigidbody>();
        gameObject.SetActive(startActive);
    }

    public void Activate()
    {
        //Start the lifetime timer
        timeStamp = Time.time + lifetime;
        zeroTime = Time.time;
        //Set the projectile to fire so it can begin movement
        interactableReady = true;
        //On awake effects can happen here
        if (onFireParticle)
        {
            onFireParticle.SetActive(true);
            StartCoroutine(WaitTimeSetActive(onFireParticleTime, onFireParticle, false));
        }

        if (audioSource && onFireClip)
        {
            audioSource.clip = onFireClip;
            audioSource.PlayOneShot(onFireClip, onFireClipVolume);
        }
    }

    public void Destruct()
    {
        //On death effects can happen here
        if (onDeathParticle)
        {
            onDeathParticle.SetActive(true);
            StartCoroutine(WaitTimeSetActive(onDeathParticleTime, onDeathParticle, false));
        }

        if (audioSource && onDeathClip)
        {
            audioSource.clip = onDeathClip;
            audioSource.PlayOneShot(onDeathClip, onDeathClipVolume);
        }

        if (subInteractables.Length > 0)
        {
            for (int i = 0; i < subInteractables.Length; i++)
            {
                Instantiate(subInteractables[i], subIntTransform[i].position, subIntTransform[i].rotation);
            }
        }
        //Needs to be the last thing to occur
        interactableReady = true;
        gameObject.SetActive(false);
    }

    public IEnumerator WaitTimeSetActive(float time, GameObject obj, bool setActive)
    {
        yield return new WaitForSeconds(time);
        //Debug.Log("YA BUSTED BITCH");
        obj.SetActive(setActive);
    }

    /// <summary>
    /// Use '0' for no minRange to be applied to targets.
    /// </summary>
    /// <param name="minRange"></param>
    /// <returns></returns>
    public GameObject ClosestTarget(float minRange)
    {
        List<GameObject> gos = new List<GameObject>();

        foreach (GameObject gObject in GameObject.FindGameObjectsWithTag(targetTag))
        {
            gos.Add(gObject);
        }

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        if (closest != null)
        {
            if(minRange > 0)
            {
                if (Vector3.Distance(transform.position, closest.transform.position) <= minRange)
                {
                    return closest;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return closest;
            }
        }
        else
        {
            return null;
        }
    }

}

using UnityEngine;
using UnityEditor;
using System.Collections;

public class Projectile : Interactable
{
    [Header("Direct Damage")]
    public int damage;
    public AnimationCurve damageCurve;

    [Header("Splash Damage")]
    public bool splash;
    public float splashRadius = 1;
    public AnimationCurve splashRadiusCurve;
    public int splashDamage;
    public AnimationCurve splashDamageCurve;

    [Header("Speed")]
    public AnimationCurve speedCurve;
    public float speed = 1;

    public enum HomingType
    {
        None,
        SemiHoming,
        Homing
    }

    [Header("Homing")]
    public HomingType homingType;
    public float homingRadius;
    public float semiHomingTurnSpeed;

    [Header("Horizontal Movement")]
    public float horizontalMoveSpeed = 0.1f;
    public AnimationCurve horizontalMoveCurve;

    [Header("Vertical Movement")]
    public float verticalMoveSpeed = 0.1f;
    public AnimationCurve verticalMoveCurve;

    void Update()
    {
        if (homingType != HomingType.None)
        {
            //Add the functionality to look the target here
        }

        if (timeStamp >= Time.time)
        {
            //Move the projectile depending on current time in lifetime
            float timeProgress = (Time.time - zeroTime) / (timeStamp - zeroTime);

            float currentSpeed = speedCurve.Evaluate(timeProgress) * speed;
            float currentHoriSpeed = horizontalMoveCurve.Evaluate(timeProgress) * horizontalMoveSpeed;
            float currentVertSpeed = verticalMoveCurve.Evaluate(timeProgress) * verticalMoveSpeed;

            //transform.Translate(new Vector3(currentHoriSpeed,currentVertSpeed,currentSpeed));
            //Vector3 directionalSpeed = new Vector3(currentHoriSpeed, currentVertSpeed, 0);
            Vector3 horiSpeed = transform.right * currentHoriSpeed;
            Vector3 vertSpeed = transform.up * currentVertSpeed;
            Vector3 forwardSpeed = transform.forward * currentSpeed;
            Vector3 totalSpeed = horiSpeed + vertSpeed + forwardSpeed;

            //Apply movement
            rbody.velocity = totalSpeed;
            //transform.Translate(new Vector3(0, 0, speed));
        }
        //If the lifetime is up on a projectile then it is no longer firing
        if (timeStamp < Time.time)
        {
            Destruct();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //Check if this is a desired target
        if (col.gameObject.tag == targetTag)
        {
            float timeProgress = (Time.time - zeroTime) / (timeStamp - zeroTime);
            //Debug.Log (timeProgress);

            float currentDamageAmount = damageCurve.Evaluate(timeProgress);
            //Debug.Log (currentDamageAmount);
            int currentDamage = Mathf.RoundToInt(currentDamageAmount * damage);
            //Debug.Log (currentDamage);
            //Apply direct damage here
            col.gameObject.GetComponent<Statistics>().ChangeHealth(currentDamage);
            //Go to destroyed state here if it is destructible by target
            if (destructUponTarget)
            {
                KillProjectile();
                Destruct();
            }
        }
        else if (col.gameObject.tag == enviroTag)
        {
            if (destructUponEnviro)
            {
                KillProjectile();
                Destruct();
            }
        }
    }

    void KillProjectile()
    {
        if (splash)
        {
            //Calculate the size of the splash radius depending on the lifetime
            float timeProgress = (Time.time - zeroTime) / (timeStamp - zeroTime);
            float currentRadiusAmount = splashRadiusCurve.Evaluate(timeProgress);
            float currentRadius = currentRadiusAmount * splashRadius;

            Collider[] splashHit;
            splashHit = Physics.OverlapSphere(transform.position, currentRadius);
            foreach (Collider other in splashHit)
            {

                if (other.CompareTag(targetTag))
                {

                    //Calculate the damage based on how close the target was to the center
                    float distanceToCenter = Vector3.Distance(other.transform.position, transform.position);
                    float distanceAmount = distanceToCenter / currentRadius;
                    float currentSplashDamageAmount = splashDamageCurve.Evaluate(distanceAmount);
                    int currentSplashDamage = Mathf.RoundToInt(currentSplashDamageAmount * splashDamage);

                    //Deal splash damage here
                    other.gameObject.GetComponent<Statistics>().ChangeHealth(currentSplashDamage);
                }
            }
        }
    }

}

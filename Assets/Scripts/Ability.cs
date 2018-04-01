using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ability : MonoBehaviour
{
    /// <summary>
    /// Ability class will have a huge amount of customization and all abilities will inherit from this class. Using the information provided by ability to carry out their abilities. If any new functionality is added, it should be done in the ability class and not the inheriting class.
    /// </summary>

    public bool activeWhileHolding = false;
    public float activeHoldingTick;
    float activeNextTick;

    //Button Interaction Types
    public enum InteractionType
    {
        Cooldown,
        Ammo,
        Charged,
    }

    public InteractionType interactionType;

    public enum AbilityInput
    {
        Down,
        Hold,
        Up,
    }

    //Interactable Pool
    private Interactable[] interactablePool;
    private int currentInteractable;

    //Cooldown
    float cooldownStamp;
    public float cooldownTime;

    public void InvokeAbility(AbilityInput abilityInput)
    {
        //Do different things depending on the if this was pressed, held or released.
        switch(abilityInput)
        {
            default:
                {
                    Debug.LogError("The ability input has not been given correctly.");
                    return;
                }
            case AbilityInput.Down:
                {
                    AbilityDown();
                    return;
                }
            case AbilityInput.Hold:
                {
                    AbilityHold();
                    return;
                }
            case AbilityInput.Up:
                {
                    AbilityUp();
                    return;
                }
        }
    }

    public virtual void AbilityDown()
    {
        Debug.Log("Ability was pressed!");
    }

    public virtual void AbilityHold()
    {
        Debug.Log("Ability is being held!");
    }

    public virtual void AbilityUp()
    {
        Debug.Log("Ability was released!");
    }

    //Ability Functions
    //Interactable Pools
    /// <summary>
    /// Instantiate an array that is a pool of interactables, poolSize long.
    /// </summary>
    /// <param name="poolSize"></param>
    public void InteractablePoolInstantiate(Interactable interactable, int poolSize, Interactable poolInteractable, bool transformChild)
    {
        interactablePool = new Interactable[poolSize];

        for (int j = 0; j < poolSize; j++)
        {
            Interactable inter = Instantiate(interactable, transform.position, transform.rotation) as Interactable;
            interactablePool[j] = inter;
            if(transformChild)
            {
                inter.transform.parent = transform; //This will make the interactables all children of the weapon
            }
        }
    }

    /// <summary>
    /// Activate the next interactable in the interactable pool.
    /// </summary>
    public void InteractableActivate(Transform firepoint)
    {
        //Find next interactable in pool
        for (int i = 0; i < interactablePool.GetLength(0); i++)
        {
            if (interactablePool[i].interactableReady)
            {
                //Debug.Log(interactablePool[i].name + " is ready to fire!");
                currentInteractable = i;
                break;
            }
        }

        //Activate and reset the interactable
        interactablePool[currentInteractable].gameObject.SetActive(true);
        interactablePool[currentInteractable].Activate();
        interactablePool[currentInteractable].transform.position = firepoint.position;
        interactablePool[currentInteractable].transform.rotation = firepoint.rotation;
        //Set the interactable to not ready in the interactable
        interactablePool[currentInteractable].interactableReady = false;
    }

    //Cooldowns
    /// <summary>
    /// Returns true if time stamp is less than current time.
    /// </summary>
    /// <returns></returns>
    public bool CooldownReady()
    {
        //Cooldown Logic goes here
        if(cooldownStamp <= Time.time)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Set the new time stamp for cooldown that is time away.
    /// </summary>
    public void CooldownSet(float time)
    {
        cooldownStamp = Time.time + time;
    }

    //Charges
    //Ammo & Reloading
    //Channeling
}




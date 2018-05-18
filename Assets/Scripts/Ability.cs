using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Ability class will have a huge amount of customization and all abilities will inherit from this class. Using the information provided by ability to carry out their abilities. If any new functionality is added, it should be done in the ability class and not the inheriting class.
/// </summary>
public class Ability : MonoBehaviour
{
    public enum AbilityInput
    {
        Down,
        Hold,
        Up,
    }

    //Interactable Pool
    private Interactable[] interactablePool;
    private int currentInteractable;

    //Ammo
    private int ammoCount;
    public int AmmoCount
    {
        get
        {
            return ammoCount;
        }
        set
        {
            ammoCount = value;
        }
    }
    private int ammoMax;
    public int AmmoMax
    {
        get
        {
            return ammoMax;
        }
        set
        {
            ammoMax = value;
        }
    }

    //Timeframes
    private float timeRate = 1;
    public float TimeRate
    {
        get
        {
            return timeRate;
        }
        set
        {
            timeRate = value;
        }
    }
    private float cooldownStamp;
    public float CooldownStamp
    {
        get
        {
            return cooldownStamp;
        }
        set
        {
            cooldownStamp = value;
        }
    }
    private float cooldownTime;
    public float CooldownTime
    {
        get
        {
            return cooldownTime;
        }
        set
        {
            cooldownTime = value;
        }
    }

    public void InvokeAbility(AbilityInput abilityInput)
    {
        //Do different things depending on the if this was pressed, held or released.
        switch (abilityInput)
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
            if (transformChild)
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

    //Charges & Channels
    /// <summary>
    /// Change the current value of ammoCount by the value of count.
    /// </summary>
    /// <param name="ammoCount"></param>
    public void AmmoSet(int count)
    {
        AmmoCount += count;
    }

    /// <summary>
    /// Sets the value of ammoCount to ammoMax.
    /// </summary>
    public void AmmoReload()
    {
        AmmoCount = AmmoMax;
    }

    //Timeframes
    /// <summary>
    /// Start the ticker.
    /// </summary>
    public void TimeSet(float time)
    {
        TimeStart();
        CooldownStamp = CooldownTime + Time.time;
        StartCoroutine(Ticker(TimeRate));
    }

    /// <summary>
    /// Stops the ticker coroutine.
    /// </summary>
    public void TimeInterrupt()
    {
        TimeStop();
        StopCoroutine(Ticker(TimeRate));
    }

    /// <summary>
    /// Called everytime that the time is started.
    /// </summary>
    public virtual void TimeStart()
    {
        Debug.Log("Timestamp started!");
    }

    /// <summary>
    /// Called everytime that the time is interrupted.
    /// </summary>
    public virtual void TimeStop()
    {
        Debug.Log("Timestamp interrupted!");
    }

    /// <summary>
    /// Called everytime that the time is ended.
    /// </summary>
    public virtual void TimeEnd()
    {
        Debug.Log("Timestamp ended!");
    }

    /// <summary>
    /// Called every frame that the time is still ticking.
    /// </summary>
    public virtual void TimeTicking()
    {
        Debug.Log("Timestamp ticking!");
    }

    private IEnumerator Ticker(float rate)
    {
        while (CooldownStamp <= Time.time)
        {
            TimeTicking();
            yield return new WaitForSeconds(rate);
        }
        TimeEnd();
    }


}




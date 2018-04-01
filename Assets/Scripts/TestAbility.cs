using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : Ability {

    public Interactable interactable;
    public Transform firepoint;
    public int interactablePoolSize;

    //Fire a projectile that has a cooldown of 5 seconds
    void Start()
    {
        //Instantiate the pool of projectiles to use in ability.
        InteractablePoolInstantiate(interactable,interactablePoolSize,interactable,false);
        CooldownSet(0);
    }

    public override void AbilityDown()
    {
        //Debug.Log("Test ability was pressed!");
        if (CooldownReady())
        {
            //Debug.Log("Test ability is fired");
            InteractableActivate(firepoint);
            CooldownSet(cooldownTime);
        }
    }

    public override void AbilityHold()
    {
        //Debug.Log("Test ability is being held!");
        if (CooldownReady())
        {
            //Debug.Log("Test ability is fired");
            InteractableActivate(firepoint);
            CooldownSet(cooldownTime);
        }
    }

    public override void AbilityUp()
    {
        //Debug.Log("Test ability was released!");
    }

}

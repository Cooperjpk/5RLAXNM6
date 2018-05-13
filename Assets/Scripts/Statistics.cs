using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Statistics : MonoBehaviour
{
    //TO DO
    //Add in the functioanlity for status effects to be added.
    //https://forum.unity.com/threads/rpg-buffs-and-debuffs.188882/
    //http://theliquidfire.com/2015/09/14/tactics-rpg-status-effects/

    //Add shield functionality. Waiting on a "isincombat" bool from controller.
    //Make the speed drawn from here.

    //Types of Health
    public int currentArmor;
    public int totalArmor;

    public int currentShield;
    public int totalShield;

    public int currentHealth;
    public int totalHealth;

    //Armor
    public float armorRecoverFraction;
    public float armorDamageFraction;
    public int currentRecoverArmor;
    public float armorDecayTime;
    public int armorDecayAmount;
    private bool armorRecoverDecaying = false;

    [Range(-1, 1)]
    public float damageMultiplier;
    [Range(-1, 1)]
    public float healingMultiplier;

    //Status Effects
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    void Start()
    {
        ResetHealth();
    }

    //Apply Effects
    public void ApplyEffect()
    {

    }

    void TriggerEffect()
    {

    }

    void EndEffect()
    {

    }

    //Change Health
    public void ChangeHealth(int value)
    {
        if(Utility.IsPositive(value))
        {
            int remainingValue = value;

            //Health
            if (remainingValue > 0)
            {
                int displacedHealth = totalHealth - currentHealth;
                currentHealth += Mathf.CeilToInt(remainingValue * healingMultiplier);
                remainingValue -= displacedHealth;
                if (currentHealth > totalHealth)
                {
                    currentHealth = totalHealth;
                }
            }

            //Shield
            if (remainingValue > 0)
            {
                int displacedShield = totalShield - currentShield;
                currentShield += Mathf.CeilToInt(remainingValue * healingMultiplier);
                remainingValue -= displacedShield;
                if (currentShield > totalShield)
                {
                    currentShield = totalShield;
                }
            }

            //Armor
            if (remainingValue > 0)
            {
                int displacedArmor = totalArmor - currentArmor;
                currentArmor += Mathf.CeilToInt(remainingValue * healingMultiplier);
                remainingValue -= displacedArmor;
                if (currentArmor > totalArmor)
                {
                    currentArmor = totalArmor;
                }
            }
        }
        else if(Utility.IsNegative(value))
        {
            int remainingValue = value;

            //Armor
            if (remainingValue > 0)
            {
                int lastArmor = currentArmor;
                remainingValue = Mathf.CeilToInt(remainingValue * damageMultiplier);
                currentArmor -= remainingValue;
                if (currentArmor <= 0)
                {
                    currentArmor = 0;
                    CheckDeath();
                }
                remainingValue -= (lastArmor - currentArmor);
                RecoveryGain(remainingValue);
            }
            //Shield
            if (remainingValue > 0)
            {
                int lastShield = currentShield;
                remainingValue = Mathf.CeilToInt(remainingValue * damageMultiplier);
                currentShield -= remainingValue;
                if (currentShield <= 0)
                {
                    currentShield = 0;
                    CheckDeath();
                }
                remainingValue -= (lastShield - currentShield);
            }
            //Health
            if (remainingValue > 0)
            {
                int lastHealth = currentHealth;
                remainingValue = Mathf.CeilToInt(remainingValue * damageMultiplier);
                currentHealth -= remainingValue;
                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    CheckDeath();
                }
                remainingValue -= (lastHealth - currentHealth);
            }
        }
    }

    //Check for Death
    void CheckDeath()
    {
        if (currentArmor <= 0 && currentShield <= 0 && currentHealth <= 0)
        {
            Death();
        }
    }

    //Become Dead
    void Death()
    {
        Debug.Log("Character should be dead bruh.");
    }

    //Set current health to total healths.
    void ResetHealth()
    {
        currentArmor = totalArmor;
        currentShield = totalShield;
        currentHealth = totalHealth;
    }

    //Based on the damage, gain recovery armor that decays overtime.
    void RecoveryGain(int damageDealt)
    {
        //Begin losing this recovery armor overtime if it is already not happening.
        if (!armorRecoverDecaying)
        {
            //New Currect Recover Armor equals a fraction of the damage dealt.
            currentRecoverArmor = currentArmor + Mathf.CeilToInt(damageDealt * armorDamageFraction);

            armorRecoverDecaying = true;
            Invoke("RecoveryDecay", armorDecayTime);
        }
        else if (armorRecoverDecaying)
        {
            Debug.Log("Not changing the decay because its already been started");
        }
    }

    void RecoveryDecay()
    {
        //Reduce recover armor by the decay amount.
        currentRecoverArmor -= armorDecayAmount;
        //If there is more recover armor than actual armor, tick down again at decay time.
        if (currentRecoverArmor > currentArmor)
        {
            Invoke("RecoveryDecay", armorDecayTime);
        }
        else
        {
            armorRecoverDecaying = false;
        }
    }

    public void RecoveryDamage(int damageDealt)
    {
        int recoveryAmount = Mathf.CeilToInt(damageDealt * armorRecoverFraction);
        if (recoveryAmount <= currentRecoverArmor - currentArmor)
        {
            currentArmor += recoveryAmount;
        }
        else
        {
            currentArmor = currentRecoverArmor;
        }
    }

}

#region Editor
[CustomEditor(typeof(Statistics))]
public class StatsEditor : Editor
{
    public int healthValue;
    public int damageValue;
    public int damageDealt;
    public bool showCurrent = false;

    public override void OnInspectorGUI()
    {
        Statistics stats = (Statistics)target;

        showCurrent = EditorGUILayout.ToggleLeft("Show Current Health", showCurrent);
        EditorGUILayout.Space();
        stats.totalArmor = EditorGUILayout.IntField("Total Armor", stats.totalArmor);
        if (showCurrent)
        {
            stats.currentArmor = EditorGUILayout.IntSlider("Current Armor", stats.currentArmor, 0, stats.totalArmor);
        }
        stats.totalShield = EditorGUILayout.IntField("Total Shield", stats.totalShield);
        if (showCurrent)
        {
            stats.currentShield = EditorGUILayout.IntSlider("Current Shield", stats.currentShield, 0, stats.totalShield);
        }
        stats.totalHealth = EditorGUILayout.IntField("Total Health", stats.totalHealth);
        if (showCurrent)
        {
            stats.currentHealth = EditorGUILayout.IntSlider("Current Health", stats.currentHealth, 0, stats.totalHealth);
        }
        EditorGUILayout.Space();
        stats.damageMultiplier = EditorGUILayout.Slider("Damage Multiplier", stats.damageMultiplier, 0.1f, 2);
        stats.healingMultiplier = EditorGUILayout.Slider("Healing Multiplier", stats.healingMultiplier, 0.1f, 2);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Armor", EditorStyles.boldLabel);
        stats.armorDecayAmount = EditorGUILayout.IntField("Armor Decay Amount", stats.armorDecayAmount);
        stats.armorDecayTime = EditorGUILayout.FloatField("Armor Decay Time", stats.armorDecayTime);
        stats.armorDamageFraction = EditorGUILayout.FloatField("Armor Damage Fraction", stats.armorDamageFraction);
        stats.armorRecoverFraction = EditorGUILayout.FloatField("Armor Recover Fraction", stats.armorRecoverFraction);
        stats.currentRecoverArmor = EditorGUILayout.IntSlider("Recover Armor", stats.currentRecoverArmor, 0, stats.totalArmor);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Health Tests", EditorStyles.boldLabel);
        healthValue = EditorGUILayout.IntField("Health Value", healthValue);
        if (GUILayout.Button("Health Value", EditorStyles.miniButtonMid))
        {
            stats.ChangeHealth(healthValue);
        }
        damageDealt = EditorGUILayout.IntField("Damage Value", damageDealt);
        if (GUILayout.Button("Do Damage", EditorStyles.miniButtonMid))
        {
            stats.RecoveryDamage(damageDealt);
        }

    }
}
#endregion


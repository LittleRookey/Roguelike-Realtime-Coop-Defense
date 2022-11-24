using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

public enum eAbilityType
{
    Dash
}

[CreateAssetMenu(menuName = "Litkey/Ability/base")]
public class Ability : ScriptableObject
{
    [Header("Debug")]
    public bool showLog;
    
    [Header("Main Ability Settings")]
    public bool isPlayer; // is player's ability
    public new string name; // ability name
    public float coolDownTime; // cooldown time for ability
    public float activeTime; // time to start cooldown after ability is activated, normally runs until the player's on key up event
    [TextArea]
    public string description; // description of the ability
    public eAbilityType abilityType; // type of ability 
    public KeyCode key; // ability use key 
    public bool Instantaneous; // will allow to run OnAbilityRunning even when player on key up

    [SerializeField] protected bool cantMoveWhileAbilityIsActive; // allows player to move while using ability
    [SerializeField] protected bool cantMoveWhileChanting; // allows player to move while using ability
    protected bool isUsingAbility; // when player is holding on a key(charge?), 

    [HideInInspector] public bool isEnded; // boolean value to immediately end the active time of ability and run into cooldown
    [Header("Chant setting")]
    public bool useChant;
    [SerializeField] public Chant chant;
    [HideInInspector] public bool chantDone;


    bool UnlockMovement = false;

    public Ability Clone() 
    {
        Ability ab = new Ability();
        ab.name = name;
        ab.coolDownTime = coolDownTime;
        ab.description = description;
        ab.abilityType = abilityType;
        ab.key = key;
        return ab;
    }

    /// <summary>
    /// callback event ran when ability starts
    /// </summary>
    /// /// <param name="parent">the gameObject Ability Holder is attached to</param>
    public virtual void OnAbilityStart(GameObject parent) 
    {
        if (showLog)
        {
            Debug.Log(name + " Ability Started");
        }
        chantDone = false;
        isUsingAbility = true;
        isEnded = false;
        if (cantMoveWhileAbilityIsActive || cantMoveWhileChanting)
        {
            PlayerMovement pm = parent.GetComponent<PlayerMovement>();
            // run animation to skill state
            pm.SetIdle();
            pm.canMove = false;

            UnlockMovement = false;
        }
        
    }

    public virtual async Task<bool> OnChantStart(GameObject parent)
    {
        if (!useChant) return false;
        chant.CreateChant(parent);
        await chant.ReadChant();
        return true;
    }


    /// <summary>
    /// callback event ran when player is holding a key
    /// </summary>
    /// /// <param name="parent">the gameObject Ability Holder is attached to</param>
    public virtual void OnAbilityRunning(GameObject parent)
    {
        if (showLog)
        {
            Debug.Log(name + " Ability Running");
        }
        if (cantMoveWhileChanting && !UnlockMovement)
        {
            // is entered when chanting is over
            PlayerMovement pm = parent.GetComponent<PlayerMovement>();
            pm.canMove = true;
            UnlockMovement = true;
        }
    }

    /// <summary>
    /// callback event ran when ability ended
    /// </summary>
    /// /// <param name="parent">the gameObject Ability Holder is attached to</param>
    public virtual void OnAbilityEnd(GameObject parent) 
    {
        if (showLog)
        {
            Debug.Log(name + " Ability Ended");
        }
        if (cantMoveWhileAbilityIsActive)
        {
            PlayerMovement pm = parent.GetComponent<PlayerMovement>();
            pm.canMove = true;
        }
        isUsingAbility = false;
    }

    


    protected bool IsOnCooldown()
    {
        return coolDownTime > 0f;
    }

    public Chant StartChant(GameObject go)
    {
        if (useChant)
        {
            chant.CreateChant(go);
        }
        return chant;
    }
}

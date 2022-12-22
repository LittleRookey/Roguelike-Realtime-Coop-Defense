using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using DG.Tweening;
using Litkey.Utility;
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
    [SerializeField] protected GameObject onAbilityStartVFX;
    [SerializeField] protected Vector3 onAbilityStartVFXOffset;

    protected GameObject onAbilityStartVFXCopy;

    [HideInInspector] public bool chantDone;
    [HideInInspector] public bool runChantEnd;

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
            //pm.SetIdle();
            //pm.anim.SetBool("isRunning", false);
            pm.canMove = false;

            UnlockMovement = false;
        }



    }

    // must call this in child class
    public virtual async Task<bool> OnChantStart(GameObject parent)
    {
        if (!useChant) return false;

        runChantEnd = false;

        // initializes chant object
        chant.CreateChant(parent);

        // stops attack if chant.stopattacking is true
        parent.TryGetComponent<UnitAttack>(out UnitAttack unitAttack);
        if (unitAttack != null)
        {
            if (chant.notAttackWhileChanting)
                unitAttack.stopAttacking = true;
        }

        // makes character look up animation
        parent.TryGetComponent<PlayerAnimatorController>(out PlayerAnimatorController playerAnimController);
        if (playerAnimController != null)
        {
            playerAnimController.LookUp(true);
        }

        // spawns ability start vfx, magic circle
        if (onAbilityStartVFXCopy == null && onAbilityStartVFX != null)
        {
            onAbilityStartVFXCopy = Instantiate(onAbilityStartVFX, parent.transform.position + (Vector3)onAbilityStartVFXOffset, onAbilityStartVFX.transform.rotation, parent.transform);

        }

        // if ability start vfx exists, run effect
        if (onAbilityStartVFXCopy != null)
            Effects.ScaleUpMagicCircle(onAbilityStartVFXCopy, 0.5f, chant.CalculateChantTimeInSec() + 1f);

        //TurnOnAbilityStartVFX((abilityStartVFX) =>
        //{
        //    abilityStartVFX.transform.localScale = Vector3.zero;
        //    float scale = 0f;
        //    DOTween.To(() => scale, x => scale = x, 0.5f, chant.CalculateChantTimeInSec())
        //        .OnUpdate(() =>
        //        {
        //            abilityStartVFX.transform.localScale = Vector3.one * scale;
        //        })
        //        .OnComplete(() => abilityStartVFX.gameObject.SetActive(false));
        //});

        await chant.ReadChant();
        return true;
    }

    public virtual void OnChantEnd(GameObject parent)
    {
        runChantEnd = true;
        parent.TryGetComponent<PlayerAnimatorController>(out PlayerAnimatorController playerAnimController);
        if (playerAnimController != null)
        {
            playerAnimController.LookUp(false);
        }

        parent.TryGetComponent<UnitAttack>(out UnitAttack unitAttack);
        if (unitAttack != null)
        {
            if (chant.notAttackWhileChanting)
                unitAttack.stopAttacking = false;
        }
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
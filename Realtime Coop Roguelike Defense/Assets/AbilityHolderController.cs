using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AbilityHolderController : MonoBehaviour
{
    [SerializeField] private AbilityHolder[] abilityHolders;

    [Space]
    [SerializeField] private bool DisableAbilitiesOnUse;
    [SerializeField] private float skillDisabledTime = 0.5f;
    private void Awake()
    {
        abilityHolders = GetComponents<AbilityHolder>();
        
    }

    private void OnValidate()
    {
        abilityHolders = GetComponents<AbilityHolder>();
    }
    private void OnEnable()
    {
        foreach (AbilityHolder ah in abilityHolders)
        {
            ah.OnAbilityUsed += LockAbility;
        }
    }

    private void OnDisable()
    {
        foreach (AbilityHolder ah in abilityHolders)
        {
            ah.OnAbilityUsed -= LockAbility;
        }
    }

    private async void LockAbility(AbilityHolder ah)
    {
        ah.isLocked = true;
        await Task.Delay((int)(skillDisabledTime * 1000));
        ah.isLocked = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

// class that also supports on collision events
public class SpawnCollisionAbility : Ability
{
    [Space]
    [Header("Spawn Collision Settings")]
    [SerializeField] protected GameObject spawningObject;
    [SerializeField] protected Vector3 objectSpawnOffsetPos;
    [SerializeField] protected Vector2 objectSize;
    
    //public bool ;
    protected Vector3 spawnPos;
    protected CollisionHandler collisionHandler;

    protected GameObject spawnedObject;

    protected virtual async Task WhilePlayerCantMove(GameObject parent)
    {
        Debug.Log("WWWWWWWWWWWWWWWWWWWWWWWWWWW");
        await Task.CompletedTask;
    }

    public override void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
        spawnPos = parent.transform.position + objectSpawnOffsetPos;
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(spawningObject, spawnPos, Quaternion.identity, parent.transform);
            spawnedObject.AddComponent<CollisionHandler>();
            collisionHandler = spawnedObject.GetComponent<CollisionHandler>();
        }
        collisionHandler.OnTriggerEntered += OnTriggerEnteredFunc;
        collisionHandler.transform.localScale = objectSize;
        collisionHandler.gameObject.SetActive(false);
        Debug.Log("Called from spawn collision ability start");
    }

    public override void OnAbilityRunning(GameObject parent)
    {
        base.OnAbilityRunning(parent);
        if (spawnedObject == null)
        {
            Debug.Log("Object is broken@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            isEnded = true;
        }

    }
    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
        spawnedObject.gameObject.SetActive(false);
        collisionHandler.OnTriggerEntered -= OnTriggerEnteredFunc;
        Debug.Log("Called from spawn collision ability end");
    }

    public virtual void OnTriggerEnteredFunc(Collider2D collision2D) { }
    
}

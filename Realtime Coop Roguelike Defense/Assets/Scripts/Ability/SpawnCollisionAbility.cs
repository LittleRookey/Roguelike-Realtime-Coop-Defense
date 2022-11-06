using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// class that also supports on collision events
public class SpawnCollisionAbility : Ability
{
    [SerializeField] protected GameObject spawningObject;
    [SerializeField] protected Vector3 objectSpawnOffsetPos;
    [SerializeField] protected Vector2 objectSize;
    
    //public bool ;
    protected Vector3 spawnPos;
    protected CollisionHandler collisionHandler;

    protected GameObject spawnedObject;

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
        collisionHandler.gameObject.SetActive(true);
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

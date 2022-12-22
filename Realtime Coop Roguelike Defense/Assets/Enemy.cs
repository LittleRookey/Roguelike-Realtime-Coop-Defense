using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Redcode.Pools;

public class Enemy : MonoBehaviour, IPoolObject
{
    public string Name;

    Health health;

    public UnityAction OnCreatedOnPool;
    private void Awake()
    {
        if (health == null)
            health = GetComponent<Health>();
        
    }


    // initializes health, position
    private void Init()
    {
        health.SetMaxHealth();
    }



    public void OnCreatedInPool()
    {
        
    }

    public void OnGettingFromPool()
    {
        Init();
    }

}

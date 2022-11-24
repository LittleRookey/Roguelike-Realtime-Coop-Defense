using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionHandler : MonoBehaviour
{
    public UnityAction<Collider2D> OnTriggerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEntered?.Invoke(collision);
    }
    
}

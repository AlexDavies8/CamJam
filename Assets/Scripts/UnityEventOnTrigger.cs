using System;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnTrigger : MonoBehaviour
{
    public UnityEvent Event;

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            Event.Invoke();
        }
    }
}
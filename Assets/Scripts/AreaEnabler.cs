using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AreaEnabler : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            target.SetActive(true);
        }
    }
}

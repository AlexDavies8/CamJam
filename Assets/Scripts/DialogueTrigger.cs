using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string _message = "Hello World";
    [SerializeField] private bool _singleUse = true;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        var speaker = col.GetComponent<DialogueSpeaker>();
        if (speaker)
        {
            speaker.Say(_message);
            if (_singleUse)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

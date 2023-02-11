using System;
using System.Collections;
using System.Collections.Generic;
using Optional;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private readonly Dictionary<Type, Component> _globals = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("GameManager already exists");
            enabled = false;
        }
    }

    public T GetGlobalComponent<T>() where T : Component
    {
        if (!_globals.ContainsKey(typeof(T))) _globals.Add(typeof(T), FindObjectOfType<T>());

        return (T)_globals[typeof(T)];
    }
}

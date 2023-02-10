using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly Dictionary<Type, Component> _globals = new();

    public T GetGlobalComponent<T>() where T : Component
    {
        if (_globals.ContainsKey(typeof(T))) _globals.Add(typeof(T), FindObjectOfType<T>());

        return (T)_globals[typeof(T)];
    }
}

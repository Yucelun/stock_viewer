using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseGameSingleton : EventTarget, IBaseSingleton
{
    public BaseGameSingleton()
    {
    }

    public abstract void Awake();

    public abstract void Start();

    public abstract void OnDestroy();

    public T get<T>() where T : new() { 
        return Kernel.get<T>();
    }
}

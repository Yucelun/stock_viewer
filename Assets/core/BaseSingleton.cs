using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISingletonEvent
{
    public EventTarget singleton;
    public string eventName;
    public Action<object> listener;
}

interface IBaseSingleton
{
    public void Awake();

    public void Start();

    public void OnDestroy();
}

public abstract class BaseSingleton : EventTarget, IBaseSingleton
{
    public BaseSingleton()
    {

    }

    public abstract void Awake();

    public abstract void Start();

    public abstract void OnDestroy();

    public T get<T>() where T : new()
    {
        return Kernel.get<T>();
    }
}


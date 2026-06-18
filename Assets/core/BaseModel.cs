using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseModel : BaseGameSingleton
{
    public BaseModel() : base() { 
    }

    public override void Awake() { 
    }

    public override void Start() { 
    }

    public override void OnDestroy()
    {
        this.clearListenToEvent();
        this.clearEvent();
    }

    private List<ISingletonEvent> listenToList = new List<ISingletonEvent>();
    protected void on<T>(string eventName, Action<object> listener) where T : BaseGameSingleton, new()
    {
        var singleton = this.get<T>();
        singleton.on(eventName, listener);
        this.listenToList.Add(new ISingletonEvent()
        {
            singleton = singleton,
            eventName = eventName,
            listener = listener
        });
    }

    protected void clearListenToEvent()
    {
        this.listenToList.ForEach(x => x.singleton.off(x.eventName, x.listener));
        this.listenToList.Clear();
    }
}

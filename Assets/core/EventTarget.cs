using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventTarget
{
    private Dictionary<string, List<Action<object>>> events = new Dictionary<string, List<Action<object>>>();

    public void on(string eventName, Action<object> listener)
    {
        List<Action<object>> listenerList = null;
        if (events.TryGetValue(eventName, out listenerList))
        {
            listenerList.Add(listener);
        }
        else
        {
            listenerList = new List<Action<object>>();
            listenerList.Add(listener);
            events.Add(eventName, listenerList);
        }
    }

    public void off(string eventName, Action<object> listener)
    {
        List<Action<object>> listenerList = null;
        if (events.TryGetValue(eventName, out listenerList))
        {
            listenerList.Remove(listener);
        }
    }

    public void clearEvent()
    {
        foreach (var item in events)
        {
            for (var i = item.Value.Count - 1; i > -1; i--)
            {
                var listener = item.Value[i];
                item.Value.Remove(listener);
            }
        }

        this.events.Clear();
    }

    public void emit(string eventName, object content)
    {
        List<Action<object>> listenerList = null;
        if (events.TryGetValue(eventName, out listenerList))
        {
            for (var i = 0; i < listenerList.Count; i++)
            {
                var listener = listenerList[i];
                listener(content);
            }
        }
    }
}

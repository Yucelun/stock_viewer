using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class BaseUIComponent : EventMonoBehaviour
{
    protected virtual void OnDestroy()
    {
        this.clearListenToEvent();
        this.clearEvent();
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void show()
    {
        this.gameObject.SetActive(true);
    }


    public T get<T>() where T : new()
    {
        return Kernel.get<T>();
    }

    private RectTransform _rectTransform = null;
    public RectTransform rectTransform
    {
        get
        {
            if (!this._rectTransform)
            {
                this._rectTransform = this.GetComponent<RectTransform>();
            }

            return this._rectTransform;
        }
    }

    public float width
    {
        get
        {
            return this.rectTransform.rect.width;
        }
        set
        {
            var sizeDelta = this.rectTransform.sizeDelta;
            this.rectTransform.sizeDelta = new Vector2(value, sizeDelta.y);
        }
    }

    public float height
    {
        get
        {
            return this.rectTransform.rect.height;
        }
        set
        {
            var sizeDelta = this.rectTransform.sizeDelta;
            this.rectTransform.sizeDelta = new Vector2(sizeDelta.x, value);
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseUI : EventMonoBehaviour
{
    private bool _useMask = true;
    public bool useMask
    {
        get
        {
            return this._useMask;
        }
        set
        {
            this._useMask = value;
        }
    }

    private bool _useMaskClick = false;
    public bool useMaskClick
    {
        get
        {
            return this._useMaskClick;
        }
        set
        {
            this._useMaskClick = value;
            this.refreshMaskClick();
        }
    }

    protected virtual void Awake()
    {
        this.refreshMaskClick();
    }

    protected void refreshMaskClick()
    {
        if (this._useMaskClick)
        {
            this.on<UiManager>("onMaskClick", this.doMaskClick);
        }
        else
        {
            this.off<UiManager>("onMaskClick", this.doMaskClick);
        }
    }

    protected virtual void OnDestroy()
    {
        this.clearListenToEvent();
        this.clearEvent();
    }

    protected void doMaskClick(object obj)
    {
        this.exit();
    }

    public void exit()
    {
        this.get<UiManager>().closeUI(this);
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

    private List<ISingletonEvent> listenToList = new List<ISingletonEvent> ();
    protected void on<T>(string eventName, Action<object> listener) where T : EventTarget, new()
    {
        var singleton = this.get<T>();
        singleton.on(eventName, listener);
        var index = this.listenToList.FindIndex(x => x.singleton == singleton && x.eventName == eventName && x.listener == listener);
        if (index == -1)
        {
            this.listenToList.Add(new ISingletonEvent()
            {
                singleton = singleton,
                eventName = eventName,
                listener = listener
            });
        }
    }
    protected void off<T>(string eventName, Action<object> listener) where T : EventTarget, new()
    {
        var singleton = this.get<T>();
        singleton.off(eventName, listener);
        var index = this.listenToList.FindIndex(x => x.singleton == singleton && x.eventName == eventName && x.listener == listener);
        if (index > -1) { 
            this.listenToList.RemoveAt(index);
        }
    }

    protected void clearListenToEvent()
    {
        this.listenToList.ForEach(x => x.singleton.off(x.eventName, x.listener));
        this.listenToList.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public abstract class ItemRenderer : EventMonoBehaviour
{
    private object _data = null;
    public object data
    {
        get
        {
            return _data;
        }

        set
        {
            _data = value;
            this.doDataChanged();
        }
    }

    private int _index = -1;
    public int index
    {
        get
        {
            return _index;
        }

        set
        {
            _index = value;
        }
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

            return _rectTransform;
        }
    }

    public float width
    {
        get
        {
            if (!this._rectTransform)
            {
                this._rectTransform = this.GetComponent<RectTransform>();
            }
            return this._rectTransform.rect.width;
        }
        set
        {
            if (!this._rectTransform)
            {
                this._rectTransform = this.GetComponent<RectTransform>();
            }
            var size = this._rectTransform.sizeDelta;
            this._rectTransform.sizeDelta = new Vector2(value, size.y);
        }
    }

    public float height
    {
        get
        {
            if (!this._rectTransform)
            {
                this._rectTransform = this.GetComponent<RectTransform>();
            }
            return this._rectTransform.rect.height;
        }
        set
        {
            if (!this._rectTransform)
            {
                this._rectTransform = this.GetComponent<RectTransform>();
            }
            var size = this._rectTransform.sizeDelta;
            this._rectTransform.sizeDelta = new Vector2(size.x, value);
        }
    }

    protected abstract void doDataChanged();

    public T get<T>() where T : new()
    {
        return Kernel.get<T>();
    }
}

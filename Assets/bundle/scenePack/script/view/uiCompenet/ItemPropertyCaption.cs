using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPropertyCaption : ItemRenderer
{
    [SerializeField]
    protected Text textPropertyCaption;

    protected void Awake() {
    }

    protected override void doDataChanged()
    {
        var value = this.data as string;

        this.textPropertyCaption.text = value;
    }
}

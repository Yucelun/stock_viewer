using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : ItemRenderer
{
    [SerializeField]
    protected Text textCaption;

    protected void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(this.doClick);
    }

    protected override void doDataChanged()
    {
        var caption = this.data as string;

        this.textCaption.text = caption;
    }

    protected void doClick()
    {
        this.emit("onClick", this);
    }
}

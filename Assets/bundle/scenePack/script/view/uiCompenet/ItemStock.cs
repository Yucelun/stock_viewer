using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemStock : ItemRenderer
{
    [SerializeField]
    protected List listProperty;

    [SerializeField]
    protected Button btnClick;

    protected void Awake()
    {
        this.btnClick.onClick.AddListener(this.doClick);
    }

    protected override void doDataChanged()
    {
        this.listProperty.replace((this.data as List<ICell>).ToList<object>());
    }

    protected void doClick()
    {
        this.emit("onClick", this.data);
    }
}
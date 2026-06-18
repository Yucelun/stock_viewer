using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemProperty : ItemRenderer
{
    [SerializeField]
    protected Text textProperty;

    protected void Awake() {
    }

    protected override void doDataChanged()
    {
        var cell = this.data as ICell;

        this.textProperty.text = cell.value;
    }
}

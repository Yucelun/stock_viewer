using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSearchStock : BaseUI
{
    [SerializeField]
    protected GroupSearchStock groupSearchStock;

    protected override void Awake()
    {
        base.Awake();

        this.groupSearchStock.on("onCancel", this.doCancelSearch);
        this.groupSearchStock.on("onSearchStockItem", this.doSearchStockItem);
    }

    protected void Start()
    {
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected void doCancelSearch(object obj)
    {
        this.exit();
    }

    protected void doSearchStockItem(object obj)
    {
        this.emit("onSearchStockItem", obj);
        this.exit();
    }
}

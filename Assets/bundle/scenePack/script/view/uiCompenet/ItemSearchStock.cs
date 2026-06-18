using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSearchStock : ItemRenderer
{
    [SerializeField]
    protected Button btnSelect;

    [SerializeField]
    protected Text textCaption;

    private void Awake()
    {
        this.btnSelect.onClick.AddListener(this.doSelect);
    }

    protected override void doDataChanged()
    {
        var stockData = this.data as OptionalStocksStock;
        var stockId = stockData.stockId;
        var name = stockData.name;
        this.textCaption.text = string.Format("{0} {1}", stockId, name);
    }

    protected void doSelect()
    {
        this.emit("onClick", this);
    }
}

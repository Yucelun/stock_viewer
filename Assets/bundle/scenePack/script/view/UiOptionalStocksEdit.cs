using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiOptionalStocksEdit : BaseUI
{
    [SerializeField]
    protected GroupOptionalStocksEdit groupOptionalStocksEdit;

    [SerializeField]
    protected GroupSearchStock groupSearchStock;

    protected override void Awake()
    {
        base.Awake();

        this.on<OptionalStocksModel>("editStatus", this.refreshEditStatus);
        this.groupOptionalStocksEdit.on("onOk", this.doOk);
        this.groupOptionalStocksEdit.on("onClose", this.doClose);

        this.groupSearchStock.on("onCancel", this.doCancelSearch);
        this.groupSearchStock.on("onSearchStockItem", this.doSearchStockItem);
    }

    protected void Start()
    {
        this.refreshEditStatus(this.get<OptionalStocksModel>().editStatus);
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected void doOk(object obj)
    {
        var editOptionalStocks = this.get<OptionalStocksModel>().editOptionalStocks;
        this.get<OptionalStocksModel>().setOptionalStocks(editOptionalStocks);

        this.exit();
    }

    protected void doClose(object obj)
    {
        var editOptionalStocks = this.get<OptionalStocksModel>().editOptionalStocks;
        var stockList = editOptionalStocks.stockList;
        if (stockList.Count > 0)
        {
            var targetStockData = stockList[stockList.Count - 1];
            var stockId = targetStockData.stockId;
            var name = targetStockData.name;
            if (stockId == "" && name == "") {
                stockList.RemoveAt(stockList.Count - 1);
                this.get<OptionalStocksModel>().setEditOptionalStocks(editOptionalStocks);
            }
        }
        this.get<OptionalStocksModel>().switchEditStatus(OptionalStocksEditStatus.None);
        this.exit();
    }

    protected void doCancelSearch(object obj)
    {
        this.get<OptionalStocksModel>().switchEditStatus(OptionalStocksEditStatus.None);
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.None);
    }

    protected void doSearchStockItem(object obj)
    {
        var stockData = obj as OptionalStocksStock;
        var editOptionalStocksStock = this.get<OptionalStocksModel>().editOptionalStocksStock;
        editOptionalStocksStock.update(stockData);
        var editOptionalStocks = this.get<OptionalStocksModel>().editOptionalStocks;
        editOptionalStocks.setOptionalStocksStock(editOptionalStocksStock);
        this.get<OptionalStocksModel>().setEditOptionalStocks(editOptionalStocks);
        this.get<OptionalStocksModel>().switchEditStatus(OptionalStocksEditStatus.None);
    }

    protected void refreshEditStatus(object obj) { 
        var editStatus = (OptionalStocksEditStatus)obj;
        this.groupOptionalStocksEdit.gameObject.SetActive(editStatus == OptionalStocksEditStatus.None);
        this.groupSearchStock.gameObject.SetActive(editStatus == OptionalStocksEditStatus.Search);
    }
}

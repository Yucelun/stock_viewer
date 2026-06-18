using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOptionalStocksStock : ItemRenderer
{
    [SerializeField]
    protected Button btnNew;

    [SerializeField]
    protected GameObject groupContent;

    [SerializeField]
    protected Button btnDelete;

    [SerializeField]
    protected Text textCaption;

    private void Awake()
    {
        this.btnNew.onClick.AddListener(this.doNew);
        this.btnDelete.onClick.AddListener(this.doDelete);
    }

    protected override void doDataChanged()
    {
        if (this.data == null)
        {
            this.btnNew.gameObject.SetActive(true);
            this.groupContent.SetActive(false);
        }
        else
        {
            this.btnNew.gameObject.SetActive(false);
            this.groupContent.SetActive(true);

            var stockData = this.data as OptionalStocksStock;
            var stockId = stockData.stockId;
            var name = stockData.name;
            this.textCaption.text = string.Format("{0} {1}", stockId, name);
        }
    }

    protected void doNew()
    {
        this.get<OptionalStocksModel>().setEditOptionalStocksStock(new OptionalStocksStock()
        {
            serialNo = OptionalStocksStock.serialCounter.generateSerialNo(),
        });
        this.get<OptionalStocksModel>().switchEditStatus(OptionalStocksEditStatus.Search);
    }

    protected void doDelete()
    {
        var editOptionalStocks = this.get<OptionalStocksModel>().editOptionalStocks;
        editOptionalStocks.removeOptionalStocksStock(this.data as OptionalStocksStock);
        this.get<OptionalStocksModel>().setEditOptionalStocks(editOptionalStocks);
    }

}

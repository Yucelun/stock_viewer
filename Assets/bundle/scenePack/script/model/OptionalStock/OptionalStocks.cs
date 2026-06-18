using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class OptionalStocksStock
{
    public int serialNo = 0;
    public string name = "";
    public string stockId = "";

    public OptionalStocksStock copy()
    {
        return new OptionalStocksStock() {
            serialNo = this.serialNo,
            name = this.name,
            stockId = this.stockId,
        };
    }

    public void update(OptionalStocksStock data)
    {
        this.name = data.name;
        this.stockId = data.stockId;
    }

    public static SerialCounter serialCounter = new SerialCounter();
}

public class OptionalStocks
{
    public int serialNo = 0;
    public string name = "";
    public bool isEdit = false;
    public bool isDefault = false;
    public List<OptionalStocksStock> stockList = new List<OptionalStocksStock>();

    public OptionalStocks copy()
    {
        return new OptionalStocks()
        {
            serialNo = this.serialNo,
            name = this.name,
            isEdit = this.isEdit,
            isDefault = this.isDefault,
            stockList = this.stockList.Select(x => x.copy()).ToList(),
        };
    }

    public void update(OptionalStocks data)
    {
        this.name = data.name;
        this.isEdit = data.isEdit;
        this.isDefault = data.isDefault;
        this.stockList = data.stockList.Select(x => x.copy()).ToList();
    }

    public OptionalStocksStock getOptionalStocksStock(int serialNo)
    {
        return this.stockList.Find(x => x.serialNo == serialNo);
    }

    public void setOptionalStocksStock(OptionalStocksStock data)
    {
        var index = this.stockList.FindIndex(x => x.serialNo == data.serialNo);
        if (index > -1)
        {
            this.stockList[index].update(data);
        }
        else
        {
            this.stockList.Add(data);
        }
    }

    public void removeOptionalStocksStock(OptionalStocksStock data)
    {
        var index = this.stockList.FindIndex(x => x.serialNo == data.serialNo);
        if (index > -1)
        {
            this.stockList.RemoveAt(index);
        }
    }

    public static SerialCounter serialCounter = new SerialCounter();
}

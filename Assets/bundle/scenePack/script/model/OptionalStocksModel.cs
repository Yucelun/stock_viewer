using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public enum OptionalStocksEditStatus
{
    None,
    Search,
}

public class IStorageOptionalStocksList
{
    public List<OptionalStocks> list;
}

public class OptionalStocksModel : BaseModel
{
    public string fileName = "optionalStocks";
    public override void Awake()
    {
        base.Awake();

        this._optionalStocksList = this.getOptionalStocksList();
        int maxSerialCounter_OptionalStocks = this._optionalStocksList.Select(x => x.serialNo).Max();
        OptionalStocks.serialCounter.replace(maxSerialCounter_OptionalStocks);
        int maxSerialCounter_OptionalStocksStock = this._optionalStocksList.Select(x => x.stockList.Select(y => y.serialNo).Max()).Max();
        OptionalStocksStock.serialCounter.replace(maxSerialCounter_OptionalStocksStock);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected List<OptionalStocks> getOptionalStocksList()
    {
        IStorageOptionalStocksList jsonObject;
        if (this.get<SaveFile>().exists(this.fileName))
        {
            var loadObject = this.get<SaveFile>().load<IStorageOptionalStocksList>(this.fileName);
            if (loadObject != null)
            {
                jsonObject = loadObject;
            }
            else
            {
                jsonObject = this.getDefaultOptionalStocksList();
            }
        }
        else
        {
            jsonObject = this.getDefaultOptionalStocksList();
        }

        return jsonObject.list;
    }

    protected IStorageOptionalStocksList getDefaultOptionalStocksList()
    {
        return new IStorageOptionalStocksList()
        {
            list = new List<OptionalStocks>() {
                new OptionalStocks()
                {
                    serialNo = OptionalStocks.serialCounter.generateSerialNo(),
                    name = "預設自選",
                    isDefault = true,
                    stockList = new List<OptionalStocksStock>
                    {
                        new OptionalStocksStock() {
                            serialNo = OptionalStocksStock.serialCounter.generateSerialNo(),
                            name = "台積電",
                            stockId = "2330",
                        }
                    }
                }
            }
        };
    }

    protected List<OptionalStocks> _optionalStocksList = new List<OptionalStocks>();
    public List<OptionalStocks> optionalStocksList
    {
        get
        {
            return this._optionalStocksList;
        }
    }

    public void removeOptionalStocks(OptionalStocks optionalStocks)
    {
        var index = this.optionalStocksList.FindIndex((x) => x.serialNo == optionalStocks.serialNo);
        if (index > -1)
        {
            this.optionalStocksList.RemoveAt(index);
            this.get<SaveFile>().save(this.fileName, new IStorageOptionalStocksList() { list = this.optionalStocksList });
            this.emit("optionalStocksList", this._optionalStocksList);
        }
    }

    public void setOptionalStocks(OptionalStocks optionalStocks)
    {
        var find = this.optionalStocksList.Find((x) => x.serialNo == optionalStocks.serialNo);
        if (find != null)
        {
            find.update(optionalStocks);
        }
        else
        {
            this.optionalStocksList.Add(optionalStocks);
        }
        this.get<SaveFile>().save(this.fileName, new IStorageOptionalStocksList() { list = this.optionalStocksList });
        this.emit("optionalStocksList", this._optionalStocksList);
    }

    protected OptionalStocks _optionalStocks = null;
    public OptionalStocks optionalStocks
    {
        get
        {
            return this._optionalStocks;
        }
    }

    public void useOptionalStocks(OptionalStocks optionalStocks)
    {
        this._optionalStocks = optionalStocks;
        this.emit("optionalStocks", optionalStocks);
    }

    protected OptionalStocks _editOptionalStocks = null;
    public OptionalStocks editOptionalStocks
    {
        get
        {
            return this._editOptionalStocks;
        }
    }

    public void setEditOptionalStocks(OptionalStocks editOptionalStocks)
    {
        this._editOptionalStocks = editOptionalStocks;
        this.emit("editOptionalStocks", this._editOptionalStocks);
    }

    private bool _isOpenOptionalStocks = false;
    public bool isOpenOptionalStocks
    {
        get
        {
            return this._isOpenOptionalStocks;
        }
    }

    public void switchOptionalStocks(bool isOpenOptionalStocks)
    {
        this._isOpenOptionalStocks = isOpenOptionalStocks;
        this.emit("isOpenOptionalStocks", this._isOpenOptionalStocks);
    }

    private bool _isEdit = false;
    public bool isEdit
    {
        get
        {
            return this._isEdit;
        }
    }

    public void switchEdit(bool isEdit)
    {
        this._isEdit = isEdit;
        this.emit("isEdit", this._isEdit);
    }

    private OptionalStocksEditStatus _editStatus = OptionalStocksEditStatus.None;
    public OptionalStocksEditStatus editStatus
    {
        get
        {
            return this._editStatus;
        }
    }

    public void switchEditStatus(OptionalStocksEditStatus editStatus)
    {
        this._editStatus = editStatus;
        this.emit("editStatus", this._editStatus);
    }

    protected OptionalStocksStock _editOptionalStocksStock = null;
    public OptionalStocksStock editOptionalStocksStock
    {
        get
        {
            return this._editOptionalStocksStock;
        }
    }

    public void setEditOptionalStocksStock(OptionalStocksStock editOptionalStocksStock)
    {
        this._editOptionalStocksStock = editOptionalStocksStock;
        this.emit("editOptionalStocksStock", this._editOptionalStocksStock);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GroupSearchStock : BaseUIComponent
{
    [SerializeField]
    protected Button btnCancel;

    [SerializeField]
    protected InputField inputFieldSearchStock;

    [SerializeField]
    protected List listSearchStock;

    protected void Awake()
    {
        this.btnCancel.onClick.AddListener(this.doCancel);
        this.inputFieldSearchStock.onValueChanged.AddListener(this.doSearchValueChanged);
        this.on<OptionalStocksModel>("editStatus", this.refreshEditStatus);
        this.listSearchStock.on("onItemClick", this.doItemClick);
    }

    protected void Start()
    {
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected void doSearchValueChanged(string value)
    {
        if (value != "")
        {
            this.searchText(value);
        }
    }
    public void searchText(string text)
    {
        this.refreshSearchStockList(
            this.get<GlobalData>().stockFinancialList.FindAll((x) => {
                return x.name.Contains(text) || x.stockId.Contains(text);
            })
        );
    }

    protected void refreshSearchStockList(object obj)
    {
        var searchStockList = obj as List<IStockFinancialData>;
        var _searchStockList = (searchStockList.Count > 5 ? searchStockList.Where((x, i) => i < 5) : searchStockList).Select(x => new OptionalStocksStock()
        {
            stockId = x.stockId,
            name = x.name,
        }).ToList<object>();
        this.listSearchStock.replace(_searchStockList);
    }

    protected void refreshEditStatus(object obj)
    {
        var editStatus = (OptionalStocksEditStatus)obj;
        if (editStatus == OptionalStocksEditStatus.Search) {
            this.inputFieldSearchStock.SetTextWithoutNotify("");
            this.listSearchStock.replace(new List<object>());
        }
    }

    protected void doCancel()
    {
        this.emit("onCancel", null);
    }

    protected void doItemClick(object obj)
    {
        var item = obj as ItemSearchStock;
        this.emit("onSearchStockItem", item.data);
    }
}

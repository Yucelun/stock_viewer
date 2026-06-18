using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GroupOptionalStocksEdit : BaseUIComponent
{
    [SerializeField]
    protected Button btnOk;

    [SerializeField]
    protected Button btnClose;

    [SerializeField]
    protected InputField inputFieldOptionalStocksName;

    [SerializeField]
    protected List listOptionalStocks;

    protected void Awake()
    {
        this.btnOk.onClick.AddListener(this.doOk);
        this.btnClose.onClick.AddListener(this.doClose);
        this.inputFieldOptionalStocksName.onValueChanged.AddListener(this.doOptionalStocksNameValueChanged);

        this.on<OptionalStocksModel>("editOptionalStocks", this.refreshEditOptionalStocks);
    }

    protected void Start()
    {
        this.refreshEditOptionalStocks(this.get<OptionalStocksModel>().editOptionalStocks);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected void refreshEditOptionalStocks(object obj)
    {
        var optionalStocks = obj as OptionalStocks;
        var name = optionalStocks.name;
        var stockList = optionalStocks.stockList;

        this.inputFieldOptionalStocksName.SetTextWithoutNotify(name);
        var _stockList = stockList.Select(x => x).ToList<object>();
        _stockList.Add(null);
        this.listOptionalStocks.replace(_stockList);
    }

    protected void doOptionalStocksNameValueChanged(string value)
    {
        var editOptionalStocks = this.get<OptionalStocksModel>().editOptionalStocks;
        editOptionalStocks.name = value;
        this.get<OptionalStocksModel>().setEditOptionalStocks(editOptionalStocks);
    }

    protected void doOk()
    {
        this.emit("onOk", null);
    }

    protected void doClose()
    {
        this.emit("onClose", null);
    }
}

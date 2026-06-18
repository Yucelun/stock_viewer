using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiOptionalStocksMenu : BaseUIComponent
{
    [SerializeField]
    protected GameObject groupBig;

    [SerializeField]
    protected GameObject groupSmall;

    [SerializeField]
    protected List listOptionalStocksBig;

    [SerializeField]
    protected List listOptionalStocksSmall;

    [SerializeField]
    protected Button btnMask;

    [SerializeField]
    protected Button btnOpen;

    [SerializeField]
    protected Button btnClose;

    //[SerializeField]
    //protected Toggle toggleEdit;

    protected void Awake()
    {
        this.btnOpen.onClick.AddListener(this.doOpen);
        this.btnClose.onClick.AddListener(this.doClose);
        this.btnMask.onClick.AddListener(this.doMask);

        //this.toggleEdit.onValueChanged.AddListener(this.doEdit);

        this.on<OptionalStocksModel>("optionalStocksList", this.refreshOptionalStocksList);
        this.on<OptionalStocksModel>("isOpenOptionalStocks", this.refreshIsOpenOptionalStocks);
        this.on<OptionalStocksModel>("isEdit", this.refreshIsEdit);
        this.on<PageModel>("pageIndex", this.refreshPageIndex);
    }

    protected void Start()
    {
        this.refreshOptionalStocksList(this.get<OptionalStocksModel>().optionalStocksList);
        this.refreshIsOpenOptionalStocks(this.get<OptionalStocksModel>().isOpenOptionalStocks);
        this.refreshIsEdit(this.get<OptionalStocksModel>().isEdit);
        this.refreshPageIndex(this.get<PageModel>().pageIndex);
    }

    protected void refreshPageIndex(object obj)
    {
        var pageIndex = (PageIndex)obj;
        this.gameObject.SetActive(pageIndex == PageIndex.OptionalStocks);
    }

    protected void refreshIsEdit(object obj)
    {
        var isEdit = (bool)obj;
        //this.toggleEdit.SetIsOnWithoutNotify(isEdit);
        this.refreshListOptionalStocksBig(this.get<OptionalStocksModel>().optionalStocksList, isEdit);
        this.btnMask.gameObject.SetActive(isEdit);
    }

    protected void refreshIsOpenOptionalStocks(object obj)
    {
        var isOpenOptionalStocks = (bool)obj;
        this.groupBig.SetActive(isOpenOptionalStocks);
        this.groupSmall.SetActive(!isOpenOptionalStocks);
    }

    protected void refreshListOptionalStocksBig(List<OptionalStocks> optionalStocksList, bool isEdit)
    {
        var _optionalStocksList = optionalStocksList.Select(x => x.copy()).ToList();
        _optionalStocksList.ForEach(x => x.isEdit = isEdit);
        if (isEdit) {
            _optionalStocksList.Add(null);
        }
        this.listOptionalStocksBig.replace(_optionalStocksList.ToList<object>());
    }

    protected void refreshOptionalStocksList(object obj)
    {
        var optionalStocksList = obj as List<OptionalStocks>;
        this.listOptionalStocksSmall.replace(optionalStocksList.ToList<object>());
        this.refreshListOptionalStocksBig(optionalStocksList, this.get<OptionalStocksModel>().isEdit);
    }

    protected void doOpen()
    {
        this.get<OptionalStocksModel>().switchOptionalStocks(true);
        this.get<OptionalStocksModel>().switchEdit(true);
    }

    protected void doClose()
    {
        this.get<OptionalStocksModel>().switchOptionalStocks(false);
        this.get<OptionalStocksModel>().switchEdit(false);
    }

    protected void doMask()
    {
        this.get<OptionalStocksModel>().switchOptionalStocks(false);
        this.get<OptionalStocksModel>().switchEdit(false);
    }

    //protected void doEdit(bool isEdit)
    //{
    //    this.get<OptionalStocksModel>().switchEdit(isEdit);
    //}
}

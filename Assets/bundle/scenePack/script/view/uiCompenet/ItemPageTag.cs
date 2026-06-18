using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPageTag : ItemRenderer
{
    [SerializeField]
    protected Button btnPage;

    [SerializeField]
    protected Text textCaption;

    private void Awake()
    {
        this.btnPage.onClick.AddListener(this.doPage);
    }

    protected override void doDataChanged()
    {
        var pageInfo = this.data as PageInfo;
        this.textCaption.text = pageInfo.name;

        this.btnPage.interactable = !pageInfo.selected;

    }

    protected void doPage()
    {
        var pageIndex = (this.data as PageInfo).pageIndex;
        this.get<PageModel>().setPageIndex(pageIndex);
        switch (pageIndex)
        {
            case PageIndex.OptionalStocks:
                if (this.get<OptionalStocksModel>().optionalStocks == null)
                {
                    this.get<OptionalStocksModel>().useOptionalStocks(this.get<OptionalStocksModel>().optionalStocksList[0]);
                }
                break;
            case PageIndex.Filter:
                if (this.get<FilterModel>().filter == null) { 
                    this.get<FilterModel>().useFilter(this.get<FilterModel>().filterList[0]);
                }
                break;
        }
    }
}

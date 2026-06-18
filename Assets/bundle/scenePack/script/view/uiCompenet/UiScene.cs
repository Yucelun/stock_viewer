using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class UiScene : BaseUIComponent
{
    [SerializeField]
    protected UiOptionalStocksList uiOptionalStocksList;

    [SerializeField]
    protected UiFilterList uiFilterList;

    [SerializeField]
    protected UiStudyList uiStudyList;

    [SerializeField]
    protected UiCompareList uiCompareList;

    [SerializeField]
    protected UiSystemList uiSystemList;
    
    protected void Awake()
    {
        this.on<PageModel>("pageIndex", this.refreshPageIndex);
    }

    private void Start()
    {
        this.refreshPageIndex(this.get<PageModel>().pageIndex);
    }

    protected void refreshPageIndex(object obj)
    {
        var pageIndex = (PageIndex)obj;
        this.uiOptionalStocksList.gameObject.SetActive(pageIndex == PageIndex.OptionalStocks);
        this.uiFilterList.gameObject.SetActive(pageIndex == PageIndex.Filter);
        this.uiStudyList.gameObject.SetActive(pageIndex == PageIndex.Study);
        this.uiCompareList.gameObject.SetActive(pageIndex == PageIndex.Compare);
        this.uiSystemList.gameObject.SetActive(pageIndex == PageIndex.System);
    }
}

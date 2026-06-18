using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiCompareList : BaseUIComponent
{
    [SerializeField]
    protected Button btnSearchLeft;

    [SerializeField]
    protected Button btnSearchRight;

    [SerializeField]
    protected Button btnReturn;

    [SerializeField]
    protected Text textLeftCaption;

    [SerializeField]
    protected Text textRightCaption;

    [SerializeField]
    protected RadarChart radarChart;

    [SerializeField]
    protected List listScoreHint;

    [SerializeField]
    protected List listCompareScore;

    protected void Awake()
    {
        this.btnSearchLeft.onClick.AddListener(this.doSearchLeft);
        this.btnSearchRight.onClick.AddListener(this.doSearchRight);
        this.btnReturn.onClick.AddListener(this.doReturn);

        this.on<CompareModel>("leftStockId", this.refreshLeftStockId);
        this.on<CompareModel>("rightStockId", this.refreshLeftStockId);
        this.on<CompareModel>("compareScoreList", this.refreshCompareScoreList);
        this.on<PageModel>("prePageIndex", this.refreshPrePageIndex);

    }

    protected void Start()
    {
        this.refreshLeftStockId(this.get<CompareModel>().leftStockId);
        this.refreshPrePageIndex(this.get<PageModel>().prePageIndex);
    }

    protected void refreshPrePageIndex(object obj)
    {
        var prePageIndex = (PageIndex)obj;
        this.btnSearchLeft.gameObject.SetActive(prePageIndex == PageIndex.None);
        this.btnSearchRight.gameObject.SetActive(prePageIndex == PageIndex.None);
        this.btnReturn.gameObject.SetActive(prePageIndex != PageIndex.None);
    }

    protected void refreshLeftStockId(object obj)
    {
        this.refreshCompareStock(obj as string, this.get<CompareModel>().rightStockId, this.get<CompareModel>().compareScoreList);
    }

    protected void refreshCompareScoreList(object obj)
    {
        this.refreshCompareStock(this.get<CompareModel>().leftStockId, this.get<CompareModel>().rightStockId, obj as List<CompareScore>);
    }

    protected void refreshCompareStock(string leftStockId, string rightStockId, List<CompareScore> compareScoreList)
    {
        var leftCompareScore = compareScoreList.Find(x => x.stockId == leftStockId);
        if (leftCompareScore == null)
        {
            return;
        }
        var rightCompareScore = compareScoreList.Find(x => x.stockId == rightStockId);
        if (rightCompareScore == null)
        {
            return;
        }
        var leftCompareSubScoreList = leftCompareScore.compareSubScoreList;
        var rightCompareSubScoreList = rightCompareScore.compareSubScoreList;
        var stockFinancialList = this.get<GlobalData>().stockFinancialList;
        var leftStockFinancialData = stockFinancialList.Find(x => x.stockId == leftStockId);
        var rightStockFinancialData = stockFinancialList.Find(x => x.stockId == rightStockId);
        var leftPropertyList = leftCompareSubScoreList.Select((x) => x.score).ToList();
        var rightPropertyList = rightCompareSubScoreList.Select((x) => x.score).ToList();
        var itemCompareScoreHintList = leftCompareSubScoreList.Select(x => new ItemCompareScoreHintData()
        {
            scoreType = x.scoreType,
            left = x.score,
            right = rightCompareSubScoreList.Find(y => y.scoreType == x.scoreType).score,
        }).ToList<object>();
        var itemCompareScorePageList = leftCompareSubScoreList.Select(x => new ItemCompareScorePageData()
        {
            scoreType = x.scoreType,
            left = x.score,
            right = rightCompareSubScoreList.Find(y => y.scoreType == x.scoreType).score,
            financialRatioList = x.compareFinancialScoreList.Select(y => new ItemCompareScorePageFinancialRatioData()
            {
                scoreType = x.scoreType,
                financialType = y.financialType,
                left = leftStockFinancialData.GetValue<float>(y.financialType.ToString()),
                right = rightStockFinancialData.GetValue<float>(y.financialType.ToString()),
            }).ToList(),
        }).ToList<object>();

        this.textLeftCaption.text = string.Format("{0} {1}", leftStockId, leftStockFinancialData.name);
        this.textRightCaption.text = string.Format("{0} {1}", rightStockId, rightStockFinancialData.name);
        this.listScoreHint.replace(itemCompareScoreHintList);
        var frontInfoList = this.radarChart.frontInfoList;
        frontInfoList[0].bg.propertyList = leftPropertyList;
        frontInfoList[0].line.propertyList = leftPropertyList;
        frontInfoList[1].bg.propertyList = rightPropertyList;
        frontInfoList[1].line.propertyList = rightPropertyList;
        this.radarChart.frontInfoList = frontInfoList;
        this.listCompareScore.replace(itemCompareScorePageList);
    }

    protected void doSearchLeft()
    {
        UiSearchStock uiSearchStock = this.get<UiManager>().openWindow<UiSearchStock>("uiPack", "uiSearchStock", NodeParent.UI);
        uiSearchStock.on("onSearchStockItem", doSearchLeftStockItem);
    }
    protected void doSearchLeftStockItem(object obj)
    {
        var stockData = obj as OptionalStocksStock;
        this.get<CompareModel>().setLeftStockId(stockData.stockId);
    }

    protected void doSearchRight()
    {
        UiSearchStock uiSearchStock = this.get<UiManager>().openWindow<UiSearchStock>("uiPack", "uiSearchStock", NodeParent.UI);
        uiSearchStock.on("onSearchStockItem", doSearchRightStockItem);
    }

    protected void doSearchRightStockItem(object obj)
    {
        var stockData = obj as OptionalStocksStock;
        this.get<CompareModel>().setRightStockId(stockData.stockId);
    }

    protected void doReturn()
    {
        this.get<PageModel>().setPageIndex(this.get<PageModel>().prePageIndex);
        this.get<PageModel>().setPrePageIndex(PageIndex.None);
    }
}

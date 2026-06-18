using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiStudyList : BaseUIComponent
{
    [SerializeField]
    protected Button btnSearch;

    [SerializeField]
    protected Button btnReturn;

    [SerializeField]
    protected Text textStudyCaption;

    [SerializeField]
    protected RadarChart radarChart;

    [SerializeField]
    protected List listScoreHint;

    [SerializeField]
    protected List listScoreTab;

    [SerializeField]
    protected List listScorePage;

    protected void Awake()
    {
        this.btnSearch.onClick.AddListener(this.doSearch);
        this.btnReturn.onClick.AddListener(this.doReturn);

        this.on<StudyModel>("studyStockId", this.refreshStudyStockId);
        this.on<StudyModel>("studyScoreList", this.refreshStudyScoreList);
        this.on<StudyModel>("selectedScoreType", this.refreshSelectedScoreType);
        this.on<PageModel>("prePageIndex", this.refreshPrePageIndex);

    }

    protected void Start()
    {
        this.refreshStudyStockId(this.get<StudyModel>().studyStockId);
        this.refreshPrePageIndex(this.get<PageModel>().prePageIndex);
    }

    protected void refreshPrePageIndex(object obj)
    {
        var prePageIndex = (PageIndex)obj;
        this.btnSearch.gameObject.SetActive(prePageIndex == PageIndex.None);
        this.btnReturn.gameObject.SetActive(prePageIndex != PageIndex.None);
    }

    protected void refreshStudyStockId(object obj)
    {
        this.refreshStudyStock(obj as string, this.get<StudyModel>().studyScoreList, this.get<StudyModel>().selectedScoreType);
    }

    protected void refreshStudyScoreList(object obj)
    {
        this.refreshStudyStock(this.get<StudyModel>().studyStockId, obj as List<StudyScore>, this.get<StudyModel>().selectedScoreType);
    }

    protected void refreshSelectedScoreType(object obj)
    {
        this.refreshStudyStock(this.get<StudyModel>().studyStockId, this.get<StudyModel>().studyScoreList, (ScoreType)obj);
    }

    protected void refreshStudyStock(string studyStockId, List<StudyScore> studyScoreList, ScoreType selectedScoreType)
    {
        var studyScore = studyScoreList.Find(x => x.stockId == studyStockId);
        if (studyScore != null)
        {
            var stockId = studyScore.stockId;
            var studySubScoreList = studyScore.studySubScoreList;
            var stockFinancialList = this.get<GlobalData>().stockFinancialList;
            var stockFinancialData = stockFinancialList.Find(x => x.stockId == stockId);
            var name = stockFinancialData.name;
            var propertyList = studySubScoreList.Select((x) => x.score).ToList();
            var itemStudyScoreHintList = studySubScoreList.Select(x => new ItemStudyScoreHintData()
            {
                scoreType = x.scoreType,
                score = x.score,
                selected = x.scoreType == selectedScoreType
            }).ToList<object>();
            var itemStudyScoreTabList = studySubScoreList.Select(x => new ItemStudyScoreTabData()
            {
                scoreType = x.scoreType,
                selected = x.scoreType == selectedScoreType
            }).ToList<object>();
            var itemStudyScorePageList = studySubScoreList.Where(x => x.scoreType == selectedScoreType).Select(x => new ItemStudyScorePageData()
            {
                scoreType = x.scoreType,
                score = x.score,
                financialRatioList = x.studyFinancialScoreList.Select(y => new ItemStudyScorePageFinancialRatioData()
                {
                    financialType = y.financialType,
                    value = stockFinancialData.GetValue<float>(y.financialType.ToString()),
                    score = y.score,
                }).ToList(),
            }).ToList<object>();

            this.textStudyCaption.text = string.Format("{0} {1}", stockId, name);
            this.listScoreHint.replace(itemStudyScoreHintList);
            var frontInfoList = this.radarChart.frontInfoList;
            frontInfoList[0].bg.propertyList = propertyList;
            frontInfoList[0].line.propertyList = propertyList;
            this.radarChart.frontInfoList = frontInfoList;
            this.listScoreTab.replace(itemStudyScoreTabList);
            this.listScorePage.replace(itemStudyScorePageList);
        }
    }

    protected void doSearch()
    {
        UiSearchStock uiSearchStock = this.get<UiManager>().openWindow<UiSearchStock>("uiPack", "uiSearchStock", NodeParent.UI);
        uiSearchStock.on("onSearchStockItem", doSearchStockItem);
    }
    protected void doSearchStockItem(object obj)
    {
        var stockData = obj as OptionalStocksStock;
        this.get<StudyModel>().setStudyStockId(stockData.stockId);
    }

    protected void doReturn()
    {
        this.get<PageModel>().setPageIndex(this.get<PageModel>().prePageIndex);
        this.get<PageModel>().setPrePageIndex(PageIndex.None);
    }
}

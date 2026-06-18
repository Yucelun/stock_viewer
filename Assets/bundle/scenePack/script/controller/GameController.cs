using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SceneController
{
    protected void Awake()
    {
        // init data
        var globalData = this.get<GlobalData>();
        // init model
        var filterModel = this.get<FilterModel>();
    }

    protected override void Start()
    {
        base.Start();


        this.get<GlobalData>().setLoadingLog("....");
        this.get<UiManager>().openWindow<UiLoading>("uiPack", "uiLoading", NodeParent.SYSTEM);
        StartCoroutine(this.loadData(0.1f));
    }

    protected void OnDestroy() { 
    }


    IEnumerator loadData(float time)
    {
        yield return new WaitForSeconds(time);
        this.get<GlobalData>().setLoadingLog("檢查更新....");
        var stockFinancialList = this.get<SheetModel>().query();
        this.get<GlobalData>().setStockFinancialList(stockFinancialList);
        yield return new WaitForSeconds(time);
        this.get<GlobalData>().setLoadingLog("計算分數....");
        var stockScoreList = this.get<ScoreModel>().getScoreList(stockFinancialList);
        this.get<GlobalData>().setStockScoreList(stockScoreList);
        yield return new WaitForSeconds(time);
        this.get<GlobalData>().setLoadingLog("準備開始....");
        yield return new WaitForSeconds(time);
        this.get<GlobalData>().setLoadingLog("");
    }
}

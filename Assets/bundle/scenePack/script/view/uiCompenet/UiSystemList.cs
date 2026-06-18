using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiSystemList : BaseUIComponent
{
    [SerializeField]
    protected Button btnUpdate;

    protected void Awake()
    {
        this.btnUpdate.onClick.AddListener(this.doUpdate);
    }

    protected void doUpdate()
    {
        this.get<GlobalData>().setLoadingLog("....");
        this.get<UiManager>().openWindow<UiLoading>("uiPack", "uiLoading", NodeParent.SYSTEM);
        this.get<GlobalData>().setLoadingLog("檢查更新....");
        var stockFinancialList = this.get<SheetModel>().query(true);
        this.get<GlobalData>().setStockFinancialList(stockFinancialList);
        this.get<GlobalData>().setLoadingLog("計算分數....");
        var stockScoreList = this.get<ScoreModel>().getScoreList(stockFinancialList);
        this.get<GlobalData>().setStockScoreList(stockScoreList);
        this.get<GlobalData>().setLoadingLog("");
    }
}

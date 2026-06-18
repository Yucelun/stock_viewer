using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FinancialType
{
    NONE,
    ReturnOfNetTangibleAssets_FirstQuartile,
    ReturnOfNetTangibleAssets_Median,
    ONWR,
    LongDebtRatio,
    YoY,
    OperatingProfitMargin,
    ReturnOfEquity_FirstQuartile,
    GrossProfitRatio,
    AnnualizedReturn,
    SalesToPrice,
    QuarterLength,
    SaveMargin,
    ReturnOfRequest,
    IntrinsicValue_FirstQuartile,
    IntrinsicValue_Median,
    IntrinsicValue_ThirdQuartile,
    price,
}

public class IStockScoreData
{
    public string stockId;
    public string name;
    public float ReturnOfNetTangibleAssets_FirstQuartile;
    public float ReturnOfNetTangibleAssets_Median;
    public float ONWR;
    public float LongDebtRatio;
    public float YoY;
    public float OperatingProfitMargin;
    public float ReturnOfEquity_FirstQuartile;
    public float GrossProfitRatio;
    public float AnnualizedReturn;
    public float SalesToPrice;
    public float SaveMargin;
}

[System.Serializable]
public class IStockFinancialData
{
    public float IntrinsicValue_FirstQuartile;
    public float IntrinsicValue_Median;
    public float IntrinsicValue_ThirdQuartile;
    public string reportUrl;
    public string reportStatus;
    public float AnnualizedReturn;
    public int QuarterLength;
    public float price;
    public float ONWR;
    public float GrossProfitRatio;
    public float ReturnOfNetTangibleAssets_FirstQuartile;
    public float ReturnOfNetTangibleAssets_Median;
    public float YoY;
    public float OperatingProfitMargin;
    public float ReturnOfEquity_FirstQuartile;
    public string stockId;
    public float ReturnOfRequest;
    public string name;
    public float LongDebtRatio;
    public float SalesToPrice;
    public float SaveMargin;
}

[System.Serializable]
public class ScoreList
{
    public List<IStockFinancialData> sampleDataList;
}

public class GlobalData : BaseGameSingleton
{
    public override void Awake()
    {
    }

    public override void Start()
    {
    }

    public override void OnDestroy()
    {
    }

    private List<IStockFinancialData> _stockFinancialList = new List<IStockFinancialData>();
    public List<IStockFinancialData> stockFinancialList
    {
        get
        {
            return this._stockFinancialList;
        }
    }

    public void setStockFinancialList(List<IStockFinancialData> stockFinancialList)
    {
        this._stockFinancialList = stockFinancialList;

        this.emit("stockFinancialList", this._stockFinancialList);
    }

    private List<IStockScoreData> _stockScoreList = new List<IStockScoreData>();
    public List<IStockScoreData> stockScoreList
    {
        get
        {
            return this._stockScoreList;
        }
    }

    public void setStockScoreList(List<IStockScoreData> stockScoreList)
    {
        this._stockScoreList = stockScoreList;

        this.emit("stockScoreList", this._stockScoreList);
    }

    private string _loadingLog = "";
    public string loadingLog {
        get { 
            return this._loadingLog;
        }
    }

    public void setLoadingLog(string log)
    {
        this._loadingLog = log;

        this.emit("loadingLog", this._loadingLog);
    }
}

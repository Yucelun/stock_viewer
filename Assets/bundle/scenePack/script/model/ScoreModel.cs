using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreModel : BaseModel
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected const int maxScore = 10;
    protected const float subLimit = 1f / maxScore;

    protected List<float> getLimits(List<float> series)
    {
        var result = new List<float>();
        var s = series.Where((x) => x >= 0).ToList();
        for (var i = 0; i < ScoreModel.maxScore + 1; i++)
        {
            var limit = quantile(s, ScoreModel.subLimit * i);
            result.Add(limit);
        }
        return result;
    }


    public float quantile(List<float> arr, float q)
    {
        arr.Sort((a, b) =>
        {
            if (a > b)
            {
                return 1;
            }
            else if (a < b)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });
        var pos = (arr.Count - 1) * q;
        var n = Mathf.FloorToInt(Mathf.Floor(pos));
        var f = pos - n;
        if (n + 1 < arr.Count)
        {
            return arr[n] + f * (arr[n + 1] - arr[n]);
        }
        else
        {
            return arr[n];
        }
    }


    protected int getGroupIndex(List<float> limits, float value, bool ascending)
    {
        if (ascending)
        {
            for (var i = 0; i < ScoreModel.maxScore; i++)
            {
                if (i == ScoreModel.maxScore - 1)
                {
                    if (limits[i] <= value && value <= limits[i + 1])
                    {
                        return i + 1;
                    }
                }
                else
                {
                    if (limits[i] <= value && value < limits[i + 1])
                    {
                        return i + 1;
                    }
                }
            }
        }
        else
        {
            for (var i = ScoreModel.maxScore - 1; i > -1; i--)
            {
                if (i == ScoreModel.maxScore - 1)
                {
                    if (limits[i] <= value && value <= limits[i + 1])
                    {
                        return ScoreModel.maxScore - i;
                    }
                }
                else
                {
                    if (limits[i] <= value && value < limits[i + 1])
                    {
                        return ScoreModel.maxScore - i;
                    }
                }
            }
        }

        return 0;
    }

    protected void setGroup(List<IStockFinancialData> sourceTable, string sourceCaption, List<IStockScoreData> targetTable, string targetCaption, bool ascending)
    {
        var limits = this.getLimits(sourceTable.Select((x) => x.GetValue<float>(sourceCaption)).ToList());
        for (int i = 0; i < sourceTable.Count; i++)
        {
            var row = sourceTable[i];
            var targetValue = this.getGroupIndex(limits, row.GetValue<float>(sourceCaption), ascending);
            targetTable[i].SetValue(targetCaption, targetValue);
        }
    }

    public List<IStockScoreData> getScoreList(List<IStockFinancialData>  sourceTable)
    {
        var targetTable = sourceTable.Select((x) => {
            return new IStockScoreData() 
            {
                stockId = x.stockId,
                name = x.name,
                ReturnOfNetTangibleAssets_FirstQuartile = 0,
                ReturnOfNetTangibleAssets_Median = 0,
                ONWR = 0,
                LongDebtRatio = 0,
                YoY = 0,
                OperatingProfitMargin = 0,
                ReturnOfEquity_FirstQuartile = 0,
                GrossProfitRatio = 0,
                AnnualizedReturn = 0,
                SalesToPrice = 0,
                SaveMargin = 0,
            };
        }).ToList();

        this.setGroup(sourceTable, "ReturnOfNetTangibleAssets_FirstQuartile", targetTable, "ReturnOfNetTangibleAssets_FirstQuartile", true);
        this.setGroup(sourceTable, "ReturnOfNetTangibleAssets_Median", targetTable, "ReturnOfNetTangibleAssets_Median", true);
        this.setGroup(sourceTable, "ONWR", targetTable, "ONWR", true);
        this.setGroup(sourceTable, "LongDebtRatio", targetTable, "LongDebtRatio", false);
        this.setGroup(sourceTable, "YoY", targetTable, "YoY", true);
        this.setGroup(sourceTable, "OperatingProfitMargin", targetTable, "OperatingProfitMargin", true);
        this.setGroup(sourceTable, "ReturnOfEquity_FirstQuartile", targetTable, "ReturnOfEquity_FirstQuartile", true);
        this.setGroup(sourceTable, "GrossProfitRatio", targetTable, "GrossProfitRatio", true);
        this.setGroup(sourceTable, "AnnualizedReturn", targetTable, "AnnualizedReturn", true);
        this.setGroup(sourceTable, "SalesToPrice", targetTable, "SalesToPrice", true);
        this.setGroup(sourceTable, "SaveMargin", targetTable, "SaveMargin", true);

        return targetTable;
    }
}

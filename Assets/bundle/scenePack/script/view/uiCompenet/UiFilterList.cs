using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class UiFilterList : BaseUIComponent
{
    [SerializeField]
    protected Grid grid;

    protected void Awake()
    {
        this.on<FilterModel>("filter", this.refreshFilter);

        this.grid.on("onRow", this.doRow);

        this.grid.columns = new List<string>() { "股票代號", "公司名稱", "安全邊際" };
    }

    protected void doRow(object obj)
    {
        var cellList = obj as List<ICell>;
        var stockId = cellList[0].value as string;
        this.get<StudyModel>().setStudyStockId(stockId);
        this.get<PageModel>().setPageIndex(PageIndex.Study);
        this.get<PageModel>().setPrePageIndex(PageIndex.Filter);
    }

    protected float getPropertyValue(FilterSort filterSort, Condition condition)
    {
        switch (condition.propertyType)
        {
            case ConditionType.FINANCE:
                return this.getFinancialRatio(filterSort.stockFinancial, condition.propertyFinancialType);
            case ConditionType.SCORE:
                return this.getScore(filterSort.studyScore, condition.propertyScoreType);
        }

        return 0f;
    }

    protected float getCompareValue(FilterSort filterSort, Condition condition)
    {
        switch (condition.compareType)
        {
            case ConditionType.FINANCE:
                return this.getFinancialRatio(filterSort.stockFinancial, condition.compareFinancialType);
            case ConditionType.SCORE:
                return this.getScore(filterSort.studyScore, condition.compareScoreType);
            case ConditionType.VALUE:
                return condition.compareValue;
        }

        return 0f;
    }

    protected float getScore(StudyScore data, ScoreType scoreType)
    {
        var find = data.studySubScoreList.Find((x) => x.scoreType == scoreType);
        if (find != null)
        {
            return find.score;
        }

        return 0f;
    }

    protected float getFinancialRatio(IStockFinancialData data, FinancialType financialType)
    {
        switch (financialType)
        {
            case FinancialType.ReturnOfNetTangibleAssets_FirstQuartile:
                return data.ReturnOfNetTangibleAssets_FirstQuartile;
            case FinancialType.ReturnOfNetTangibleAssets_Median:
                return data.ReturnOfNetTangibleAssets_Median;
            case FinancialType.ReturnOfEquity_FirstQuartile:
                return data.ReturnOfEquity_FirstQuartile;
            case FinancialType.AnnualizedReturn:
                return data.AnnualizedReturn;
            case FinancialType.GrossProfitRatio:
                return data.GrossProfitRatio;
            case FinancialType.LongDebtRatio:
                return data.LongDebtRatio;
            case FinancialType.ONWR:
                return data.ONWR;
            case FinancialType.OperatingProfitMargin:
                return data.OperatingProfitMargin;
            case FinancialType.SalesToPrice:
                return data.SalesToPrice;
            case FinancialType.YoY:
                return data.YoY;
            case FinancialType.QuarterLength:
                return data.QuarterLength;
            case FinancialType.SaveMargin:
                return data.SaveMargin;
            case FinancialType.ReturnOfRequest:
                return data.ReturnOfRequest;
            case FinancialType.IntrinsicValue_FirstQuartile:
                return data.IntrinsicValue_FirstQuartile;
            case FinancialType.IntrinsicValue_Median:
                return data.IntrinsicValue_Median;
            case FinancialType.IntrinsicValue_ThirdQuartile:
                return data.IntrinsicValue_ThirdQuartile;
            case FinancialType.price:
                return data.price;
        }

        return 0f;
    }

    protected bool compare(OperatorType operatorType, float propertyValue, float compareValue)
    {
        switch (operatorType)
        {
            case OperatorType.GREATER:
                return propertyValue > compareValue;
            case OperatorType.LESS:
                return propertyValue < compareValue;
            case OperatorType.GREATER_OR_EQUAL:
                return propertyValue >= compareValue;
            case OperatorType.LESS_OR_EQUAL:
                return propertyValue <= compareValue;
            case OperatorType.EQUAL:
                return propertyValue == compareValue;
        }

        return false;
    }


    protected void refreshFilter(object obj)
    {
        var filter = obj as Filter;
        var conditionList = filter.conditionList;
        var sortConditionList = filter.sortConditionList;

        // 過濾設定不完全的條件
        var filterConditionList = conditionList.Where((y) =>
        {
            if (y.operatorType == OperatorType.NONE)
            {
                return false;
            }

            if (y.propertyType == ConditionType.NONE)
            {
                return false;
            }

            if (y.propertyType == ConditionType.FINANCE && y.propertyFinancialType == FinancialType.NONE)
            {
                return false;
            }

            if (y.propertyType == ConditionType.SCORE && y.propertyScoreType == ScoreType.NONE)
            {
                return false;
            }

            if (y.compareType == ConditionType.NONE)
            {
                return false;
            }

            if (y.compareType == ConditionType.FINANCE && y.compareFinancialType == FinancialType.NONE)
            {
                return false;
            }

            if (y.compareType == ConditionType.SCORE && y.compareScoreType == ScoreType.NONE)
            {
                return false;
            }

            return true;
        });

        var filterSortList = this.get<GlobalData>().stockFinancialList.Select((x) => new FilterSort()
        {
            stockFinancial = x,
            studyScore = this.get<StudyModel>().studyScoreList.Find((y) => y.stockId == x.stockId),
        }).Where(x =>
        {
            return (
                filterConditionList
                    .Select((y) =>
                    {
                        var propertyValue = this.getPropertyValue(x, y);
                        var compareValue = this.getCompareValue(x, y);
                        return this.compare(y.operatorType, propertyValue, compareValue);
                    })
                    .Where((z) => z).Count() == filter.conditionList.Count
            );
        }).ToList();

        // 過濾設定不完全的條件
        var filterSortConditionList = sortConditionList.Where((y) =>
        {
            if (y.sortType == ConditionType.NONE)
            {
                return false;
            }

            if (y.sortType == ConditionType.FINANCE && y.sortFinancialType == FinancialType.NONE)
            {
                return false;
            }

            if (y.sortType == ConditionType.SCORE && y.sortScoreType == ScoreType.NONE)
            {
                return false;
            }

            return true;
        }).ToList();

        filterSortList.Sort((FilterSort _a, FilterSort _b) =>
        {
            foreach (var sortCondition in filterSortConditionList)
            {
                var a = this.getSortValue(_a, sortCondition);
                var b = this.getSortValue(_b, sortCondition);
                if (a == b)
                {
                    continue;
                }
                else
                {
                    if (b > a)
                    {
                        return 1;
                    }
                    else if (b < a)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            return 0;
        });

        var filterPropertyList = filterSortList.Select(x =>
        {
            return new List<ICell>() { new ICell { value = x.stockFinancial.stockId }, new ICell { value = x.stockFinancial.name }, new ICell { value = Common.numberToPercent(x.stockFinancial.SaveMargin, 2) } };
        }).ToList();
        this.grid.replace(new IGrid() { propertyList = filterPropertyList });
    }

    protected float getFinancialSortValue(IStockFinancialData stockFinancialData, FinancialType sortFinancialType)
    {
        switch (sortFinancialType)
        {
            case FinancialType.AnnualizedReturn:
                return stockFinancialData.AnnualizedReturn;
            case FinancialType.GrossProfitRatio:
                return stockFinancialData.GrossProfitRatio;
            case FinancialType.LongDebtRatio:
                return stockFinancialData.LongDebtRatio;
            case FinancialType.ONWR:
                return stockFinancialData.ONWR;
            case FinancialType.OperatingProfitMargin:
                return stockFinancialData.OperatingProfitMargin;
            case FinancialType.ReturnOfEquity_FirstQuartile:
                return stockFinancialData.ReturnOfEquity_FirstQuartile;
            case FinancialType.ReturnOfNetTangibleAssets_FirstQuartile:
                return stockFinancialData.ReturnOfNetTangibleAssets_FirstQuartile;
            case FinancialType.SalesToPrice:
                return stockFinancialData.SalesToPrice;
            case FinancialType.YoY:
                return stockFinancialData.YoY;
        }

        return 0f;
    }

    protected float getScoreSortValue(StudyScore studyScore, ScoreType sortScoreType)
    {
        var studySubScoreList = studyScore.studySubScoreList;
        if (sortScoreType == ScoreType.TOTAL)
        {
            return studySubScoreList.Select((x) => x.score).Sum();
        }
        else
        {
            var find = studySubScoreList.Find((x) => x.scoreType == sortScoreType);
            if (find != null)
            {
                return find.score;
            }
        }
        return 0;
    }

    protected float getSortValue(FilterSort filterSort, SortCondition sortCondition)
    {
        var sortType = sortCondition.sortType;
        var sortFinancialType = sortCondition.sortFinancialType;
        var sortScoreType = sortCondition.sortScoreType;
        switch (sortType)
        {
            case ConditionType.FINANCE:
                return this.getFinancialSortValue(filterSort.stockFinancial, sortFinancialType);
            case ConditionType.SCORE:
                return this.getScoreSortValue(filterSort.studyScore, sortScoreType);
        }

        return 0f;
    }
}

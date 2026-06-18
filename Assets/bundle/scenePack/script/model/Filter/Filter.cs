using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

public enum ConditionType
{
    NONE,
    SCORE,
    FINANCE,
    VALUE
}

public enum OperatorType
{
    NONE,
    GREATER,
    LESS,
    GREATER_OR_EQUAL,
    LESS_OR_EQUAL,
    EQUAL
}

public class Condition
{
    public int serialNo = 0;
    public ConditionType propertyType = ConditionType.NONE;
    public FinancialType propertyFinancialType = FinancialType.NONE;
    public ScoreType propertyScoreType = ScoreType.NONE;
    public OperatorType operatorType = OperatorType.NONE;
    public ConditionType compareType = ConditionType.NONE;
    public FinancialType compareFinancialType = FinancialType.NONE;
    public ScoreType compareScoreType = ScoreType.NONE;
    public float compareValue = 0;

    public Condition copy()
    {
        return new Condition()
        {
            serialNo = this.serialNo,
            propertyType = this.propertyType,
            propertyFinancialType = this.propertyFinancialType,
            propertyScoreType = this.propertyScoreType,
            operatorType = this.operatorType,
            compareType = this.compareType,
            compareFinancialType = this.compareFinancialType,
            compareScoreType = this.compareScoreType,
            compareValue = this.compareValue,
        };
    }

    public void update(Condition data)
    {
        propertyType = data.propertyType;
        propertyFinancialType = data.propertyFinancialType;
        propertyScoreType = data.propertyScoreType;
        operatorType = data.operatorType;
        compareType = data.compareType;
        compareFinancialType = data.compareFinancialType;
        compareScoreType = data.compareScoreType;
        compareValue = data.compareValue;
    }

    public static SerialCounter serialCounter = new SerialCounter();
}

public class SortCondition
{
    public int serialNo = 0;
    public ConditionType sortType = ConditionType.NONE;
    public FinancialType sortFinancialType = FinancialType.NONE;
    public ScoreType sortScoreType = ScoreType.NONE;

    public SortCondition copy()
    {
        return new SortCondition()
        {
            serialNo = this.serialNo,
            sortType = this.sortType,
            sortFinancialType = this.sortFinancialType,
            sortScoreType = this.sortScoreType,
        };
    }

    public void update(SortCondition data)
    {
        sortType = data.sortType;
        sortFinancialType = data.sortFinancialType;
        sortScoreType = data.sortScoreType;
    }

    public static SerialCounter serialCounter = new SerialCounter();
}

public class Filter
{
    public int serialNo = 0;
    public string name = "";
    public bool isEdit = false;
    public bool isDefault = false;
    public List<Condition> conditionList = new List<Condition>();
    public List<SortCondition> sortConditionList = new List<SortCondition>();

    public Filter copy()
    {
        return new Filter()
        {
            serialNo = this.serialNo,
            name = this.name,
            isEdit = this.isEdit,
            isDefault = this.isDefault,
            conditionList = this.conditionList.Select(x => x.copy()).ToList(),
            sortConditionList = this.sortConditionList.Select(x => x.copy()).ToList(),
        };
    }

    public void update(Filter data)
    {
        this.name = data.name;
        this.isEdit = data.isEdit;
        this.isDefault = data.isDefault;
        this.conditionList = data.conditionList.Select(x => x.copy()).ToList();
        this.sortConditionList = data.sortConditionList.Select(x => x.copy()).ToList();
    }


    public Condition getCondition(int serialNo)
    {
        return this.conditionList.Find(x => x.serialNo == serialNo);
    }

    public void setCondition(Condition conditionData)
    {
        var index = this.conditionList.FindIndex(x => x.serialNo == conditionData.serialNo);
        if (index > -1)
        {
            this.conditionList[index].update(conditionData);
        }
        else
        {
            this.conditionList.Add(conditionData);
        }
    }

    public void removeCondition(Condition conditionData)
    {
        var index = this.conditionList.FindIndex(x => x.serialNo == conditionData.serialNo);
        if (index > -1)
        {
            this.conditionList.RemoveAt(index);
        }
    }

    public SortCondition getSortCondition(int serialNo)
    {
        return this.sortConditionList.Find(x => x.serialNo == serialNo);
    }

    public void setSortCondition(SortCondition sortConditionData)
    {
        var index = this.sortConditionList.FindIndex(x => x.serialNo == sortConditionData.serialNo);
        if (index > -1)
        {
            this.sortConditionList[index].update(sortConditionData);
        }
        else
        {
            this.sortConditionList.Add(sortConditionData);
        }
    }

    public void removeSortCondition(SortCondition sortConditionData)
    {
        var index = this.sortConditionList.FindIndex(x => x.serialNo == sortConditionData.serialNo);
        if (index > -1)
        {
            this.sortConditionList.RemoveAt(index);
        }
    }

    public static SerialCounter serialCounter = new SerialCounter();
}

public class FilterConst
{
    public static List<ConditionType> propertyTypeList = new List<ConditionType>() { ConditionType.FINANCE, ConditionType.SCORE };
    public static List<FinancialType> propertyFinancialTypeList = new List<FinancialType>() {
        FinancialType.AnnualizedReturn,
        FinancialType.GrossProfitRatio,
        FinancialType.LongDebtRatio,
        FinancialType.ONWR,
        FinancialType.OperatingProfitMargin,
        FinancialType.ReturnOfEquity_FirstQuartile,
        FinancialType.ReturnOfNetTangibleAssets_FirstQuartile,
        FinancialType.SalesToPrice,
        FinancialType.YoY,
        FinancialType.QuarterLength,
        FinancialType.SaveMargin,
        FinancialType.ReturnOfRequest,
        FinancialType.IntrinsicValue_FirstQuartile,
        FinancialType.IntrinsicValue_Median,
        FinancialType.IntrinsicValue_ThirdQuartile,
        FinancialType.price,
};
    public static List<ScoreType> propertyScoreTypeList = new List<ScoreType>() {
ScoreType.INVESTMENT, ScoreType.PRODUCT, ScoreType.ASSET, ScoreType.SPECULATION, ScoreType.FINANCE, ScoreType.MANAGER};
    public static List<OperatorType> operatorList = new List<OperatorType>() { OperatorType.GREATER, OperatorType.GREATER_OR_EQUAL, OperatorType.EQUAL, OperatorType.LESS_OR_EQUAL, OperatorType.LESS };
    public static List<ConditionType> compareTypeList = new List<ConditionType>() {
    ConditionType.FINANCE, ConditionType.SCORE, ConditionType.VALUE
};
    public static List<FinancialType> compareFinancialTypeList = new List<FinancialType>() {
        FinancialType.AnnualizedReturn,
        FinancialType.GrossProfitRatio,
        FinancialType.LongDebtRatio,
        FinancialType.ONWR,
        FinancialType.OperatingProfitMargin,
        FinancialType.ReturnOfEquity_FirstQuartile,
        FinancialType.ReturnOfNetTangibleAssets_FirstQuartile,
        FinancialType.SalesToPrice,
        FinancialType.YoY
};
    public static List<ScoreType> compareScoreTypeList = new List<ScoreType>() {
ScoreType.INVESTMENT, ScoreType.PRODUCT, ScoreType.ASSET, ScoreType.SPECULATION, ScoreType.FINANCE, ScoreType.MANAGER

};

    public static string getOperatorCaption(OperatorType operatorType)
    {
        switch (operatorType)
        {
            case OperatorType.GREATER:
                return ">";
            case OperatorType.GREATER_OR_EQUAL:
                return ">=";
            case OperatorType.EQUAL:
                return "==";
            case OperatorType.LESS_OR_EQUAL:
                return "<=";
            case OperatorType.LESS:
                return "<";
        }

        return "";
    }

    public static string getScoreTypeCaption(ScoreType scoreType)
    {
        switch (scoreType)
        {
            case ScoreType.INVESTMENT:
                return "投資價值";
            case ScoreType.PRODUCT:
                return "產品獲利能力";
            case ScoreType.ASSET:
                return "資產獲利能力";
            case ScoreType.SPECULATION:
                return "投機價值";
            case ScoreType.FINANCE:
                return "財務健康";
            case ScoreType.MANAGER:
                return "管理能力";
            case ScoreType.TOTAL:
                return "總分";
        }

        return "";
    }

    public static string getFinancialTypeCaption(FinancialType financialType)
    {
        switch (financialType)
        {
            case FinancialType.AnnualizedReturn:
                return "年化報酬率";
            case FinancialType.LongDebtRatio:
                return "長期債務比率";
            case FinancialType.GrossProfitRatio:
                return "毛利率";
            case FinancialType.ONWR:
                return "本業資金占比";
            case FinancialType.OperatingProfitMargin:
                return "營業利益率";
            case FinancialType.ReturnOfEquity_FirstQuartile:
                return "股東權益報酬率(Q1)";
            case FinancialType.ReturnOfNetTangibleAssets_FirstQuartile:
                return "淨有形資產收益率(Q1)";
            case FinancialType.ReturnOfNetTangibleAssets_Median:
                return "淨有形資產收益率(Q2)";
            case FinancialType.SalesToPrice:
                return "營收市值比";
            case FinancialType.YoY:
                return "營收年增率";
            case FinancialType.QuarterLength:
                return "季度數量";
            case FinancialType.SaveMargin:
                return "安全邊際";
            case FinancialType.ReturnOfRequest:
                return "要求報酬率";
            case FinancialType.IntrinsicValue_FirstQuartile:
                return "內在價值(Q1)";
            case FinancialType.IntrinsicValue_Median:
                return "內在價值(Q2)";
            case FinancialType.IntrinsicValue_ThirdQuartile:
                return "內在價值(Q3)";
            case FinancialType.price:
                return "收盤價";
        }

        return "";
    }

    public static string getConditionTypeCaption(ConditionType conditionType)
    {
        switch (conditionType)
        {
            case ConditionType.FINANCE:
                return "財務比率";
            case ConditionType.SCORE:
                return "評分";
            case ConditionType.VALUE:
                return "數值";
        }

        return "";
    }

    public static ConditionType getPropertyType(int index)
    {
        if (-1 < index && index < FilterConst.propertyTypeList.Count)
        {
            return FilterConst.propertyTypeList[index];
        }
        else
        {
            return ConditionType.NONE;
        }
    }

    public static FinancialType getPropertyFinancialType(int index)
    {
        if (-1 < index && index < FilterConst.propertyFinancialTypeList.Count)
        {
            return FilterConst.propertyFinancialTypeList[index];
        }
        else
        {
            return FinancialType.NONE;
        }
    }

    public static ScoreType getPropertyScoreType(int index)
    {
        if (-1 < index && index < FilterConst.propertyScoreTypeList.Count)
        {
            return FilterConst.propertyScoreTypeList[index];
        }
        else
        {
            return ScoreType.NONE;
        }
    }

    public static OperatorType getOperator(int index)
    {
        if (-1 < index && index < FilterConst.operatorList.Count)
        {
            return FilterConst.operatorList[index];
        }
        else
        {
            return OperatorType.NONE;
        }
    }

    public static ConditionType getCompareType(int index)
    {
        if (-1 < index && index < FilterConst.compareTypeList.Count)
        {
            return FilterConst.compareTypeList[index];
        }
        else
        {
            return ConditionType.NONE;
        }
    }

    public static FinancialType getCompareFinancialType(int index)
    {
        if (-1 < index && index < FilterConst.compareFinancialTypeList.Count)
        {
            return FilterConst.compareFinancialTypeList[index];
        }
        else
        {
            return FinancialType.NONE;
        }
    }

    public static ScoreType getCompareScoreType(int index)
    {
        if (-1 < index && index < FilterConst.compareScoreTypeList.Count)
        {
            return FilterConst.compareScoreTypeList[index];
        }
        else
        {
            return ScoreType.NONE;
        }
    }

    public static List<ConditionType> sortTypeList = new List<ConditionType>() {
    ConditionType.FINANCE, ConditionType.SCORE
};

    public static List<FinancialType> sortFinancialTypeList = new List<FinancialType>() {
    FinancialType.AnnualizedReturn,
        FinancialType.GrossProfitRatio,
        FinancialType.LongDebtRatio,
        FinancialType.ONWR,
        FinancialType.OperatingProfitMargin,
        FinancialType.ReturnOfEquity_FirstQuartile,
        FinancialType.ReturnOfNetTangibleAssets_FirstQuartile,
        FinancialType.SalesToPrice,
        FinancialType.YoY
 };
    public static List<ScoreType> sortScoreTypeList = new List<ScoreType>() {
ScoreType.INVESTMENT, ScoreType.PRODUCT, ScoreType.ASSET, ScoreType.SPECULATION, ScoreType.FINANCE, ScoreType.MANAGER, ScoreType.TOTAL
};

    public static ConditionType getSortType(int index)
    {
        if (-1 < index && index < FilterConst.sortTypeList.Count)
        {
            return FilterConst.sortTypeList[index];
        }
        else
        {
            return ConditionType.NONE;
        }
    }

    public static FinancialType getSortFinancialType(int index)
    {
        if (-1 < index && index < FilterConst.sortFinancialTypeList.Count)
        {
            return FilterConst.sortFinancialTypeList[index];
        }
        else
        {
            return FinancialType.NONE;
        }
    }

    public static ScoreType getSortScoreType(int index)
    {
        if (-1 < index && index < FilterConst.sortScoreTypeList.Count)
        {
            return FilterConst.sortScoreTypeList[index];
        }
        else
        {
            return ScoreType.NONE;
        }
    }

    // caption
    public static List<ScoreType> scoreTypeList = new List<ScoreType>() {
    ScoreType.INVESTMENT, ScoreType.PRODUCT, ScoreType.ASSET, ScoreType.SPECULATION, ScoreType.FINANCE, ScoreType.MANAGER
};
}
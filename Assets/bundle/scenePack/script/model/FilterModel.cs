using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public enum FilterEidtStatus
{
    None,
    Condition,
    SortCondition,
}

public class IStorageFilterList
{
    public List<Filter> list;
}

public class ItemFilterStockPropertyData
{
    public ConditionType conditionType;
    public FinancialType financialType;
    public ScoreType scoreType;
    public float value;
}

public class ItemFilterStockData
{
    public string stockId;
    public List<ItemFilterStockPropertyData> itemFilterStockPropertyList;
}

public class FilterSort { 
    public string stockId;
    public IStockFinancialData stockFinancial;
    public StudyScore studyScore;
}

public class FilterModel : BaseModel
{
    public string fileName = "filter";
    public override void Awake()
    {
        base.Awake();

        this._filterList = this.getFilterList();
        int maxSerialCounter_Filter = this._filterList.Select(x => x.serialNo).Max();
        Filter.serialCounter.replace(maxSerialCounter_Filter);
        int maxSerialCounter_Condition = this._filterList.Select(x => x.conditionList.Select(y => y.serialNo).Max()).Max();
        Condition.serialCounter.replace(maxSerialCounter_Condition);
        int maxSserialCounter_SortCondition = this._filterList.Select(x => x.sortConditionList.Select(y => y.serialNo).Max()).Max();
        SortCondition.serialCounter.replace(maxSserialCounter_SortCondition);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }


    protected List<Filter> getFilterList()
    {
        IStorageFilterList jsonObject;
        if (this.get<SaveFile>().exists(this.fileName))
        {
            var loadObject = this.get<SaveFile>().load<IStorageFilterList>(this.fileName);
            if (loadObject != null)
            {
                jsonObject = loadObject;
            }
            else
            {
                jsonObject = this.getDefaultFilterList();
            }
        }
        else
        {
            jsonObject = this.getDefaultFilterList();
        }

        return jsonObject.list;
    }

    protected IStorageFilterList getDefaultFilterList()
    {
        return new IStorageFilterList()
        {
            list = new List<Filter>() {
                new Filter() {
                    name = "預設",
                    isDefault = true,
                    conditionList = new List<Condition> () {
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.ReturnOfNetTangibleAssets_FirstQuartile,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.12f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.ReturnOfEquity_FirstQuartile,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.12f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.SaveMargin,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.2f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.AnnualizedReturn,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.05f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        }
                    },
                    sortConditionList = new List<SortCondition> () {
                        new SortCondition () {
                            sortType = ConditionType.SCORE,
                            sortFinancialType = FinancialType.NONE,
                            sortScoreType = ScoreType.SPECULATION,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        },
                        new SortCondition () {
                            sortType = ConditionType.FINANCE,
                            sortFinancialType = FinancialType.AnnualizedReturn,
                            sortScoreType = ScoreType.NONE,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        },
                        new SortCondition () {
                            sortType = ConditionType.SCORE,
                            sortFinancialType = FinancialType.NONE,
                            sortScoreType = ScoreType.TOTAL,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        },
                        new SortCondition () {
                            sortType = ConditionType.FINANCE,
                            sortFinancialType = FinancialType.YoY,
                            sortScoreType = ScoreType.NONE,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        },
                        new SortCondition () {
                            sortType = ConditionType.SCORE,
                            sortFinancialType = FinancialType.NONE,
                            sortScoreType = ScoreType.INVESTMENT,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        }
                    }
                },
                new Filter() {
                    name = "價值",
                    isDefault = true,
                    conditionList = new List<Condition> () {
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.ReturnOfNetTangibleAssets_FirstQuartile,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.12f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.ReturnOfEquity_FirstQuartile,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.12f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.SaveMargin,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.2f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.AnnualizedReturn,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.05f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        }
                    },
                    sortConditionList = new List<SortCondition> () {
                        new SortCondition () {
                            sortType = ConditionType.SCORE,
                            sortFinancialType = FinancialType.NONE,
                            sortScoreType = ScoreType.INVESTMENT,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        },
                        new SortCondition () {
                            sortType = ConditionType.SCORE,
                            sortFinancialType = FinancialType.NONE,
                            sortScoreType = ScoreType.TOTAL,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        }
                    }
                },
                new Filter() {
                    name = "投機",
                    isDefault = true,
                    conditionList = new List<Condition> () {
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.YoY,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.1f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.ReturnOfNetTangibleAssets_FirstQuartile,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.08f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.ReturnOfEquity_FirstQuartile,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.08f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.SaveMargin,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.AnnualizedReturn,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.05f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        }
                    },
                    sortConditionList = new List<SortCondition> () {
                        new SortCondition () {
                            sortType = ConditionType.SCORE,
                            sortFinancialType = FinancialType.NONE,
                            sortScoreType = ScoreType.SPECULATION,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        },
                        new SortCondition () {
                            sortType = ConditionType.SCORE,
                            sortFinancialType = FinancialType.NONE,
                            sortScoreType = ScoreType.TOTAL,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        }
                    }
                },
                new Filter() {
                    name = "安全",
                    isDefault = true,
                    conditionList = new List<Condition> () {
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.ReturnOfEquity_FirstQuartile,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.12f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.SaveMargin,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.2f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        },
                        new Condition() {
                            propertyType = ConditionType.FINANCE,
                            propertyFinancialType = FinancialType.AnnualizedReturn,
                            propertyScoreType = ScoreType.NONE,
                            operatorType = OperatorType.GREATER_OR_EQUAL,
                            compareType = ConditionType.VALUE,
                            compareFinancialType = FinancialType.NONE,
                            compareScoreType = ScoreType.NONE,
                            compareValue = 0.08f,
                            serialNo = Condition.serialCounter.generateSerialNo(),
                        }
                    },
                    sortConditionList = new List<SortCondition> () {
                        new SortCondition () {
                            sortType = ConditionType.SCORE,
                            sortFinancialType = FinancialType.NONE,
                            sortScoreType = ScoreType.TOTAL,
                            serialNo = SortCondition.serialCounter.generateSerialNo(),
                        }
                    }
                }
            }
        };
    }

    protected List<Filter> _filterList = new List<Filter>();
    public List<Filter> filterList
    {
        get
        {
            return this._filterList;
        }
    }

    public void removeFilter(Filter filter)
    {
        var index = this.filterList.FindIndex((x) => x.serialNo == filter.serialNo);
        if (index > -1)
        {
            this.filterList.RemoveAt(index);
            this.get<SaveFile>().save(this.fileName, new IStorageFilterList() { list = this.filterList });
            this.emit("filterList", this._filterList);
        }
    }

    public void setFilter(Filter filter)
    {
        var find = this.filterList.Find((x) => x.serialNo == filter.serialNo);
        if (find != null)
        {
            find.update(filter);
        }
        else {
            this.filterList.Add(filter);
        }
        this.get<SaveFile>().save(this.fileName, new IStorageFilterList() { list = this.filterList });
        this.emit("filterList", this._filterList);
    }

    protected Filter _filter = null;
    public Filter filter
    {
        get
        {
            return this._filter;
        }
    }

    public void useFilter(Filter filter)
    {
        this._filter = filter;
        this.emit("filter", filter);
    }

    protected Filter _editFilter = null;
    public Filter editFilter
    {
        get
        {
            return this._editFilter;
        }
    }

    public void setEditFilter(Filter editFilter)
    {
        this._editFilter = editFilter;
        this.emit("editFilter", this._editFilter);
    }

    private bool _isOpenFilter = false;
    public bool isOpenFilter
    {
        get
        {
            return this._isOpenFilter;
        }
    }

    public void switchFilter(bool isOpenFilter)
    {
        this._isOpenFilter = isOpenFilter;
        this.emit("isOpenFilter", this._isOpenFilter);
    }

    private bool _isEdit = false;
    public bool isEdit
    {
        get
        {
            return this._isEdit;
        }
    }

    public void switchEdit(bool isEdit)
    {
        this._isEdit = isEdit;
        this.emit("isEdit", this._isEdit);
    }

    protected Condition _editConditionData = null;
    public Condition editConditionData
    {
        get
        {
            return this._editConditionData;
        }
    }

    public void setEditConditionData(Condition editConditionData)
    {
        this._editConditionData = editConditionData;
        this.emit("editConditionData", this._editConditionData);
    }

    private FilterEidtStatus _editStatus = FilterEidtStatus.None;
    public FilterEidtStatus editStatus
    {
        get
        {
            return this._editStatus;
        }
    }

    public void switchEditStatus(FilterEidtStatus editStatus)
    {
        this._editStatus = editStatus;
        this.emit("editStatus", this._editStatus);
    }


    protected SortCondition _editSortConditionData = null;
    public SortCondition editSortConditionData
    {
        get
        {
            return this._editSortConditionData;
        }
    }

    public void setEditSortConditionData(SortCondition editSortConditionData)
    {
        this._editSortConditionData = editSortConditionData;
        this.emit("editSortConditionData", this._editSortConditionData);
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GroupFilterSortConditionEdit : BaseUIComponent
{
    [SerializeField]
    protected Button btnOk;

    [SerializeField]
    protected Button btnCancel;

    [SerializeField]
    protected ComboBox comboBoxSortType;

    [SerializeField]
    protected ComboBox comboBoxSortValue;

    private void Awake()
    {
        this.btnOk.onClick.AddListener(this.doOk);
        this.btnCancel.onClick.AddListener(this.doCancel);

        this.comboBoxSortType.on("onItemClick", this.doSortTypeItemClick);
        this.comboBoxSortValue.on("onItemClick", this.doSortValueItemClick);

        this.on<FilterModel>("editSortConditionData", this.refreshEditSortConditionData);
    }

    protected void doOk()
    {
        var editFilter = this.get<FilterModel>().editFilter;
        editFilter.setSortCondition(this.get<FilterModel>().editSortConditionData);
        this.get<FilterModel>().setEditFilter(editFilter);
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.None);
    }

    protected void doCancel()
    {
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.None);
    }

    protected void refreshEditSortConditionData(object obj)
    {
        var sortConditionData = obj as SortCondition;
        var sortType = sortConditionData.sortType;

        var sortTypeList  = FilterConst.sortTypeList;
        this.comboBoxSortType.selections = sortTypeList.Select((x) => FilterConst.getConditionTypeCaption(x)).ToList();
        this.comboBoxSortType.selectedIndex = sortTypeList.FindIndex((x) => x == sortType);
        this.refreshSortType(sortType);
    }


    protected void doSortTypeItemClick(object obj)
    {
        var comboBox = obj as ComboBox;
        var sortType = FilterConst.getSortType(comboBox.selectedIndex);
        var editFilter  = this.get<FilterModel>().editFilter;
        var editSortConditionData = this.get<FilterModel>().editSortConditionData;
        editSortConditionData.sortType = sortType;
        this.get<FilterModel>().setEditSortConditionData(editSortConditionData);
    }

    protected void refreshSortType(ConditionType sortType)
    {
        var editSortConditionData = this.get<FilterModel>().editSortConditionData;
        switch (sortType)
        {
            case ConditionType.FINANCE:
                var sortFinancialType  = editSortConditionData.sortFinancialType;
                var sortFinancialTypeList = FilterConst.sortFinancialTypeList;
                this.comboBoxSortValue.selections = sortFinancialTypeList.Select((x) => FilterConst.getFinancialTypeCaption(x)).ToList();
                this.comboBoxSortValue.selectedIndex = sortFinancialTypeList.FindIndex((x) => x == sortFinancialType);
                break;

            case ConditionType.SCORE:
                var sortScoreType  = editSortConditionData.sortScoreType;
                var sortScoreTypeList = FilterConst.sortScoreTypeList;
                this.comboBoxSortValue.selections = sortScoreTypeList.Select((x) => FilterConst.getScoreTypeCaption(x)).ToList();
                this.comboBoxSortValue.selectedIndex = sortScoreTypeList.FindIndex((x) => x == sortScoreType);
                break;

            default:
                this.comboBoxSortValue.selections = new List<string>();
                this.comboBoxSortValue.selectedIndex = -1;
                break;
        }
    }

    protected void doSortValueItemClick(object obj)
    {
        var comboBox = obj as ComboBox;
        var editFilter = this.get<FilterModel>().editFilter;
        var editSortConditionData = this.get<FilterModel>().editSortConditionData;
        var sortType = editSortConditionData.sortType;
        switch (sortType)
        {
            case ConditionType.FINANCE:
                editSortConditionData.sortFinancialType = FilterConst.getSortFinancialType(comboBox.selectedIndex);
                break;
            case ConditionType.SCORE:
                editSortConditionData.sortScoreType = FilterConst.getSortScoreType(comboBox.selectedIndex);
                break;
        }
        this.get<FilterModel>().setEditSortConditionData(editSortConditionData);
    }
}

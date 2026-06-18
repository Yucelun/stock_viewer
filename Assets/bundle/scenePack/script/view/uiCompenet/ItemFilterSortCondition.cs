using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemFilterSortCondition : ItemRenderer
{
    [SerializeField]
    protected Button btnNew;

    [SerializeField]
    protected GameObject groupSortCondition;

    [SerializeField]
    protected Text textSortCondition;

    [SerializeField]
    protected Button btnDelete;

    [SerializeField]
    protected Button btnEdit;

    private void Awake()
    {
        this.btnNew.onClick.AddListener(this.doNew);
        this.btnDelete.onClick.AddListener(this.doDelete);
        this.btnEdit.onClick.AddListener(this.doEdit);
    }

    protected override void doDataChanged() {
        if (this.data == null)
        {
            this.btnNew.gameObject.SetActive(true);
            this.groupSortCondition.SetActive(false);
        }
        else
        {
            this.btnNew.gameObject.SetActive(false);
            this.groupSortCondition.SetActive(true);

            var sortConditionData = this.data as SortCondition;
            this.textSortCondition.text = this.getPropertyCaption(sortConditionData);
        }
    }

    protected void doDelete()
    {
        var editFilter = this.get<FilterModel>().editFilter;
        var sortConditionData = this.data as SortCondition;
        editFilter.removeSortCondition(sortConditionData);
        this.get<FilterModel>().setEditFilter(editFilter);
    }

    protected void doNew()
    {
        this.get<FilterModel>().setEditSortConditionData(new SortCondition()
        {
            serialNo = SortCondition.serialCounter.generateSerialNo(),
        });
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.SortCondition);
    }

    protected void doEdit()
    {
        this.get<FilterModel>().setEditSortConditionData(this.data as SortCondition);
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.SortCondition);
    }

    protected string getPropertyCaption(SortCondition sortCondition)
    {
        switch (sortCondition.sortType)
        {
            case ConditionType.FINANCE:
                return FilterConst.getFinancialTypeCaption(sortCondition.sortFinancialType);
            case ConditionType.SCORE:
                return FilterConst.getScoreTypeCaption(sortCondition.sortScoreType);
        }
        return "";
    }
}

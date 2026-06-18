using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemFilterCondition : ItemRenderer
{
    [SerializeField]
    protected Button btnNew;

    [SerializeField]
    protected GameObject groupCondition;

    [SerializeField]
    protected Text textCondition;

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
            this.groupCondition.SetActive(false);
        }
        else
        {
            this.btnNew.gameObject.SetActive(false);
            this.groupCondition.SetActive(true);

            var conditionData = this.data as Condition;
            this.textCondition.text = string.Format("{0}{1}{2}", this.getPropertyCaption(conditionData), this.getOperatorCaption(conditionData), this.getCompareTarget(conditionData));
        }
    }

    protected void doDelete()
    {
        var editFilter = this.get<FilterModel>().editFilter;
        var conditionData = this.data as Condition;
        editFilter.removeCondition(conditionData);
        this.get<FilterModel>().setEditFilter(editFilter);
    }

    protected void doNew()
    {
        this.get<FilterModel>().setEditConditionData(new Condition()
        {
            serialNo = Condition.serialCounter.generateSerialNo(),
        });
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.Condition);
    }

    protected void doEdit()
    {
        this.get<FilterModel>().setEditConditionData(this.data as Condition);
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.Condition);
    }

    protected string getPropertyCaption(Condition condition)
    {
        switch (condition.propertyType)
        {
            case ConditionType.FINANCE:
                return FilterConst.getFinancialTypeCaption(condition.propertyFinancialType);
            case ConditionType.SCORE:
                return FilterConst.getScoreTypeCaption(condition.propertyScoreType);
        }
        return "";
    }

    protected string getOperatorCaption(Condition condition)
    {
        return FilterConst.getOperatorCaption(condition.operatorType);
    }

    protected string getCompareTarget(Condition condition)
    {
        switch (condition.compareType)
        {
            case ConditionType.FINANCE:
                return FilterConst.getFinancialTypeCaption(condition.compareFinancialType);
            case ConditionType.SCORE:
                return FilterConst.getScoreTypeCaption(condition.compareScoreType);
            case ConditionType.VALUE:
                return condition.compareValue.ToString();
        }
        return "";
    }
}

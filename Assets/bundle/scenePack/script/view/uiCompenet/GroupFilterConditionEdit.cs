using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GroupFilterConditionEdit : BaseUIComponent
{
    [SerializeField]
    protected Button btnOk;

    [SerializeField]
    protected Button btnCancel;

    [SerializeField]
    protected ComboBox comboBoxPropertyType;

    [SerializeField]
    protected ComboBox comboBoxPropertyValue;

    [SerializeField]
    protected ComboBox comboBoxOperator;

    [SerializeField]
    protected ComboBox comboBoxCompareType;

    [SerializeField]
    protected ComboBox comboBoxCompareValue;

    [SerializeField]
    protected InputField inputFieldCompareValue;

    private void Awake()
    {
        this.btnOk.onClick.AddListener(this.doOk);
        this.btnCancel.onClick.AddListener(this.doCancel);

        this.comboBoxPropertyType.on("onItemClick", this.doPropertyTypeItemClick);
        this.comboBoxPropertyValue.on("onItemClick", this.doPropertyValueItemClick);
        this.comboBoxOperator.on("onItemClick", this.doOperatorItemClick);
        this.comboBoxCompareType.on("onItemClick", this.doCompareTypeItemClick);
        this.comboBoxCompareValue.on("onItemClick", this.doCompareValueItemClick);
        this.inputFieldCompareValue.onEndEdit.AddListener(this.doEndEditCompareValue);

        this.on<FilterModel>("editConditionData", this.refreshEditConditionData);
    }

    protected void doOk()
    {
        var editFilter = this.get<FilterModel>().editFilter;
        editFilter.setCondition(this.get<FilterModel>().editConditionData);
        this.get<FilterModel>().setEditFilter(editFilter);
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.None);
    }

    protected void doCancel()
    {
        this.get<FilterModel>().switchEditStatus(FilterEidtStatus.None);
    }


    protected void refreshEditConditionData(object obj)
    {
        var conditionData = obj as Condition;
        var propertyType = conditionData.propertyType;
        var operatorType = conditionData.operatorType;
        var compareType = conditionData.compareType;

        var propertyTypeList = FilterConst.propertyTypeList;
        var operatorList = FilterConst.operatorList;
        var compareTypeList = FilterConst.compareTypeList;
        this.comboBoxPropertyType.selections = propertyTypeList.Select((x) => FilterConst.getConditionTypeCaption(x)).ToList();
        this.comboBoxPropertyType.selectedIndex = propertyTypeList.FindIndex((x) => x == propertyType);
        this.refreshPropertyType(propertyType);

        this.comboBoxOperator.selections = operatorList.Select((x) => FilterConst.getOperatorCaption(x)).ToList();
        this.comboBoxOperator.selectedIndex = operatorList.FindIndex((x) => x == operatorType);

        this.comboBoxCompareType.selections = compareTypeList.Select((x) => FilterConst.getConditionTypeCaption(x)).ToList();
        this.comboBoxCompareType.selectedIndex = compareTypeList.FindIndex((x) => x == compareType);
        this.refreshCompareType(compareType);
    }

    protected void doEndEditCompareValue(string text)
    {
        var compareValue = 0.0f;
        if (float.TryParse(text, out compareValue))
        {
            var editConditionData = this.get<FilterModel>().editConditionData;
            editConditionData.compareValue = compareValue;
            this.get<FilterModel>().setEditConditionData(editConditionData);
        }
    }

    protected void doOperatorItemClick(object obj)
    {
        var comboBox = obj as ComboBox;
        var operatorType = FilterConst.getOperator(comboBox.selectedIndex);
        var editConditionData = this.get<FilterModel>().editConditionData;
        editConditionData.operatorType = operatorType;
        this.get<FilterModel>().setEditConditionData(editConditionData);
    }


    protected void doCompareTypeItemClick(object obj)
    {
        var comboBox = obj as ComboBox;
        var compareType = FilterConst.getCompareType(comboBox.selectedIndex);
        var editConditionData = this.get<FilterModel>().editConditionData;
        editConditionData.compareType = compareType;
        this.get<FilterModel>().setEditConditionData(editConditionData);
    }

    protected void refreshCompareType(ConditionType compareType)
    {
        var editConditionData = this.get<FilterModel>().editConditionData;
        switch (compareType)
        {
            case ConditionType.FINANCE:
                this.comboBoxCompareValue.gameObject.SetActive(true);
                this.inputFieldCompareValue.gameObject.SetActive(false);

                var compareFinancialType = editConditionData.compareFinancialType;
                var compareFinancialTypeList = FilterConst.compareFinancialTypeList;
                this.comboBoxCompareValue.selections = compareFinancialTypeList.Select((x) => FilterConst.getFinancialTypeCaption(x)).ToList();
                this.comboBoxCompareValue.selectedIndex = compareFinancialTypeList.FindIndex((x) => x == compareFinancialType);
                break;

            case ConditionType.SCORE:
                this.comboBoxCompareValue.gameObject.SetActive(true);
                this.inputFieldCompareValue.gameObject.SetActive(false);

                var compareScoreType = editConditionData.compareScoreType;
                var compareScoreTypeList = FilterConst.compareScoreTypeList;
                this.comboBoxCompareValue.selections = compareScoreTypeList.Select((x) => FilterConst.getScoreTypeCaption(x)).ToList();
                this.comboBoxCompareValue.selectedIndex = compareScoreTypeList.FindIndex((x) => x == compareScoreType);
                break;

            case ConditionType.VALUE:
                this.comboBoxCompareValue.gameObject.SetActive(false);
                this.inputFieldCompareValue.gameObject.SetActive(true);

                var compareValue = editConditionData.compareValue;
                this.inputFieldCompareValue.SetTextWithoutNotify(compareValue.ToString());
                break;
            default:
                this.comboBoxCompareValue.gameObject.SetActive(true);
                this.inputFieldCompareValue.gameObject.SetActive(false);

                this.comboBoxCompareValue.selections = new List<string>();
                this.comboBoxCompareValue.selectedIndex = -1;
                this.inputFieldCompareValue.SetTextWithoutNotify("");
                break;
        }
    }

    protected void doPropertyTypeItemClick(object obj)
    {
        var comboBox = obj as ComboBox;
        var propertyType = FilterConst.getPropertyType(comboBox.selectedIndex);
        var editConditionData = this.get<FilterModel>().editConditionData;
        editConditionData.propertyType = propertyType;
        this.get<FilterModel>().setEditConditionData(editConditionData);
    }

    protected void refreshPropertyType(ConditionType propertyType)
    {
        var editConditionData = this.get<FilterModel>().editConditionData;
        switch (propertyType)
        {
            case ConditionType.FINANCE:
                var propertyFinancialType = editConditionData.propertyFinancialType;
                var propertyFinancialTypeList = FilterConst.propertyFinancialTypeList;
                this.comboBoxPropertyValue.selections = propertyFinancialTypeList.Select((x) => FilterConst.getFinancialTypeCaption(x)).ToList();
                this.comboBoxPropertyValue.selectedIndex = propertyFinancialTypeList.FindIndex((x) => x == propertyFinancialType);
                break;

            case ConditionType.SCORE:
                var propertyScoreType = editConditionData.propertyScoreType;
                var propertyScoreTypeList = FilterConst.propertyScoreTypeList;
                this.comboBoxPropertyValue.selections = propertyScoreTypeList.Select((x) => FilterConst.getScoreTypeCaption(x)).ToList();
                this.comboBoxPropertyValue.selectedIndex = propertyScoreTypeList.FindIndex((x) => x == propertyScoreType);
                break;

            default:
                this.comboBoxPropertyValue.selections = new List<string>();
                this.comboBoxPropertyValue.selectedIndex = -1;
                break;
        }
    }
    protected void doPropertyValueItemClick(object obj)
    {
        var comboBox = obj as ComboBox;
        var editConditionData = this.get<FilterModel>().editConditionData;
        var propertyType = editConditionData.propertyType;
        switch (propertyType)
        {
            case ConditionType.FINANCE:
                editConditionData.propertyFinancialType = FilterConst.getPropertyFinancialType(comboBox.selectedIndex);
                break;
            case ConditionType.SCORE:
                editConditionData.propertyScoreType = FilterConst.getPropertyScoreType(comboBox.selectedIndex);

                break;
        }
        this.get<FilterModel>().setEditConditionData(editConditionData);
    }

    protected void doCompareValueItemClick(object obj)
    {
        var comboBox = obj as ComboBox;
        var editConditionData = this.get<FilterModel>().editConditionData;
        var compareType = editConditionData.compareType;
        switch (compareType)
        {
            case ConditionType.FINANCE:
                editConditionData.compareFinancialType = FilterConst.getCompareFinancialType(comboBox.selectedIndex);
                break;
            case ConditionType.SCORE:
                editConditionData.compareScoreType = FilterConst.getCompareScoreType(comboBox.selectedIndex);
                break;
        }
        this.get<FilterModel>().setEditConditionData(editConditionData);
    }
}

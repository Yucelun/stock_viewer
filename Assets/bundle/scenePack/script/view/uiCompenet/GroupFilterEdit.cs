using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GroupFilterEdit : BaseUIComponent
{
    [SerializeField]
    protected Button btnOk;

    [SerializeField]
    protected Button btnClose;

    [SerializeField]
    protected InputField inputFieldFilterName;

    [SerializeField]
    protected List listFilterCondition;


    [SerializeField]
    protected List listFilterSortCondition;

    protected void Awake()
    {
        this.btnOk.onClick.AddListener(this.doOk);
        this.btnClose.onClick.AddListener(this.doClose);
        this.inputFieldFilterName.onValueChanged.AddListener(this.doFilterNameValueChanged);

        this.on<FilterModel>("editFilter", this.refreshEditFilter);
    }

    protected void Start()
    {
        this.refreshEditFilter(this.get<FilterModel>().editFilter);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected void refreshEditFilter(object obj)
    {
        var editFilter = obj as Filter;
        var name = editFilter.name;
        var conditionList = editFilter.conditionList;
        var sortConditionList = editFilter.sortConditionList;

        this.inputFieldFilterName.SetTextWithoutNotify(name);
        var _conditionList = conditionList.Select(x => x).ToList<object>();
        _conditionList.Add(null);

        this.listFilterCondition.replace(_conditionList, true); 

        var _sortConditionList = sortConditionList.Select(x => x).ToList<object>();
        _sortConditionList.Add(null);
        this.listFilterSortCondition.replace(_sortConditionList, true);
    }

    protected void doFilterNameValueChanged(string value)
    {
        var editFilter = this.get<FilterModel>().editFilter;
        editFilter.name = value;
        this.get<FilterModel>().setEditFilter(editFilter);
    }

    protected void doOk()
    {
        this.emit("onOk", null);
    }

    protected void doClose()
    {
        this.emit("onClose", null);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiFilterEdit : BaseUI
{
    [SerializeField]
    protected GroupFilterEdit groupFilterEdit;

    [SerializeField]
    protected GroupFilterConditionEdit groupFilterConditionEdit;

    [SerializeField]
    protected GroupFilterSortConditionEdit groupFilterSortConditionEdit;

    protected override void Awake()
    {
        base.Awake();

        this.on<FilterModel>("editStatus", this.refresheEditStatus);
        this.groupFilterEdit.on("onOk", this.doOk);
        this.groupFilterEdit.on("onClose", this.doClose);

    }

    private void Start()
    {
        this.refresheEditStatus(this.get<FilterModel>().editStatus);
    }

    protected void doOk(object obj)
    {
        var editFilter = this.get<FilterModel>().editFilter;
        this.get<FilterModel>().setFilter(editFilter);

        this.exit();
    }

    protected void doClose(object obj)
    {
        this.exit();
    }

    protected void refresheEditStatus(object obj)
    {
        var editStatus = (FilterEidtStatus)obj;
        this.groupFilterEdit.gameObject.SetActive(editStatus == FilterEidtStatus.None);
        this.groupFilterConditionEdit.gameObject.SetActive(editStatus == FilterEidtStatus.Condition);
        this.groupFilterSortConditionEdit.gameObject.SetActive(editStatus == FilterEidtStatus.SortCondition);
    }
}

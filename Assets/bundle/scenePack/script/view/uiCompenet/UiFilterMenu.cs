using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiFilterMenu : BaseUIComponent
{
    [SerializeField]
    protected GameObject groupBig;

    [SerializeField]
    protected GameObject groupSmall;

    [SerializeField]
    protected List listFilterBig;

    [SerializeField]
    protected List listFilterSmall;

    [SerializeField]
    protected Button btnMask;

    [SerializeField]
    protected Button btnOpen;

    [SerializeField]
    protected Button btnClose;

    protected void Awake()
    {
        this.btnOpen.onClick.AddListener(this.doOpen);
        this.btnClose.onClick.AddListener(this.doClose);
        this.btnMask.onClick.AddListener(this.doMask);

        this.on<FilterModel>("filterList", this.refreshFilterList);
        this.on<FilterModel>("isOpenFilter", this.refreshIsOpenFilter);
        this.on<FilterModel>("isEdit", this.refreshIsEdit);
        this.on<PageModel>("pageIndex", this.refreshPageIndex);
    }

    protected void Start()
    {
        this.refreshFilterList(this.get<FilterModel>().filterList);
        this.refreshIsOpenFilter(this.get<FilterModel>().isOpenFilter);
        this.refreshIsEdit(this.get<FilterModel>().isEdit);
        this.refreshPageIndex(this.get<PageModel>().pageIndex);
    }

    protected void refreshPageIndex(object obj)
    {
        var pageIndex = (PageIndex)obj;
        this.gameObject.SetActive(pageIndex == PageIndex.Filter);
    }

    protected void refreshIsEdit(object obj)
    {
        var isEdit = (bool)obj;
        this.refreshListFilterBig(this.get<FilterModel>().filterList, isEdit);
        this.btnMask.gameObject.SetActive(isEdit);
    }

    protected void refreshIsOpenFilter(object obj)
    {
        var isOpenFilter = (bool)obj;
        this.groupBig.SetActive(isOpenFilter);
        this.groupSmall.SetActive(!isOpenFilter);
    }

    protected void refreshListFilterBig(List<Filter> filterList, bool isEdit)
    {
        var _filterList = filterList.Select(x => x.copy()).ToList();
        _filterList.ForEach(x => x.isEdit = isEdit);
        if (isEdit)
        {
            _filterList.Add(null);
        }
        this.listFilterBig.replace(_filterList.ToList<object>());
    }

    protected void refreshFilterList(object obj)
    {
        var filterList = obj as List<Filter>;
        this.listFilterSmall.replace(filterList.ToList<object>());
        this.refreshListFilterBig(filterList, this.get<FilterModel>().isEdit);
    }

    protected void doOpen()
    {
        this.get<FilterModel>().switchFilter(true);
        this.get<FilterModel>().switchEdit(true);
    }

    protected void doClose()
    {
        this.get<FilterModel>().switchFilter(false);
        this.get<FilterModel>().switchEdit(false);
    }

    protected void doMask()
    {
        this.get<FilterModel>().switchFilter(false);
        this.get<FilterModel>().switchEdit(false);
    }
}

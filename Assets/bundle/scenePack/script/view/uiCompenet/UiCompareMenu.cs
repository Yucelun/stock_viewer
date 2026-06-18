using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiCompareMenu : BaseUIComponent
{
    [SerializeField]
    protected GameObject groupBig;

    [SerializeField]
    protected GameObject groupSmall;

    [SerializeField]
    protected List listCompareBig;

    [SerializeField]
    protected List listCompareSmall;

    [SerializeField]
    protected Button btnMask;

    [SerializeField]
    protected Button btnOpen;

    [SerializeField]
    protected Button btnClose;


    [SerializeField]
    protected Toggle toggleEdit;

    protected void Awake()
    {
        this.btnOpen.onClick.AddListener(this.doOpen);
        this.btnClose.onClick.AddListener(this.doClose);
        this.btnMask.onClick.AddListener(this.doMask);

        this.toggleEdit.onValueChanged.AddListener(this.doEdit);

        this.on<CompareModel>("compareList", this.refreshCompareList);
        this.on<CompareModel>("isOpenCompare", this.refreshIsOpenCompare);
        this.on<CompareModel>("isEdit", this.refreshIsEdit);
        this.on<PageModel>("pageIndex", this.refreshPageIndex);
    }

    protected void Start()
    {
        this.refreshCompareList(this.get<CompareModel>().compareList);
        this.refreshIsOpenCompare(this.get<CompareModel>().isOpenCompare);
        this.refreshIsEdit(this.get<CompareModel>().isEdit);
        this.refreshPageIndex(this.get<PageModel>().pageIndex);
    }

    protected void refreshPageIndex(object obj)
    {
        var pageIndex = (PageIndex)obj;
        this.gameObject.SetActive(pageIndex == PageIndex.Compare);
    }

    protected void refreshIsEdit(object obj)
    {
        var isEdit = (bool)obj;
        this.toggleEdit.SetIsOnWithoutNotify(isEdit);
        this.refreshListCompareBig(this.get<CompareModel>().compareList, isEdit);
        this.btnMask.gameObject.SetActive(isEdit);
    }

    protected void refreshIsOpenCompare(object obj)
    {
        var isOpenCompare = (bool)obj;
        this.groupBig.SetActive(isOpenCompare);
        this.groupSmall.SetActive(!isOpenCompare);
    }

    protected void refreshListCompareBig(List<Compare> compareList, bool isEdit)
    {
        var _compareList = compareList.Select(x => x.copy()).ToList();
        _compareList.ForEach(x => x.isEdit = isEdit);
        if (isEdit) {
            _compareList.Add(null);
        }
        this.listCompareBig.replace(_compareList.ToList<object>());
    }

    protected void refreshCompareList(object obj)
    {
        var compareList = obj as List<Compare>;
        this.listCompareSmall.replace(compareList.ToList<object>());
        this.refreshListCompareBig(compareList, this.get<CompareModel>().isEdit);
    }

    protected void doOpen()
    {
        this.get<CompareModel>().switchCompare(true);
    }

    protected void doClose()
    {
        this.get<CompareModel>().switchCompare(false);
        this.get<CompareModel>().switchEdit(false);
    }

    protected void doMask()
    {
        this.get<CompareModel>().switchCompare(false);
        this.get<CompareModel>().switchEdit(false);
    }

    protected void doEdit(bool isEdit)
    {
        this.get<CompareModel>().switchEdit(isEdit);
    }
}

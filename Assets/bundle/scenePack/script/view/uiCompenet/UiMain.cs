using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class UiMain : BaseUIComponent
{
    [SerializeField]
    protected List listPageTab;

    protected void Awake()
    {
        this.on<PageModel>("pageList", this.refreshPageList);
        this.on<PageModel>("pageIndex", this.refreshPageIndex);
        this.on<PageModel>("prePageIndex", this.refreshPrePageIndex);
    }

    private void Start()
    {
        this.refreshPageIndex(this.get<PageModel>().pageIndex);
    }

    protected void refreshPageList(object obj)
    {
        this.refreshListPageTab(obj as List<PageInfo>, this.get<PageModel>().pageIndex, this.get<PageModel>().prePageIndex);
    }

    protected void refreshPageIndex(object obj)
    {
        this.refreshListPageTab(this.get<PageModel>().pageList, (PageIndex)obj, this.get<PageModel>().prePageIndex);
    }

    protected void refreshPrePageIndex(object obj)
    {
        this.refreshListPageTab(this.get<PageModel>().pageList, this.get<PageModel>().pageIndex, (PageIndex)obj);
    }

    protected void refreshListPageTab(List<PageInfo> pageList, PageIndex pageIndex, PageIndex prePageIndex)
    {
        var _pageList = pageList.Select(x => new PageInfo() { name = x.name, pageIndex = x.pageIndex, selected = prePageIndex == PageIndex.None ? x.pageIndex == pageIndex : true }).ToList<object>();
        this.listPageTab.replace(_pageList);
    }
}

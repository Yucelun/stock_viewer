using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PageIndex { 
    None = -1,
    OptionalStocks = 0,
    Filter = 1,
    Study = 2,
    Compare = 3,
    System = 4,
}

public class PageInfo
{
    public bool selected = false;
    public string name;
    public PageIndex pageIndex;
}

public class PageModel : BaseModel
{
    private List<PageInfo> _pageList = new List<PageInfo>();
    public List<PageInfo> pageList
    {
        get { return this._pageList; }
    }

    protected List<PageInfo> getPageList()
    {
        return new List<PageInfo>() {
        new PageInfo() {
            name = "自選股",
            pageIndex = PageIndex.OptionalStocks,
        },
        new PageInfo() {
            name = "選股",
            pageIndex = PageIndex.Filter,
        },
        new PageInfo() {
            name = "個股研究",
            pageIndex = PageIndex.Study,
        },
        new PageInfo() {
            name = "比較個股",
            pageIndex = PageIndex.Compare,
        },
        new PageInfo() {
            name = "系統",
            pageIndex = PageIndex.System,
        },
    };
    }

    public override void Awake()
    {
        base.Awake();

        this._pageList = this.getPageList();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private PageIndex _pageIndex = PageIndex.Study;
    public PageIndex pageIndex
    {
        get
        {
            return this._pageIndex;
        }
    }
    public void setPageIndex(PageIndex index)
    {
        this._pageIndex = index;
        this.emit("pageIndex", this._pageIndex);
    }

    private PageIndex _prePageIndex = PageIndex.None;
    public PageIndex prePageIndex
    {
        get
        {
            return this._prePageIndex;
        }
    }
    public void setPrePageIndex(PageIndex index)
    {
        this._prePageIndex = index;
        this.emit("prePageIndex", this._prePageIndex);
    }
}

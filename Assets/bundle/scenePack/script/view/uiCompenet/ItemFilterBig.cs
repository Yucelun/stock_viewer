using UnityEngine;
using UnityEngine.UI;

public class ItemFilterBig : ItemRenderer
{
    [SerializeField]
    protected Text textFilterCaption;

    [SerializeField]
    protected Button btnFilter;

    [SerializeField]
    protected Button btnNew;

    [SerializeField]
    protected GameObject groupFilter;

    [SerializeField]
    protected Button btnDelete;

    [SerializeField]
    protected Button btnEdit;

    protected void Awake()
    {
        this.btnFilter.onClick.AddListener(this.doFilter);
        this.btnNew.onClick.AddListener(this.doNew);
        this.btnDelete.onClick.AddListener(this.doDelete);
        this.btnEdit.onClick.AddListener(this.doEdit);
    }

    protected override void doDataChanged()
    {
        if (this.data == null)
        {
            this.btnNew.gameObject.SetActive(true);
            this.groupFilter.SetActive(false);
        }
        else
        {
            var filter = this.data as Filter;
            var name = filter.name;
            var isEdit = filter.isDefault ? false : filter.isEdit;
            this.btnNew.gameObject.SetActive(false);
            this.groupFilter.SetActive(true);
            this.textFilterCaption.text = name;
            this.btnDelete.gameObject.SetActive(isEdit);
            this.btnEdit.gameObject.SetActive(isEdit);
        }
    }

    protected void doFilter()
    {
        this.get<FilterModel>().useFilter(this.data as Filter);
    }

    protected void doNew()
    {
        this.get<FilterModel>().setEditFilter(new Filter()
        {
            serialNo = Filter.serialCounter.generateSerialNo(),
        });
        this.get<UiManager>().openWindow<UiFilterEdit>("uiPack", "uiFilterEdit", NodeParent.UI);
    }

    protected void doDelete()
    {
        UiHint uiHint = this.get<UiManager>().openWindow<UiHint>("uiPack", "uiHint", NodeParent.UI);
        uiHint.showText("確定要刪除?");
        uiHint.on("onResult", this.doCheckDeleteResult);
    }

    protected void doCheckDeleteResult(object obj)
    {
        var result = (bool)obj;
        if (result)
        {
            this.get<FilterModel>().removeFilter(this.data as Filter);
        }
    }

    protected void doEdit()
    {
        var filter = this.data as Filter;
        this.get<FilterModel>().setEditFilter(filter);
        this.get<UiManager>().openWindow<UiFilterEdit>("uiPack", "uiFilterEdit", NodeParent.UI);
    }
}

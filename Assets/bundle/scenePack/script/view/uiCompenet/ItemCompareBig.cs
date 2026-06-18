using UnityEngine;
using UnityEngine.UI;

public class ItemCompareBig : ItemRenderer
{
    [SerializeField]
    protected Text textCompareCaption;

    [SerializeField]
    protected Button btnCompare;

    [SerializeField]
    protected Button btnNew;

    [SerializeField]
    protected GameObject groupCompare;

    [SerializeField]
    protected Button btnDelete;

    [SerializeField]
    protected Button btnEdit;

    protected void Awake()
    {
        this.btnCompare.onClick.AddListener(this.doCompare);
        this.btnNew.onClick.AddListener(this.doNew);
        this.btnDelete.onClick.AddListener(this.doDelete);
        this.btnEdit.onClick.AddListener(this.doEdit);
    }

    protected override void doDataChanged()
    {
        if (this.data == null)
        {
            this.btnNew.gameObject.SetActive(true);
            this.groupCompare.SetActive(false);
        }
        else
        {
            var Compare = this.data as Compare;
            var name = Compare.name;
            var isEdit = Compare.isEdit;
            this.btnNew.gameObject.SetActive(false);
            this.groupCompare.SetActive(true);
            this.textCompareCaption.text = name;
            this.btnDelete.gameObject.SetActive(isEdit);
            this.btnEdit.gameObject.SetActive(isEdit);
        }
    }

    protected void doCompare()
    {
        this.get<CompareModel>().useCompare(this.data as Compare);
    }

    protected void doNew()
    {
        this.get<CompareModel>().setEditCompare(new Compare() {
            serialNo = Compare.serialCounter.generateSerialNo(),
        });
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
            this.get<CompareModel>().removeCompare(this.data as Compare);
        }
    }

    protected void doEdit()
    {
        var Compare = this.data as Compare;
        this.get<CompareModel>().setEditCompare(Compare);

        //this.get<UiManager>().openWindow<UiCompareEdit>("uiPack", "uiCompareEdit", NodeParent.UI);
    }
}

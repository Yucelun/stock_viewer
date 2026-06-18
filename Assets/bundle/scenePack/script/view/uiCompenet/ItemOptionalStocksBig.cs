using UnityEngine;
using UnityEngine.UI;

public class ItemOptionalStocksBig : ItemRenderer
{
    [SerializeField]
    protected Text textOptionalStocksCaption;

    [SerializeField]
    protected Button btnOptionalStocks;

    [SerializeField]
    protected Button btnNew;

    [SerializeField]
    protected GameObject groupOptionalStocks;

    [SerializeField]
    protected Button btnDelete;

    [SerializeField]
    protected Button btnEdit;

    protected void Awake()
    {
        this.btnOptionalStocks.onClick.AddListener(this.doOptionalStocks);
        this.btnNew.onClick.AddListener(this.doNew);
        this.btnDelete.onClick.AddListener(this.doDelete);
        this.btnEdit.onClick.AddListener(this.doEdit);
    }

    protected override void doDataChanged()
    {
        if (this.data == null)
        {
            this.btnNew.gameObject.SetActive(true);
            this.groupOptionalStocks.SetActive(false);
        }
        else
        {
            var optionalStocks = this.data as OptionalStocks;
            var name = optionalStocks.name;
            var isEdit = optionalStocks.isDefault ? false : optionalStocks.isEdit;
            this.btnNew.gameObject.SetActive(false);
            this.groupOptionalStocks.SetActive(true);
            this.textOptionalStocksCaption.text = name;
            this.btnDelete.gameObject.SetActive(isEdit);
            this.btnEdit.gameObject.SetActive(isEdit);
        }
    }

    protected void doOptionalStocks()
    {
        this.get<OptionalStocksModel>().useOptionalStocks(this.data as OptionalStocks);
    }

    protected void doNew()
    {
        this.get<OptionalStocksModel>().setEditOptionalStocks(new OptionalStocks()
        {
            serialNo = OptionalStocks.serialCounter.generateSerialNo(),
        });
        this.get<UiManager>().openWindow<UiOptionalStocksEdit>("uiPack", "uiOptionalStocksEdit", NodeParent.UI);
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
            this.get<OptionalStocksModel>().removeOptionalStocks(this.data as OptionalStocks);
        }
    }

    protected void doEdit()
    {
        var optionalStocks = this.data as OptionalStocks;
        this.get<OptionalStocksModel>().setEditOptionalStocks(optionalStocks);

        this.get<UiManager>().openWindow<UiOptionalStocksEdit>("uiPack", "uiOptionalStocksEdit", NodeParent.UI);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ItemOptionalStocksSmall : ItemRenderer
{
    [SerializeField]
    protected Text textOptionalStocksCaption;

    [SerializeField]
    protected Button btnOptionalStocks;

    protected void Awake()
    {
        this.btnOptionalStocks.onClick.AddListener(this.doOptionalStocks);
    }

    protected override void doDataChanged()
    {
        if (this.data != null)
        {
            var name = (this.data as OptionalStocks).name;
            this.textOptionalStocksCaption.text = name.Length > 2 ? name.Substring(0, 2) : name;
        }
    }

    protected void doOptionalStocks()
    {
        this.get<OptionalStocksModel>().useOptionalStocks(this.data as OptionalStocks);
    }
}

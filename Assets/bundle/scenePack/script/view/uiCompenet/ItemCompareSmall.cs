using UnityEngine;
using UnityEngine.UI;

public class ItemCompareSmall : ItemRenderer
{
    [SerializeField]
    protected Text textCompareCaption;

    [SerializeField]
    protected Button btnCompare;

    protected void Awake()
    {
        this.btnCompare.onClick.AddListener(this.doCompare);
    }

    protected override void doDataChanged()
    {
        if (this.data != null)
        {
            var name = (this.data as Compare).name;
            this.textCompareCaption.text = name.Length > 2 ? name.Substring(0, 2) : name;
        }
    }

    protected void doCompare()
    {
        this.get<CompareModel>().useCompare(this.data as Compare);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ItemFilterSmall : ItemRenderer
{
    [SerializeField]
    protected Text textFilterCaption;

    [SerializeField]
    protected Button btnFilter;

    protected void Awake()
    {
        this.btnFilter.onClick.AddListener(this.doFilter);
    }

    protected override void doDataChanged()
    {
        if (this.data != null)
        {
            var name = (this.data as Filter).name;
            this.textFilterCaption.text = name.Length > 2 ? name.Substring(0, 2) : name;
        }
    }

    protected void doFilter()
    {
        this.get<FilterModel>().useFilter(this.data as Filter);
    }
}

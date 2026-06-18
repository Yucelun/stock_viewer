using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComboBox : BaseUIComponent
{
    [SerializeField]
    protected Button btnSelect;

    [SerializeField]
    protected Text textCaption;

    [SerializeField]
    protected List listSelection;

    private void Awake()
    {
        this.btnSelect.onClick.AddListener(this.doSelect);
        this.listSelection.on("onItemClick", this.doItemClick);
        this.listSelection.gameObject.SetActive(false);
    }
    protected void doSelect()
    {
        this.listSelection.gameObject.SetActive(!this.listSelection.gameObject.activeSelf);
    }

    protected void doItemClick(object obj)
    {
        var item = obj as ItemRenderer;
        this.listSelection.gameObject.SetActive(false);
        this.selectedIndex = item.index;
        this.emit("onItemClick", this);
    }

    protected string getCaption()
    {
        if (-1 < this.selectedIndex && this.selectedIndex < this.selections.Count)
        {
            return this.selections[this.selectedIndex];
        }
        else
        {
            return "";
        }
    }
    protected void refreshCaption()
    {
        this.textCaption.text = this.getCaption();
    }

    protected int _selectedIndex = 0;
    public int selectedIndex {
        get { 
            return this._selectedIndex;
        }

        set { 
            this._selectedIndex = value;
            this.refreshCaption();
        }
    }

    protected List<string> _selections = new List<string>();
    public List<string> selections {
        get { 
            return this._selections;
        }

        set
        {
            this._selections = value;
            this.listSelection.replace(this._selections.ToList<object>());

            this.refreshCaption();
        }
    }
}

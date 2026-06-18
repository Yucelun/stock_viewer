using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ICell { 
    public string value;
}

public class IGrid
{
    public List<List<ICell>> propertyList;
}

public class Grid : BaseUIComponent
{
    [SerializeField]
    protected List listPropertyCaption;

    [SerializeField]
    protected List listStock;

    private void Awake()
    {
        this.listStock.on("onItemClick", this.doItemClick);
    }

    public void replace(IGrid data)
    {
        var propertyList = data.propertyList;
        this.listStock.replace(propertyList.ToList<object>());
    }

    protected void doItemClick(object obj) {
        this.emit("onRow", obj as List<ICell>);
    }

    protected List<string> _columns  = new List<string>();
    public List<string> columns
    {
        set {

            this._columns = value;
            this.listPropertyCaption.replace(this._columns.ToList<object>());
        }
    }
}

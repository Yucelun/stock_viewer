using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public class CompareScoreType
{
    public int serialNo = 0;
    public ScoreType scoreType = ScoreType.NONE;
    public string name = "";
    public List<FinancialType> financialList = new List<FinancialType>();
    public string color = "FFFFFF";
    public string strokeColor = "000000";

    public CompareScoreType copy()
    {
        return new CompareScoreType()
        {
            serialNo = this.serialNo,
            scoreType = this.scoreType, 
            name = this.name,
            financialList = this.financialList.Select(x => x).ToList(),
            color = this.color,
            strokeColor = this.strokeColor,
        };
    }

    public void update(CompareScoreType data)
    {
        this.scoreType = data.scoreType;
        this.name = data.name;
        this.financialList = data.financialList.Select(x => x).ToList();
        this.color = data.color;
        this.strokeColor = data.strokeColor;
    }

    public static SerialCounter serialCounter = new SerialCounter();
}

public class Compare
{
    public int serialNo = 0;
    public string name = "";
    public bool isEdit = false;
    public List<CompareScoreType> scoreTypeList = new List<CompareScoreType>();

    public Compare copy()
    {
        return new Compare()
        {
            serialNo = this.serialNo,
            name = this.name,
            isEdit = this.isEdit,
            scoreTypeList = this.scoreTypeList.Select(x => x.copy()).ToList(),
        };
    }

    public void update(Compare data)
    {
        this.name = data.name;
        this.isEdit = data.isEdit;
        this.scoreTypeList = data.scoreTypeList.Select(x => x.copy()).ToList();
    }

    public CompareScoreType getCompareStock(int serialNo)
    {
        return this.scoreTypeList.Find(x => x.serialNo == serialNo);
    }

    public void setCompareStock(CompareScoreType data)
    {
        var index = this.scoreTypeList.FindIndex(x => x.serialNo == data.serialNo);
        if (index > -1)
        {
            this.scoreTypeList[index].update(data);
        }
        else
        {
            this.scoreTypeList.Add(data);
        }
    }

    public void removeCompareStock(CompareScoreType data)
    {
        var index = this.scoreTypeList.FindIndex(x => x.serialNo == data.serialNo);
        if (index > -1)
        {
            this.scoreTypeList.RemoveAt(index);
        }
    }

    public static SerialCounter serialCounter = new SerialCounter();
}

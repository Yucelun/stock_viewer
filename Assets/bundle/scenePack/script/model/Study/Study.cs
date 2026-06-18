using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public enum ScoreType
{
    NONE,
    PRODUCT,
    MANAGER,
    ASSET,
    FINANCE,
    INVESTMENT,
    SPECULATION,
    TOTAL
}

public class StudyScoreType
{
    public int serialNo = 0;
    public ScoreType scoreType = ScoreType.NONE;
    public string name = "";
    public List<FinancialType> financialList = new List<FinancialType>();
    public string color = "FFFFFF";
    public string strokeColor = "000000";

    public StudyScoreType copy()
    {
        return new StudyScoreType()
        {
            serialNo = this.serialNo,
            scoreType = this.scoreType, 
            name = this.name,
            financialList = this.financialList.Select(x => x).ToList(),
            color = this.color,
            strokeColor = this.strokeColor,
        };
    }

    public void update(StudyScoreType data)
    {
        this.scoreType = data.scoreType;
        this.name = data.name;
        this.financialList = data.financialList.Select(x => x).ToList();
        this.color = data.color;
        this.strokeColor = data.strokeColor;
    }

    public static SerialCounter serialCounter = new SerialCounter();
}

public class Study
{
    public int serialNo = 0;
    public string name = "";
    public bool isEdit = false;
    public List<StudyScoreType> scoreTypeList = new List<StudyScoreType>();

    public Study copy()
    {
        return new Study()
        {
            serialNo = this.serialNo,
            name = this.name,
            isEdit = this.isEdit,
            scoreTypeList = this.scoreTypeList.Select(x => x.copy()).ToList(),
        };
    }

    public void update(Study data)
    {
        this.name = data.name;
        this.isEdit = data.isEdit;
        this.scoreTypeList = data.scoreTypeList.Select(x => x.copy()).ToList();
    }

    public StudyScoreType getStudyStock(int serialNo)
    {
        return this.scoreTypeList.Find(x => x.serialNo == serialNo);
    }

    public void setStudyStock(StudyScoreType data)
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

    public void removeStudyStock(StudyScoreType data)
    {
        var index = this.scoreTypeList.FindIndex(x => x.serialNo == data.serialNo);
        if (index > -1)
        {
            this.scoreTypeList.RemoveAt(index);
        }
    }

    public static SerialCounter serialCounter = new SerialCounter();
}



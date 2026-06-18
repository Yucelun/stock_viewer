using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStudyScoreTab : ItemRenderer
{
    [SerializeField]
    protected Button btnTab;

    [SerializeField]
    protected Text textCaption;

    [SerializeField]
    protected RawImage rawImageBg;

    [SerializeField]
    protected Outline outlineBg;

    protected void Awake()
    {
        this.btnTab.onClick.AddListener(this.doTab);
    }

    protected override void doDataChanged()
    {
        var data = this.data as ItemStudyScoreTabData;
        var scoreType = data.scoreType;
        var selected = data.selected;
        var studyScoreType = this.get<StudyModel>().study.scoreTypeList.Find(x => x.scoreType == scoreType);
        var name = studyScoreType.name;
        var color = studyScoreType.color;
        var strokeColor = studyScoreType.strokeColor;
        this.textCaption.text = name.Length > 2 ? name.Substring(0, 2) : name;
        this.textCaption.color = Colors.FromHex(selected ? color : strokeColor);
        this.outlineBg.effectColor = Colors.FromHex(selected ? color : strokeColor);
        this.rawImageBg.color = Colors.FromHex(selected ? strokeColor : color); 
    }

    protected void doTab()
    {
        var data = this.data as ItemStudyScoreTabData;
        var scoreType = data.scoreType;
        this.get<StudyModel>().setSelectedScoreType(scoreType);
    }
}

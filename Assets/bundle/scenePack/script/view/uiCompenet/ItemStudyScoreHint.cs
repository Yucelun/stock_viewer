using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStudyScoreHint : ItemRenderer
{
    [SerializeField]
    protected Text textCaption;

    [SerializeField]
    protected Text textScore;

    [SerializeField]
    protected RawImage rawImageBg;

    [SerializeField]
    protected Outline outlineBg;
    
    protected override void doDataChanged()
    {
        var data = this.data as ItemStudyScoreHintData;
        var scoreType = data.scoreType;
        var score = data.score;
        var selected = data.selected;
        var studyScoreType = this.get<StudyModel>().study.scoreTypeList.Find(x => x.scoreType == scoreType);
        var name = studyScoreType.name;
        var color = studyScoreType.color;
        var strokeColor = studyScoreType.strokeColor;
        this.textCaption.text = name.Length > 2 ? name.Substring(0, 2) : name;
        this.textScore.text = Common.numberWithCommas(score, 1);
        this.textCaption.color = Colors.FromHex(selected ? color : strokeColor);
        this.textScore.color = Colors.FromHex(selected ? color : strokeColor);
        this.outlineBg.effectColor = Colors.FromHex(selected ? color : strokeColor);
        this.rawImageBg.color = Colors.FromHex(selected ? strokeColor : color);

        this.setWidget();
    }
    protected void setWidget()
    {
        var rectTransform = this.GetComponent<RectTransform>();
        switch (this.index)
        {
            case 0: // 上
                rectTransform.localPosition = new Vector3(0, 125, 0);
                break;
            case 1: // 左上
                rectTransform.localPosition = new Vector3(-120, 63, 0);
                break;
            case 2: // 左下
                rectTransform.localPosition = new Vector3(-120, -63, 0);
                break;
            case 3: // 下
                rectTransform.localPosition = new Vector3(0, -125, 0);
                break;
            case 4: // 右下
                rectTransform.localPosition = new Vector3(120, -63, 0);
                break;
            case 5: // 右上
                rectTransform.localPosition = new Vector3(120, 63, 0);
                break;
        }
    }
}

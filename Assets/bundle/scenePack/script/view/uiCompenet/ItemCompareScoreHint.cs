using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCompareScoreHint : ItemRenderer
{
    [SerializeField]
    protected Text textCaption;

    [SerializeField]
    protected RawImage rawImageBg;

    [SerializeField]
    protected Outline outlineBg;

    [SerializeField]
    protected Text textScoreLeft;

    [SerializeField]
    protected RawImage rawImageLeft;

    [SerializeField]
    protected Text textScoreRight;

    [SerializeField]
    protected RawImage rawImageRight;

    protected override void doDataChanged()
    {
        var data = this.data as ItemCompareScoreHintData;
        var scoreType = data.scoreType;
        var selected = data.selected;
        var compareScoreType = this.get<CompareModel>().compare.scoreTypeList.Find(x => x.scoreType == scoreType);
        var name = compareScoreType.name;
        var color = compareScoreType.color;
        var strokeColor = compareScoreType.strokeColor;

        this.textCaption.color = Colors.FromHex(selected ? color : strokeColor);
        this.textCaption.text = name.Length > 2 ? name.Substring(0, 2) : name;

        this.rawImageLeft.color = this.get<CompareModel>().leftStrokeColor;
        this.textScoreLeft.color = this.get<CompareModel>().leftColor;
        this.textScoreLeft.text = Common.numberWithCommas(data.left, 1);
        this.rawImageRight.color = this.get<CompareModel>().rightStrokeColor;
        this.textScoreRight.color = this.get<CompareModel>().rightColor;
        this.textScoreRight.text = Common.numberWithCommas(data.right, 1);
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

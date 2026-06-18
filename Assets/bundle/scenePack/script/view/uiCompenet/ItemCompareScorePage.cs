using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemCompareScorePage : ItemRenderer
{
    [SerializeField]
    protected RawImage rawImageCaption;

    [SerializeField]
    protected Text textCaption;

    [SerializeField]
    protected Text textLeftScore;

    [SerializeField]
    protected RawImage rawImageIconWinLeft;

    [SerializeField]
    protected RawImage rawImageLeft;

    [SerializeField]
    protected Outline outlineLeft;

    [SerializeField]
    protected Text textRightScore;

    [SerializeField]
    protected RawImage rawImageIconWinRight;

    [SerializeField]
    protected RawImage rawImageRight;

    [SerializeField]
    protected Outline outlineRight;

    [SerializeField]
    protected List listCompareScorePageFinancialRatio;

    protected override void doDataChanged()
    {
        var data = this.data as ItemCompareScorePageData;
        var scoreType = data.scoreType;
        var compareScoreType = this.get<CompareModel>().compare.scoreTypeList.Find(x => x.scoreType == scoreType);
        var financialRatioList = data.financialRatioList;

        this.rawImageCaption.color = Colors.FromHex(compareScoreType.strokeColor);
        this.textCaption.color = Colors.FromHex(compareScoreType.color);
        var caption = FilterConst.getScoreTypeCaption(scoreType);
        this.textCaption.text = caption.Length > 2 ? caption.Substring(0, 2) : caption; 

        this.outlineLeft.effectColor = Colors.FromHex(compareScoreType.strokeColor);
        this.rawImageLeft.color = this.get<CompareModel>().leftStrokeColor;
        this.textLeftScore.color = this.get<CompareModel>().leftColor;
        this.textLeftScore.text = Common.numberWithCommas(data.left, 1);

        this.outlineRight.effectColor = Colors.FromHex(compareScoreType.strokeColor);
        this.rawImageRight.color = this.get<CompareModel>().rightStrokeColor;
        this.textRightScore.color = this.get<CompareModel>().rightColor;
        this.textRightScore.text = Common.numberWithCommas(data.right, 1);

        this.listCompareScorePageFinancialRatio.replace(financialRatioList.ToList<object>());

        this.rawImageIconWinLeft.gameObject.SetActive(data.left > data.right);
        this.rawImageIconWinRight.gameObject.SetActive(data.left < data.right);
    }
}
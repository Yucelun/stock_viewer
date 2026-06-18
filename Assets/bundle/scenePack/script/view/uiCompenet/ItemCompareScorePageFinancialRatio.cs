using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCompareScorePageFinancialRatio : ItemRenderer
{
    [SerializeField]
    protected Text textCaption;

    [SerializeField]
    protected RawImage rawImageCaption;

    [SerializeField]
    protected Text textLeftValue;

    [SerializeField]
    protected RawImage rawImageLeft;

    [SerializeField]
    protected RawImage rawImageIconWinLeft;

    [SerializeField]
    protected Text textRightValue;

    [SerializeField]
    protected RawImage rawImageIconWinRight;

    [SerializeField]
    protected RawImage rawImageRight;

    protected override void doDataChanged()
    {
        var financialRatioData = this.data as ItemCompareScorePageFinancialRatioData;
        var scoreType = financialRatioData.scoreType;
        var financialType = financialRatioData.financialType;
        var caption = FilterConst.getFinancialTypeCaption(financialType);
        var compareScoreType = this.get<CompareModel>().compare.scoreTypeList.Find(x => x.scoreType == scoreType);

        this.rawImageCaption.color = Colors.FromHex(compareScoreType.color);
        this.textCaption.color = Colors.FromHex(compareScoreType.strokeColor);
        this.textCaption.text = caption;
        this.rawImageLeft.color = this.get<CompareModel>().leftStrokeColor;
        this.textLeftValue.color = this.get<CompareModel>().leftColor;
        this.textLeftValue.text = Common.numberWithCommas(financialRatioData.left * 100, 2) + "%";
        this.rawImageRight.color = this.get<CompareModel>().rightStrokeColor;
        this.textRightValue.color = this.get<CompareModel>().rightColor;
        this.textRightValue.text = Common.numberWithCommas(financialRatioData.right * 100, 2) + "%";

        this.rawImageIconWinLeft.gameObject.SetActive(financialRatioData.left > financialRatioData.right);
        this.rawImageIconWinRight.gameObject.SetActive(financialRatioData.left < financialRatioData.right);
    }
}

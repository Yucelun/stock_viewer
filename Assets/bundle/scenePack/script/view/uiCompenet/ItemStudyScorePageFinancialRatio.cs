using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ItemStudyScorePageFinancialRatio : ItemRenderer
{
    [SerializeField]
    protected Text textFinancialRatioCaption;

    [SerializeField]
    protected Text textFinancialRatio;

    [SerializeField]
    protected Text textPropertyGroup;

    protected void Awake() {
    }

    protected override void doDataChanged()
    {
        var financialRatioData = this.data as ItemStudyScorePageFinancialRatioData;
        var financialType = financialRatioData.financialType;
        var value = financialRatioData.value;
        var score = financialRatioData.score;
        var caption = FilterConst.getFinancialTypeCaption(financialType);

        this.textFinancialRatioCaption.text = caption;
        this.textFinancialRatio.text = Common.numberWithCommas(value * 100, 2)+"%";
        this.textPropertyGroup.text = score.ToString();
    }
}

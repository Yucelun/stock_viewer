using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemStudyScorePage : ItemRenderer
{    
    [SerializeField]
    protected GameObject groupFinancialRatio;

    [SerializeField]
    protected RawImage rawImageStroke;

    [SerializeField]
    protected RawImage rawImagePanel;

    [SerializeField]
    protected Text textScore;

    [SerializeField]
    protected List listFinancialRatio;

    private void Awake()
    {
    }

    protected override void doDataChanged()
    {
        var data = this.data as ItemStudyScorePageData;
        var scoreType = data.scoreType;
        var score = data.score;
        var financialRatioList = data.financialRatioList;
        this.listFinancialRatio.replace(financialRatioList.ToList<object>());

        var studyScore = this.get<StudyModel>().study.scoreTypeList.Find(x => x.scoreType == scoreType);
        if (studyScore != null)
        {
            this.rawImageStroke.color = Colors.FromHex(studyScore.strokeColor);
            this.rawImagePanel.color = Colors.FromHex(studyScore.color);
            this.textScore.color = Colors.FromHex(studyScore.strokeColor);
        }

        this.textScore.text = Common.numberWithCommas(score, 1);
    }
}

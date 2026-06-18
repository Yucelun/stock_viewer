using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public class ItemStudyScorePageData
{
    public ScoreType scoreType;
    public float score;
    public List<ItemStudyScorePageFinancialRatioData> financialRatioList;
}

public class ItemStudyScorePageFinancialRatioData
{
    public FinancialType financialType;
    public float value;
    public float score;
}

public class ItemStudyScoreTabData
{
    public ScoreType scoreType;
    public bool selected = false;
}
public class ItemStudyScoreHintData
{
    public ScoreType scoreType;
    public float score;
    public bool selected = false;
}


public class StudyScore
{
    public string stockId;
    public List<StudySubScore> studySubScoreList;
}

public class StudyFinancialScore
{
    public FinancialType financialType;
    public float score;
}

public class StudySubScore
{
    public ScoreType scoreType;
    public float score;
    public List<StudyFinancialScore> studyFinancialScoreList;
}


public class IStorageStudyList
{
    public List<Study> list;
}

public class StudyModel : BaseModel
{
    public string fileName = "study";
    public override void Awake()
    {
        base.Awake();

        this.on<GlobalData>("stockScoreList", this.refreshStockScoreList);

        this._studyList = this.getStudyList();
        int maxSerialCounter_Study = this._studyList.Select(x => x.serialNo).Max();
        Study.serialCounter.replace(maxSerialCounter_Study);
        int maxSerialCounter_StudyScoreType = this._studyList.Select(x => x.scoreTypeList.Select(y => y.serialNo).Max()).Max();
        StudyScoreType.serialCounter.replace(maxSerialCounter_StudyScoreType);

        this.useStudy(this._studyList[0]);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private List<StudyScore> _studyScoreList = new List<StudyScore>();
    public List<StudyScore> studyScoreList {
        get {
            return this._studyScoreList;
        }
    }

    protected void refreshStockScoreList(object obj)
    {
        var stockScoreList = obj as List<IStockScoreData>;
        this._studyScoreList = stockScoreList.Select((x) => {
            return new StudyScore()
            {
                stockId = x.stockId,
                studySubScoreList = this.study.scoreTypeList.Select((y) => {
                    var studyFinancialScoreList = y.financialList.Select((financialType) => {
                        return new StudyFinancialScore()
                        {
                            financialType = financialType,
                            score = x.GetValue<float>(financialType.ToString())
                        };
                    }).ToList();

                    return new StudySubScore()
                    {
                        scoreType = y.scoreType,
                        score = this.getScore(studyFinancialScoreList),
                        studyFinancialScoreList = studyFinancialScoreList
                    };
                }).ToList()
            };
        }).ToList();

        this.emit("studyScoreList", this.studyScoreList);
    }

    protected float getScore(List<StudyFinancialScore> studyFinancialScoreList)
    {
        if (studyFinancialScoreList.Count > 0)
        {
            return studyFinancialScoreList.Select((x) => x.score).Sum() / studyFinancialScoreList.Count;
        }

        return 0;
    }

    protected List<Study> getStudyList()
    {
        IStorageStudyList jsonObject;
        if (this.get<SaveFile>().exists(this.fileName))
        {
            var loadObject = this.get<SaveFile>().load<IStorageStudyList>(this.fileName);
            if (loadObject != null)
            {
                jsonObject = loadObject;
            }
            else
            {
                jsonObject = this.getDefaultStudyList();
            }
        }
        else
        {
            jsonObject = this.getDefaultStudyList();
        }

        return jsonObject.list;
    }

    protected IStorageStudyList getDefaultStudyList()
    {
        return new IStorageStudyList()
        {
            list = new List<Study>() {
                new Study()
                {
                    name = "研究",
                    serialNo = Study.serialCounter.generateSerialNo(),
                    scoreTypeList = new List<StudyScoreType>
                    {
                        new StudyScoreType() {
                            name = "投資的價值",
                            serialNo =  StudyScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.INVESTMENT,
                            financialList = new List<FinancialType>() {
                                FinancialType.AnnualizedReturn,
                            },
                            color = "#F8CECC",
                            strokeColor = "#B85450"
                        },
                        new StudyScoreType() {
                            name = "產品的獲利能力",
                            serialNo =  StudyScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.PRODUCT,
                            financialList = new List<FinancialType>() {
                                FinancialType.GrossProfitRatio, 
                                FinancialType.OperatingProfitMargin
                            },
                            color = "#FFE6CC",
                            strokeColor =  "#D79B00"
                        },
                        new StudyScoreType() {
                            name = "資產的獲利能力",
                            serialNo =  StudyScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.ASSET,
                            financialList = new List<FinancialType>() {
                                FinancialType.ReturnOfNetTangibleAssets_FirstQuartile,
                                FinancialType.ReturnOfNetTangibleAssets_Median,
                            },
                            color = "#FFF2CC",
                            strokeColor = "#D6B656"
                        },
                        new StudyScoreType() {
                            name = "投機的價值",
                            serialNo = StudyScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.SPECULATION,
                            financialList = new List<FinancialType>() {
                                FinancialType.SalesToPrice, FinancialType.YoY, FinancialType.ONWR
                            },
                            color = "#D5E8D4",
                            strokeColor = "#82B366"
                        },
                        new StudyScoreType() {
                            name = "財務的健康",
                            serialNo = StudyScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.FINANCE,
                            financialList = new List<FinancialType>() {
                                FinancialType.LongDebtRatio
                            },
                            color = "#DAE8FC",
                           strokeColor = "#6C8EBF",
                        },
                        new StudyScoreType() {
                            name = "管理者的能力",
                            serialNo = StudyScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.MANAGER,
                            financialList = new List<FinancialType>() {
                                FinancialType.ReturnOfEquity_FirstQuartile
                            },
                            color = "#E1D5E7",
                            strokeColor = "#9673A6"
                        }
                    }
                }
            }
        };
    }

    protected List<Study> _studyList = new List<Study>();
    public List<Study> studyList
    {
        get
        {
            return this._studyList;
        }
    }

    public void removeStudy(Study study)
    {
        var index = this.studyList.FindIndex((x) => x.serialNo == study.serialNo);
        if (index > -1)
        {
            this.studyList.RemoveAt(index);
            this.get<SaveFile>().save(this.fileName, new IStorageStudyList() { list = this.studyList });
            this.emit("studyList", this._studyList);
        }
    }

    public void setStudy(Study study)
    {
        var find = this.studyList.Find((x) => x.serialNo == study.serialNo);
        if (find != null)
        {
            find.update(study);
        }
        else
        {
            this.studyList.Add(study);
        }
        this.get<SaveFile>().save(this.fileName, new IStorageStudyList() { list = this.studyList });
        this.emit("studyList", this._studyList);
    }

    protected Study _study = null;
    public Study study
    {
        get
        {
            return this._study;
        }
    }

    public void useStudy(Study study)
    {
        this._study = study;
        this.emit("study", study);
    }

    protected Study _editStudy = null;
    public Study editStudy
    {
        get
        {
            return this._editStudy;
        }
    }

    public void setEditStudy(Study editStudy)
    {
        this._editStudy = editStudy;
        this.emit("editStudy", this._editStudy);
    }

    private bool _isOpenStudy = false;
    public bool isOpenStudy
    {
        get
        {
            return this._isOpenStudy;
        }
    }

    public void switchStudy(bool isOpenStudy)
    {
        this._isOpenStudy = isOpenStudy;
        this.emit("isOpenStudy", this._isOpenStudy);
    }

    private bool _isEdit = false;
    public bool isEdit
    {
        get
        {
            return this._isEdit;
        }
    }

    public void switchEdit(bool isEdit)
    {
        this._isEdit = isEdit;
        this.emit("isEdit", this._isEdit);
    }

    protected StudyScoreType _editStudyScore = null;
    public StudyScoreType editStudyScore
    {
        get
        {
            return this._editStudyScore;
        }
    }

    public void setEditStudyScore(StudyScoreType editStudyScore)
    {
        this._editStudyScore = editStudyScore;
        this.emit("editStudyScore", this._editStudyScore);
    }

    private string _studyStockId = "2330";
    public string studyStockId {
        get { 
            return this._studyStockId;
        }
    }

    public void setStudyStockId(string studyStockId) {
        this._studyStockId = studyStockId;

        this.emit("studyStockId", this._studyStockId);
    }

    private ScoreType _selectedScoreType = ScoreType.INVESTMENT;
    public ScoreType selectedScoreType
    {
        get
        {
            return this._selectedScoreType;
        }
    }

    public void setSelectedScoreType(ScoreType selectedScoreType)
    {
        this._selectedScoreType = selectedScoreType;

        this.emit("selectedScoreType", this._selectedScoreType);
    }
}

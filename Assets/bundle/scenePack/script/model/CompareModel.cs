using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public enum SideType { 
    Left,
    Right
}

public class ItemCompareScorePageData
{
    public ScoreType scoreType;
    public float left;
    public float right;
    public List<ItemCompareScorePageFinancialRatioData> financialRatioList;
}

public class ItemCompareScorePageFinancialRatioData
{
    public ScoreType scoreType;
    public FinancialType financialType;
    public float left;
    public float right;
}

public class ItemCompareScoreTabData
{
    public ScoreType scoreType;
    public bool selected = false;
}
public class ItemCompareScoreHintData
{
    public ScoreType scoreType;
    public float left;
    public float right;
    public bool selected = false;
}


public class CompareScore
{
    public string stockId;
    public List<CompareSubScore> compareSubScoreList;
}

public class CompareFinancialScore
{
    public FinancialType financialType;
    public float score;
}

public class CompareSubScore
{
    public ScoreType scoreType;
    public float score;
    public List<CompareFinancialScore> compareFinancialScoreList;
}


public class IStorageCompareList
{
    public List<Compare> list;
}

public class CompareModel : BaseModel
{
    public string fileName = "compare";
    public override void Awake()
    {
        base.Awake();

        this.on<GlobalData>("stockScoreList", this.refreshStockScoreList);

        this._compareList = this.getCompareList();
        int maxSerialCounter_Compare = this._compareList.Select(x => x.serialNo).Max();
        Compare.serialCounter.replace(maxSerialCounter_Compare);
        int maxSerialCounter_CompareScoreType = this._compareList.Select(x => x.scoreTypeList.Select(y => y.serialNo).Max()).Max();
        CompareScoreType.serialCounter.replace(maxSerialCounter_CompareScoreType);

        this.useCompare(this._compareList[0]);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private List<CompareScore> _compareScoreList = new List<CompareScore>();
    public List<CompareScore> compareScoreList
    {
        get {
            return this._compareScoreList;
        }
    }

    protected void refreshStockScoreList(object obj)
    {
        var stockScoreList = obj as List<IStockScoreData>;
        this._compareScoreList = stockScoreList.Select((x) => {
            return new CompareScore()
            {
                stockId = x.stockId,
                compareSubScoreList = this.compare.scoreTypeList.Select((y) => {
                    var compareFinancialScoreList = y.financialList.Select((financialType) => {
                        return new CompareFinancialScore()
                        {
                            financialType = financialType,
                            score = x.GetValue<float>(financialType.ToString())
                        };
                    }).ToList();

                    return new CompareSubScore()
                    {
                        scoreType = y.scoreType,
                        score = this.getScore(compareFinancialScoreList),
                        compareFinancialScoreList = compareFinancialScoreList
                    };
                }).ToList()
            };
        }).ToList();

        this.emit("compareScoreList", this.compareScoreList);
    }

    protected float getScore(List<CompareFinancialScore> compareFinancialScoreList)
    {
        if (compareFinancialScoreList.Count > 0)
        {
            return compareFinancialScoreList.Select((x) => x.score).Sum() / compareFinancialScoreList.Count;
        }

        return 0;
    }

    protected List<Compare> getCompareList()
    {
        IStorageCompareList jsonObject;
        if (this.get<SaveFile>().exists(this.fileName))
        {
            var loadObject = this.get<SaveFile>().load<IStorageCompareList>(this.fileName);
            if (loadObject != null)
            {
                jsonObject = loadObject;
            }
            else
            {
                jsonObject = this.getDefaultCompareList();
            }
        }
        else
        {
            jsonObject = this.getDefaultCompareList();
        }

        return jsonObject.list;
    }

    protected IStorageCompareList getDefaultCompareList()
    {
        return new IStorageCompareList()
        {
            list = new List<Compare>() {
                new Compare()
                {
                    name = "研究",
                    serialNo = Compare.serialCounter.generateSerialNo(),
                    scoreTypeList = new List<CompareScoreType>
                    {
                        new CompareScoreType() {
                            name = "投資的價值",
                            serialNo = CompareScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.INVESTMENT,
                            financialList = new List<FinancialType>() {
                                FinancialType.AnnualizedReturn
                            },
                            color = "#F8CECC",
                            strokeColor = "#B85450"
                        },
                        new CompareScoreType() {
                            name = "產品的獲利能力",
                            serialNo = CompareScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.PRODUCT,
                            financialList = new List<FinancialType>() {
                                FinancialType.GrossProfitRatio, FinancialType.OperatingProfitMargin
                            },
                            color = "#FFE6CC",
                            strokeColor =  "#D79B00"
                        },
                        new CompareScoreType() {
                            name = "資產的獲利能力",
                            serialNo = CompareScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.ASSET,
                            financialList = new List<FinancialType>() {
                                FinancialType.ReturnOfNetTangibleAssets_FirstQuartile,
                                FinancialType.ReturnOfNetTangibleAssets_Median
                            },
                            color = "#FFF2CC",
                            strokeColor = "#D6B656"
                        },
                        new CompareScoreType() {
                            name = "投機的價值",
                            serialNo = CompareScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.SPECULATION,
                            financialList = new List<FinancialType>() {
                                FinancialType.SalesToPrice, FinancialType.YoY, FinancialType.ONWR
                            },
                            color = "#D5E8D4",
                            strokeColor = "#82B366"
                        },
                        new CompareScoreType() {
                            name = "財務的健康",
                            serialNo = CompareScoreType.serialCounter.generateSerialNo(),
                            scoreType = ScoreType.FINANCE,
                            financialList = new List<FinancialType>() {
                                FinancialType.LongDebtRatio
                            },
                            color = "#DAE8FC",
                           strokeColor = "#6C8EBF",
                        },
                        new CompareScoreType() {
                            name = "管理者的能力",
                            serialNo = CompareScoreType.serialCounter.generateSerialNo(),
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

    protected List<Compare> _compareList = new List<Compare>();
    public List<Compare> compareList
    {
        get
        {
            return this._compareList;
        }
    }

    public void removeCompare(Compare compare)
    {
        var index = this.compareList.FindIndex((x) => x.serialNo == compare.serialNo);
        if (index > -1)
        {
            this.compareList.RemoveAt(index);
            this.get<SaveFile>().save(this.fileName, new IStorageCompareList() { list = this.compareList });
            this.emit("compareList", this._compareList);
        }
    }

    public void setCompare(Compare compare)
    {
        var find = this.compareList.Find((x) => x.serialNo == compare.serialNo);
        if (find != null)
        {
            find.update(compare);
        }
        else
        {
            this.compareList.Add(compare);
        }
        this.get<SaveFile>().save(this.fileName, new IStorageCompareList() { list = this.compareList });
        this.emit("compareList", this._compareList);
    }

    protected Compare _compare = null;
    public Compare compare
    {
        get
        {
            return this._compare;
        }
    }

    public void useCompare(Compare compare)
    {
        this._compare = compare;
        this.emit("compare", compare);
    }

    protected Compare _editCompare = null;
    public Compare editCompare
    {
        get
        {
            return this._editCompare;
        }
    }

    public void setEditCompare(Compare editCompare)
    {
        this._editCompare = editCompare;
        this.emit("editCompare", this._editCompare);
    }

    private bool _isOpenCompare = false;
    public bool isOpenCompare
    {
        get
        {
            return this._isOpenCompare;
        }
    }

    public void switchCompare(bool isOpenCompare)
    {
        this._isOpenCompare = isOpenCompare;
        this.emit("isOpenCompare", this._isOpenCompare);
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

    protected CompareScoreType _editCompareScore = null;
    public CompareScoreType editCompareScore
    {
        get
        {
            return this._editCompareScore;
        }
    }

    public void setEditCompareScore(CompareScoreType editCompareScore)
    {
        this._editCompareScore = editCompareScore;
        this.emit("editCompareScore", this._editCompareScore);
    }

    private string _leftStockId = "2330";
    public string leftStockId
    {
        get { 
            return this._leftStockId;
        }
    }

    public void setLeftStockId(string leftStockId) {
        this._leftStockId = leftStockId;

        this.emit("leftStockId", this._leftStockId);
    }

    private string _rightStockId = "3034";
    public string rightStockId
    {
        get
        {
            return this._rightStockId;
        }
    }

    public void setRightStockId(string rightStockId)
    {
        this._rightStockId = rightStockId;

        this.emit("rightStockId", this._rightStockId);
    }

    public Color leftColor = Colors.FromHex("#F5F5F5");
    public Color leftStrokeColor = Colors.FromHex("#666666");
    public Color rightColor = Colors.FromHex("#666666");
    public Color rightStrokeColor = Colors.FromHex("#F5F5F5");
}

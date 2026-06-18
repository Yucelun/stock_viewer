using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using UnityEngine;

public class SheetModel : BaseModel
{
    public string updateFileName = "updateTime";
    public string fileName = "stockFinancial";
    public override void Awake()
    {
        base.Awake();
    }

    public bool needUpdate()
    {
        if (!this.get<SaveFile>().exists(this.updateFileName))
        {
            return true;
        }

        var old = this.get<SaveFile>().load<DateTime>(this.updateFileName);
        DateTime now = DateTime.Now;
        if (old.Year != now.Year) { 
            return true;
        }
        
        if (old.Month != now.Month) { 
            return true;
        }

        if (old.Day != now.Day)
        {
            return true;
        }

        if (old.Hour < 18)
        {
            if (now.Hour >= 18)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public List<IStockFinancialData> query(bool force = false)
    {
        if (force || this.get<SheetModel>().needUpdate())
        {
            this.get<GlobalData>().setLoadingLog("更新資料....");
            var source = this.load();
            foreach (var item in keyMap)
            {
                source = source.Replace(item.Key, item.Value);
            }
            var result = CSVParser.Deserialize<IStockFinancialData>(source).ToList();
            this.get<SaveFile>().save(this.fileName, result);
            this.get<SaveFile>().save(this.updateFileName, DateTime.Now);
            return result;
        }
        else
        {
            this.get<GlobalData>().setLoadingLog("讀取資料....");
            return this.get<SaveFile>().load<List<IStockFinancialData>>(this.fileName);
        }
    }

    protected string load()
    {
        var sheetId = "1T8prv6D82Ld6Ht1D_2WzHhLQwItet2nR0wuApQcP33g";
        var sheetName = HttpUtility.UrlEncode("combin_all_full");
        var sheetURL = string.Format("https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:csv&sheet={1}", sheetId, sheetName);
        var request = WebRequest.Create(sheetURL);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());
        string results = sr.ReadToEnd();
        sr.Close();

        return results;
    }


    protected Dictionary<string, string> keyMap = new Dictionary<string, string>() {
        { "內在價值(Q1)", "IntrinsicValue_FirstQuartile" }, //
        { "內在價值(Q2)", "IntrinsicValue_Median" }, //
        { "內在價值(Q3)", "IntrinsicValue_ThirdQuartile" }, //
        { "分析報告", "reportUrl" },
        { "分析資料狀態", "reportStatus" },
        { "年化報酬率", "AnnualizedReturn" }, //
        { "季度數量", "QuarterLength" }, //
        { "收盤價", "price" }, //
        { "本業資金占總資產比率", "ONWR" }, //
        { "毛利率", "GrossProfitRatio" }, //
        { "淨有形資產收益率(Q1)", "ReturnOfNetTangibleAssets_FirstQuartile" }, //
        { "淨有形資產收益率(Q2)", "ReturnOfNetTangibleAssets_Median" }, //
        { "營收年增率(當月)", "YoY" }, //
        { "營業利益率", "OperatingProfitMargin" }, //
        { "股東權益報酬率(Q1)", "ReturnOfEquity_FirstQuartile" }, //
        { "股票代號", "stockId" },
        { "要求報酬率(Q1)", "ReturnOfRequest" }, //
        { "證券名稱", "name" },
        { "長期債務比率", "LongDebtRatio" }, //
        { "營收市值比", "SalesToPrice" }, //
        { "安全邊際", "SaveMargin" } //
    };
}

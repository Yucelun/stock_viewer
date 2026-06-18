using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UiOptionalStocksList : BaseUIComponent
{
    [SerializeField]
    protected Grid grid;

    protected void Awake() {
        this.on<OptionalStocksModel>("optionalStocks", this.refreshOptionalStocks);

        this.grid.on("onRow", this.doRow);

        this.grid.columns = new List<string>() { "股票代號", "公司名稱", "安全邊際" };
    }

    protected void refreshOptionalStocks(object obj)
    {
        this.refreshGrid(obj as OptionalStocks, this.get<GlobalData>().stockFinancialList);
    }

    protected void refreshGrid(OptionalStocks optionalStocks, List<IStockFinancialData> stockFinancialList)
    {
        var optionalStocksStockFinancialList = optionalStocks.stockList.Select(x => {
            var stock = stockFinancialList.Find(y => x.stockId == y.stockId);
            return new List<ICell>() { new ICell() { value = stock.stockId }, new ICell() { value = stock.name }, new ICell() { value = Common.numberToPercent(stock.SaveMargin, 2) } };
        }).ToList();

        this.grid.replace(new IGrid() { propertyList = optionalStocksStockFinancialList });
    }

    protected void doRow(object obj) { 
        var cellList = obj as List<ICell>;
        var stockId = cellList[0].value as string;
        this.get<StudyModel>().setStudyStockId(stockId);
        this.get<PageModel>().setPageIndex(PageIndex.Study);
        this.get<PageModel>().setPrePageIndex(PageIndex.OptionalStocks);
    }
}

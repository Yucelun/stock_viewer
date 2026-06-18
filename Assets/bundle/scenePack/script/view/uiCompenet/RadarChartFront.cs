using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadarChartFront : BaseUIComponent
{
    [SerializeField]
    public UiPolygon bg;

    [SerializeField]
    public UiPolygon line;

    [SerializeField]
    public RadarChartInfo _bgInfo;
    public RadarChartInfo bgInfo
    {
        set
        {
            this._bgInfo = value;
            if (this.bg)
            {
                this.bg.color = this._bgInfo.color;
                this.bg.thickness = this._bgInfo.thickness;
                this.bg.propertyList = this._bgInfo.propertyList.Select(x => x / this._bgInfo.maxScore).ToList();
                this.bg.fill = this._bgInfo.fill;
                this.bg.rectTransform.offsetMax = Vector2.zero;
                this.bg.rectTransform.offsetMin = Vector2.zero;
                this.bg.SetVerticesDirty();
            }
        }
    }

    [SerializeField]
    public RadarChartInfo _lineInfo;
    public RadarChartInfo lineInfo
    {
        set
        {
            this._lineInfo = value;
            if (this.line)
            {
                this.line.color = this._lineInfo.color;
                this.line.thickness = this._lineInfo.thickness;
                this.line.propertyList = this._lineInfo.propertyList.Select(x => x / this._lineInfo.maxScore).ToList();
                this.line.fill = this._lineInfo.fill;
                this.line.rectTransform.offsetMax = Vector2.zero;
                this.line.rectTransform.offsetMin = Vector2.zero;
                this.line.SetVerticesDirty();
            }
        }
    }
}

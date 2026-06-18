using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RadarChartInfo
{
    public List<float> propertyList = new List<float>();
    public Color color = Colors.FromHex("#B1DDF0");
    public int maxScore = 10;
    public float thickness = 2;
    public bool fill = false;
}

[Serializable]
public class RadarChartFrontInfo
{
    public RadarChartInfo bg = new RadarChartInfo()
    {
        color = Colors.FromHex("#B1DDF088"),
         fill = true
};

    public RadarChartInfo line = new RadarChartInfo()
    {
        color = Colors.FromHex("#10739E")
    };
}

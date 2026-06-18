using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class Common
{
    public static string numberWithCommas(float value, int fractionDigits = 0)
    {
        var _fixed = Mathf.Pow(10, fractionDigits); // 改無條件捨去
        return (Mathf.Floor(value * _fixed) / _fixed).ToString("N" + fractionDigits, new CultureInfo("en-US"));
    }

    public static string numberToPercent(float value, int fractionDigits = 0)
    {
        var _fixed = Mathf.Pow(10, fractionDigits); // 改無條件捨去
        return (Mathf.Floor(value * _fixed) / _fixed).ToString("P" + fractionDigits, new CultureInfo("en-US"));
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Globalization;
using System.Linq;
using System;
using System.Reflection;

public class Colors
{
    public static Color FromHex(string hex)
    {
        if (hex.Length < 7)
        {
            throw new System.FormatException("Needs a string with a length of at least 6");
        }

        var r = hex.Substring(1, 2);
        var g = hex.Substring(3, 2);
        var b = hex.Substring(5, 2);
        string alpha;
        if (hex.Length >= 9)
            alpha = hex.Substring(7, 2);
        else
            alpha = "FF";

        return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
                        (int.Parse(g, NumberStyles.HexNumber) / 255f),
                        (int.Parse(b, NumberStyles.HexNumber) / 255f),
                        (int.Parse(alpha, NumberStyles.HexNumber) / 255f));
    }
}
public static class MyExtensions
{
    public static T GetValue<T>(this object obj, string name)
    {
        var type = obj.GetType();
        var fieldinfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        var find = fieldinfo.Find(x => x.Name == name);
        if (find == null)
        {
            return default(T);
        }
        else { 
            return (T)find.GetValue(obj);
        }
    }
    public static void SetValue(this object obj, string name, object value)
    {
        var type = obj.GetType();
        var fieldinfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList();
        var find = fieldinfo.Find(x => x.Name == name);
        if (find != null)
        {
            find.SetValue(obj, value);
        }
    }
}
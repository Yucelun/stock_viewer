using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.ComponentModel;

[ExecuteInEditMode]
[AddComponentMenu("Chart/RadarChart")]
public class RadarChart : BaseUIComponent
{
    [SerializeField]
    public int maxProperty = 3;

    [SerializeField]
    public int maxFront = 1;

    [SerializeField]
    public List<RadarChartFrontInfo> frontInfoList = new List<RadarChartFrontInfo>();

    [SerializeField]
    protected List<RadarChartFront> frontList = new List<RadarChartFront>();

    [SerializeField]
    public Color _bgColor = Color.gray;
    public Color bgColor
    {
        set
        {
            this._bgColor = value;
        }
    }


    [SerializeField]
    public Color _lineColor = Colors.FromHex("#444444");
    public Color lineColor
    {
        set
        {
            this._lineColor = value;
        }
    }

    [SerializeField]
    public UiPolygon bg;

    [SerializeField]
    public GameObject groupLevel;

    [SerializeField]
    public GameObject groupFront;

    [SerializeField]
    public int maxLevel = 5;

    [SerializeField]
    public int maxScore = 10;

    [SerializeField]
    protected List<UiPolygon> levelList = new List<UiPolygon>();

    [SerializeField]
    public float _thickness = 2;
    public float thickness
    {
        set
        {
            this._thickness = value;
        }
    }

    protected void refreshLevel()
    {
        if (this.groupLevel)
        {
            if (levelList.Count != maxLevel)
            {
                var count = levelList.Count;
                var diff = maxLevel - count;
                if (diff > 0)
                {
                    for (var i = 0; i < diff; i++)
                    {
                        GameObject levelGo = new GameObject(string.Format("level{0}", i + count));
                        levelGo.transform.parent = groupLevel.transform;
                        var level = levelGo.AddComponent<UiPolygon>();
                        level.rectTransform.anchorMax = Vector2.one;
                        level.rectTransform.anchorMin = Vector2.zero;
                        levelList.Add(level);
                        level.fill = false;
                    }
                }
                else
                {
                    for (var i = diff; i < 0; i++)
                    {
                        var level = levelList[levelList.Count - 1];
                        levelList.RemoveAt(levelList.Count - 1);
                        level.rectTransform.SetParent(null);
                        DestroyImmediate(level.gameObject);
                    }
                }
            }
        }

        var size = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height);
        var radius = size / 2;
        for (var i = 0; i < levelList.Count; i++)
        {
            var level = levelList[i];
            level.thickness = (i == levelList.Count - 1) ? _thickness * 2 : _thickness;
            level.color = this._lineColor;
            level.propertyList = this.propertyMaxList;

            var offset = radius * (maxLevel - (i + 1)) / maxLevel;

            level.rectTransform.offsetMax = new Vector2(-offset, -offset);
            level.rectTransform.offsetMin = new Vector2(offset, offset);
            level.SetVerticesDirty();
        }
    }


    protected void refreshFront()
    {
        if (this.groupFront)
        {
            if (this.frontList.Count != this.maxFront)
            {
                var count = this.frontList.Count;
                var diff = this.maxFront - count;
                if (diff > 0)
                {
                    for (var i = 0; i < diff; i++)
                    {
                        GameObject frontGo = new GameObject(string.Format("front{0}", i + count));
                        frontGo.transform.parent = this.groupFront.transform;
                        var front = frontGo.AddComponent<RadarChartFront>();
                        front.rectTransform.anchorMax = Vector2.one;
                        front.rectTransform.anchorMin = Vector2.zero;
                        front.rectTransform.offsetMax = Vector2.zero;
                        front.rectTransform.offsetMin = Vector2.zero;

                        GameObject frontBgGo = new GameObject(string.Format("frontBg{0}", i + count));
                        frontBgGo.transform.parent = frontGo.transform;
                        var frontBg = frontBgGo.AddComponent<UiPolygon>();
                        frontBg.rectTransform.anchorMax = Vector2.one;
                        frontBg.rectTransform.anchorMin = Vector2.zero;
                        frontBg.rectTransform.offsetMax = Vector2.zero;
                        frontBg.rectTransform.offsetMin = Vector2.zero;
                        frontBg.fill = false;

                        GameObject frontLineGo = new GameObject(string.Format("frontLine{0}", i + count));
                        frontLineGo.transform.parent = frontGo.transform;
                        var frontLine = frontLineGo.AddComponent<UiPolygon>();
                        frontLine.rectTransform.anchorMax = Vector2.one;
                        frontLine.rectTransform.anchorMin = Vector2.zero;
                        frontLine.rectTransform.offsetMax = Vector2.zero;
                        frontLine.rectTransform.offsetMin = Vector2.zero;
                        frontLine.fill = false;

                        front.bg = frontBg;
                        front.line = frontLine;

                        this.frontList.Add(front);
                        this.frontInfoList.Add(new RadarChartFrontInfo());
                    }
                }
                else
                {
                    for (var i = diff; i < 0; i++)
                    {
                        var front = this.frontList[this.frontList.Count - 1];
                        this.frontList.RemoveAt(this.frontList.Count - 1);
                        this.frontInfoList.RemoveAt(this.frontInfoList.Count - 1);
                        front.rectTransform.SetParent(null);
                        DestroyImmediate(front.gameObject);
                    }
                }
            }
        }

        for (var i = 0; i < frontList.Count; i++)
        {
            var front = this.frontList[i];
            var frontInfo = this.frontInfoList[i];

            this.refreshPropertyList(frontInfo.bg.propertyList, maxProperty);
            this.refreshPropertyList(frontInfo.line.propertyList, maxProperty);

            front.bgInfo = frontInfo.bg;
            front.lineInfo = frontInfo.line;
        }
    }

    protected void refreshPropertyList(List<float> propertyList, int maxProperty)
    {
        var count = propertyList.Count;
        var diff = maxProperty - count;
        if (diff > 0)
        {
            for (var j = 0; j < diff; j++)
            {
                propertyList.Add(1f);
            }
        }
        else
        {
            for (var j = diff; j < 0; j++)
            {
                propertyList.RemoveAt(levelList.Count - 1);
            }
        }
    }

    protected List<float> propertyMaxList = new List<float>();
    protected void refreshProperty()
    {
        this.propertyMaxList.Clear();
        for (var i = 0; i < this.maxProperty; i++)
        {
            this.propertyMaxList.Add(1f);
        }

        if (this.bg)
        {
            this.bg.color = this._bgColor;
            this.bg.propertyList = this.propertyMaxList.Select(x => x).ToList();
        }

        this.refreshLevel();

        this.refreshFront();
    }

    private void Update()
    {
        this.refreshProperty();
    }

#if UNITY_EDITOR
    [MenuItem("GameObject/Chart/Radar", false, 10)]
    static void createRadarGameObject(MenuCommand menuCommand)
    {
        GameObject radarGo = new GameObject("radarChart");

        GameObject bgGo = new GameObject("bg");
        bgGo.transform.parent = radarGo.transform;
        var bg = bgGo.AddComponent<UiPolygon>();
        var bgTransform = bgGo.GetComponent<RectTransform>();
        bgTransform.anchorMax = Vector2.one;
        bgTransform.anchorMin = Vector2.zero;
        bgTransform.offsetMax = Vector2.zero;
        bgTransform.offsetMin = Vector2.zero;

        GameObject groupLevelGo = new GameObject("groupLevel");
        groupLevelGo.transform.parent = radarGo.transform;
        var groupLevel = groupLevelGo.AddComponent<RectTransform>();
        groupLevel.anchorMax = Vector2.one;
        groupLevel.anchorMin = Vector2.zero;
        groupLevel.offsetMax = Vector2.zero;
        groupLevel.offsetMin = Vector2.zero;

        GameObject groupFrontGo = new GameObject("groupFront");
        groupFrontGo.transform.parent = radarGo.transform;
        var groupFront = groupFrontGo.AddComponent<RectTransform>();
        groupFront.anchorMax = Vector2.one;
        groupFront.anchorMin = Vector2.zero;
        groupFront.offsetMax = Vector2.zero;
        groupFront.offsetMin = Vector2.zero;

        RadarChart radar = radarGo.AddComponent<RadarChart>();
        radar.bg = bg;
        radar.groupLevel = groupLevelGo;
        radar.groupFront = groupFrontGo;
        radar.refreshLevel();
        radar.refreshFront();

        GameObjectUtility.SetParentAndAlign(radarGo, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(radarGo, "Create " + radarGo.name);
        Selection.activeObject = radarGo;
    }
#endif
}

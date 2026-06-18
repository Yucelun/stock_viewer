using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[DisallowMultipleComponent]
public class UiPolygon : MaskableGraphic
{
    [SerializeField]
    [Range(0, 1)]
    private List<float> _propertyList = new List<float>();
    public List<float> propertyList {
        set {
            this._propertyList = value;
            SetVerticesDirty();
        }
    }

    [SerializeField]
    [Range(0, 360)]
    public float _rotation = 90;

    [SerializeField]
    public float _thickness = 1;
    public float thickness
    {
        set
        {
            this._thickness = value;
            SetVerticesDirty();
        }
    }


    [SerializeField]
    public bool fill = true;

    [SerializeField]
    public float size = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        size = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height);
        thickness = (float)Mathf.Clamp(_thickness, 0, size / 2);
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (this._propertyList.Count < 3)
        {
            return;
        }

        var vertorList = this.getVectorList(this._propertyList);
        // 外緣
        for (int i = 0; i < vertorList.Count; i++)
        {
            var v = vertorList[i];
            var value = this._propertyList[i];
            float outer = rectTransform.pivot.x * size * value;
            var vector = v * outer;
            vh.AddVert(vector, this.color, new Vector2(0f, 0f)); // 1
        }
        // 內緣
        for (int i = 0; i < vertorList.Count; i++)
        {
            var v = fill ? Vector3.zero : vertorList[i];
            var value = this._propertyList[i];
            float inner = rectTransform.pivot.x * size * value - _thickness;
            var vector = v * inner;
            vh.AddVert(vector, this.color, new Vector2(0f, 0f)); // 1
        }

        for (int i = 0; i < vertorList.Count; i++)
        {
            // 內1, 外1, 外2
            vh.AddTriangle(i % vertorList.Count + vertorList.Count, i % vertorList.Count, (i+1) % vertorList.Count);
            // 外2, 內1, 內2
            vh.AddTriangle((i + 1) % vertorList.Count, i % vertorList.Count + vertorList.Count, (i + 1) % vertorList.Count + vertorList.Count);
        }
    }

    protected List<Vector3> getVectorList(List<float> sourceList)
    {
        var subAngle = 360 / sourceList.Count;
        return sourceList.Select((x, i) => {
            var angle = this._rotation + subAngle * i;
            var degree = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(degree), Mathf.Sin(degree));
        }).ToList();
    }
}

using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class List : BaseUIComponent
{
    private List<object> listData = new List<object>();
    private List<ItemRenderer> listItemRenderer = new List<ItemRenderer>();

    public int count {
        get {
            return listData.Count;
        }
    }

    [SerializeField]
    private GameObject itemPrefab;

    private bool reverse = false;

    public void replace(List<object> listData, bool reverse = false) {
        this.reverse = reverse;

        this.listData.Clear();
        this.listData.AddRange(listData);

        this.doDataChange(this.listData);
    }

    protected void doDataChange(List<object> listData)
    {
        if (this.listItemRenderer.Count != listData.Count)
        {
            var count = this.listItemRenderer.Count;
            var diff = listData.Count - count;

            if (diff > 0)
            {
                for (var i = 0; i < diff; i++) {
                    var newItem = Instantiate(this.itemPrefab);
                    newItem.transform.SetParent(this.rectTransform, false);
                    var newItemRenderer = newItem.GetComponent<ItemRenderer>();
                    newItemRenderer.on("onClick", this.doItemClick);
                    this.listItemRenderer.Add(newItemRenderer);
                }
            }
            else
            {
                for (var i = diff; i < 0; i++) {
                    var itemRenderer = listItemRenderer[this.listItemRenderer.Count - 1];
                    this.listItemRenderer.RemoveAt(this.listItemRenderer.Count - 1);
                    itemRenderer.rectTransform.SetParent(null);
                    itemRenderer.off("onClick", this.doItemClick);
                    Destroy(itemRenderer.gameObject);
                }
            }
        }

        var useList = new List<object>();
        useList.AddRange(listData);
        if (this.reverse) {
            useList.Reverse();
        }

        for (var i = 0; i < useList.Count; i++)
        {
            var itemRenderer = this.listItemRenderer[i];
            itemRenderer.index = this.reverse ? useList.Count - 1 - i : i;
            itemRenderer.data = useList[i];
        }
    }

    public void doItemClick(object item) {
        this.emit("onItemClick", item);
    }
}

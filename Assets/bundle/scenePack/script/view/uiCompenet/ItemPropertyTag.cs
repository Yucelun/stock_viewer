using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class IItemPropertyTag
{
    public string caption;
    public float value;
    public Vector3 position;
}


public class ItemPropertyTag : ItemRenderer
{

    [SerializeField]
    protected Text textPropertyCaption;

    [SerializeField]
    protected Text textProperty;

    protected void Awake() {
    }

    protected override void doDataChanged()
    {
        var itemPropertyTag = this.data as IItemPropertyTag;
        var caption = itemPropertyTag.caption;
        var value = itemPropertyTag.value;
        var position = itemPropertyTag.position;

        this.textPropertyCaption.text = caption;
        this.textProperty.text = value.ToString();
        // TODO 設置位置
        //this.node.setPosition(position);
        //this.getComponent(Layout).verticalDirection = [2, 3, 4].indexOf(this.index) > -1 ? Layout.VerticalDirection.BOTTOM_TO_TOP : Layout.VerticalDirection.TOP_TO_BOTTOM;
    }
}

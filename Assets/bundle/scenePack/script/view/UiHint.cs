using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiHint : BaseUI
{
    [SerializeField]
    protected Text textContent;

    [SerializeField]
    protected Button btnConfirm;

    [SerializeField]
    protected Button btnCancel;

    protected override void Awake() {
        base.Awake();

        this.btnConfirm.onClick.AddListener(this.doConfirm);
        this.btnCancel.onClick.AddListener(this.doCancel);
    }

    protected void doConfirm()
    {
        this.emit("onResult", true);
        this.exit();
    }

    protected void doCancel()
    {
        this.emit("onResult", false);
        this.exit();
    }

    public void showText(string content) {
        this.textContent.text = content;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTest : BaseUI
{
    [SerializeField]
    protected Button btnTest;

    [SerializeField]
    protected Text textTest;

    protected override void Awake() {
        base.Awake();

        this.btnTest.onClick.AddListener(this.doTest);
    }

    protected void Start() { }

    protected void doTest()
    {
        this.textTest.text = "觸發 click";
    }
}

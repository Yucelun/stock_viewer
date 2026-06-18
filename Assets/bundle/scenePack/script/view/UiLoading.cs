using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLoading : BaseUI
{
    [SerializeField]
    protected RawImage rawImageLoading;

    [SerializeField]
    protected Text textInfo;

    protected override void Awake()
    {
        base.Awake();

        this.get<GlobalData>().on("loadingLog", this.showLoadingLog);
    }

    protected void Start()
    {
        this.rawImageLoading.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.showLoadingLog(this.get<GlobalData>().loadingLog);
    }

    protected void Update()
    {
        this.rawImageLoading.transform.Rotate(0.0f, 0.0f, Time.deltaTime * 200);

    }

    protected void showLoadingLog(object content)
    {
        string loadingLog = (string)content;
        if (loadingLog == "")
        {
            this.exit();
        }
        else
        {
            this.textInfo.text = loadingLog;
        }
    }
}

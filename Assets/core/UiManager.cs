using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum NodeParent
{
    SCENE,
    MAIN,
    MASK,
    UI,
    ALERT,
    SYSTEM,
    WAITTING
}

public interface ISceneUiNodes
{
    public GameObject nodeScene { get; }
    public GameObject nodeMain { get; }
    public GameObject nodeMask { get; }
    public GameObject nodeUI { get; }
    public GameObject nodeAlert { get; }
    public GameObject nodeSystem { get; }
    public GameObject nodeWaiting { get; }
}


public class UiManager : BaseSingleton
{
    private string bundleResourcesPath = ""; //Bundle
    private string prefabPath = "prefab/"; //預設Prefab路徑
    private bool maskAlwaysActive = false; //是否開啟常駐，源於有些loading等待畫面直接不給予點擊相關功能
    private ISceneUiNodes sceneUiNodes = null;
    private Dictionary<NodeParent, GameObject> nodes = new Dictionary<NodeParent, GameObject>();
    public override void Awake()
    {
    }

    public override void Start()
    {
    }

    public override void OnDestroy()
    {
    }
    public void setSceneUiNodes(ISceneUiNodes sceneUiNodes)
    {
        this.sceneUiNodes = sceneUiNodes;

        //做淺拷貝引用對象
        this.nodes.Add(NodeParent.SCENE, this.sceneUiNodes.nodeScene);
        this.nodes.Add(NodeParent.MAIN, this.sceneUiNodes.nodeMain);
        this.nodes.Add(NodeParent.MASK, this.sceneUiNodes.nodeMask);
        this.nodes.Add(NodeParent.UI, this.sceneUiNodes.nodeUI);
        this.nodes.Add(NodeParent.ALERT, this.sceneUiNodes.nodeAlert);
        this.nodes.Add(NodeParent.SYSTEM, this.sceneUiNodes.nodeSystem);
        this.nodes.Add(NodeParent.WAITTING, this.sceneUiNodes.nodeWaiting);
        this.nodes[NodeParent.WAITTING].SetActive(false);
        this.nodes[NodeParent.MASK].SetActive(false);

        this.nodes[NodeParent.MASK].GetComponent<Button>().onClick.AddListener(this.doMaskClick);
    }

    protected void doMaskClick()
    {
        this.emit("onMaskClick", null);
    }

    public string bundleResPath
    {
        get
        {
            return this.bundleResourcesPath;
        }
        set { 
            this.bundleResourcesPath = value;
        }
    }

    public void initSetting()
    {
        this.maskAlwaysActive = false; //取消常駐

        this.emit("onInitSetting", null);
    }

    public void setMaskAttr(bool alwaysActive)
    {
        this.maskAlwaysActive = alwaysActive;
    }

    public bool getMaskAttr() {
        return this.maskAlwaysActive;
    }

    public T openWindow<T>(string bundleName, string uiName, NodeParent parent, bool showLoading = false) where T : BaseUI {
        var fullPath = bundleName + "/" + this.prefabPath + uiName;
        if (showLoading)
        {
            this.nodes[NodeParent.WAITTING].GetComponent<WaitingMask>().show();
        }

        try
        {
            GameObject prefab = Resources.Load<GameObject>(fullPath);
            GameObject go = UnityEngine.Object.Instantiate(prefab);
            T ui = go.GetComponent<T>();

            if (parent == NodeParent.MASK)
            {
                Debug.LogError("Error: nodeMask cannot have child nodes added to it.");
            }
            else if (parent != NodeParent.SCENE)
            {
                ui.rectTransform.SetParent(this.nodes[parent].transform, false);
                this.nodes[NodeParent.MASK].SetActive(ui.useMask);
            }
            else
            {
                ui.rectTransform.SetParent(this.nodes[NodeParent.SCENE].transform, false);
            }

            if (showLoading)
            {
                this.nodes[NodeParent.WAITTING].GetComponent<WaitingMask>().hide();
            }

            return ui;
        }
        catch (Exception  err)
        {
            if (showLoading)
            {
                this.nodes[NodeParent.WAITTING].GetComponent<WaitingMask>().hide();
            }
            Debug.LogError("Failed to load resources: " + err.ToString());
        }

        return null;
    }

    /**
     * 關閉視窗
     * @param UI 節點
     */
    public void closeUI(BaseUI ui)
    {
        if (ui.transform.parent.Equals(this.nodes[NodeParent.MASK].transform))
        {
            Debug.LogError("Error: nodeMask cannot destroy.");
        }
        else
        {
            ui.transform.SetParent(null);
            UnityEngine.Object.Destroy(ui.gameObject);
            this.checkMaskClose();
        }
    }

    /**
     * 關閉所有視窗
     * @param parent 傳入父層節點名
     */
    public void closeAllUI(NodeParent parent)
    {
        if (parent == NodeParent.MASK)
        {
            Debug.LogError("Error: nodeMask cannot destroy.");
        }
        else
        {

            for (var i = this.nodes[parent].transform.childCount - 1; i > -1; i--) {
                var child = this.nodes[parent].transform.GetChild(i);
                UnityEngine.Object.Destroy(child.gameObject);
            } 
            this.checkMaskClose();
        }
    }

    /**
     * 確認nodeMask是否可以關閉
     */
    private void checkMaskClose()
    {
        //若mask需要常駐，則直接跳出
        if (this.maskAlwaysActive)
        {
            return;
        }

        var childrenCount = new List<NodeParent>() { NodeParent.UI, NodeParent.ALERT, NodeParent.SYSTEM }.Select(x => this.nodes[x].transform.childCount).Sum();
        if (childrenCount == 0)
        {
            this.nodes[NodeParent.MASK].gameObject.SetActive(false);
        }
    }
}

using UnityEngine;

public class SceneController : MonoBehaviour, ISceneUiNodes
{
    [SerializeField]
    public GameObject _nodeScene;
    public GameObject nodeScene {
        get {
            return this._nodeScene;
        }
    }

    [SerializeField]
    public GameObject _nodeMain;
    public GameObject nodeMain
    {
        get
        {
            return this._nodeMask;
        }
    }

    [SerializeField]
    public GameObject _nodeMask;
    public GameObject nodeMask
    {
        get
        {
            return this._nodeMask;
        }
    }

    [SerializeField]
    public GameObject _nodeUI;
    public GameObject nodeUI
    {
        get
        {
            return this._nodeUI;
        }
    }

    [SerializeField]
    public GameObject _nodeAlert;
    public GameObject nodeAlert
    {
        get
        {
            return this._nodeAlert;
        }
    }

    [SerializeField]
    public GameObject _nodeSystem;
    public GameObject nodeSystem
    {
        get
        {
            return this._nodeSystem;
        }
    }

    [SerializeField]
    public GameObject _nodeWaiting;
    public GameObject nodeWaiting
    {
        get
        {
            return this._nodeWaiting;
        }
    }

    protected virtual void  Start()
    {
        // init manager
        this.get<UiManager>().setSceneUiNodes(this);
    }

    public T get<T>() where T : new()
    {
        return Kernel.get<T>();
    }
}

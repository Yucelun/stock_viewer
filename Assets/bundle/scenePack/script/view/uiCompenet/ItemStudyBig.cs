using UnityEngine;
using UnityEngine.UI;

public class ItemStudyBig : ItemRenderer
{
    [SerializeField]
    protected Text textStudyCaption;

    [SerializeField]
    protected Button btnStudy;

    [SerializeField]
    protected Button btnNew;

    [SerializeField]
    protected GameObject groupStudy;

    [SerializeField]
    protected Button btnDelete;

    [SerializeField]
    protected Button btnEdit;

    protected void Awake()
    {
        this.btnStudy.onClick.AddListener(this.doStudy);
        this.btnNew.onClick.AddListener(this.doNew);
        this.btnDelete.onClick.AddListener(this.doDelete);
        this.btnEdit.onClick.AddListener(this.doEdit);
    }

    protected override void doDataChanged()
    {
        if (this.data == null)
        {
            this.btnNew.gameObject.SetActive(true);
            this.groupStudy.SetActive(false);
        }
        else
        {
            var Study = this.data as Study;
            var name = Study.name;
            var isEdit = Study.isEdit;
            this.btnNew.gameObject.SetActive(false);
            this.groupStudy.SetActive(true);
            this.textStudyCaption.text = name;
            this.btnDelete.gameObject.SetActive(isEdit);
            this.btnEdit.gameObject.SetActive(isEdit);
        }
    }

    protected void doStudy()
    {
        this.get<StudyModel>().useStudy(this.data as Study);
    }

    protected void doNew()
    {
        this.get<StudyModel>().setEditStudy(new Study()
        {
            serialNo = Study.serialCounter.generateSerialNo(),
        });
        //this.get<UiManager>().openWindow<UiStudyEdit>("uiPack", "uiStudyEdit", NodeParent.UI);
    }

    protected void doDelete()
    {
        UiHint uiHint = this.get<UiManager>().openWindow<UiHint>("uiPack", "uiHint", NodeParent.UI);
        uiHint.showText("確定要刪除?");
        uiHint.on("onResult", this.doCheckDeleteResult);
    }

    protected void doCheckDeleteResult(object obj)
    {
        var result = (bool)obj;
        if (result)
        {
            this.get<StudyModel>().removeStudy(this.data as Study);
        }
    }

    protected void doEdit()
    {
        var Study = this.data as Study;
        this.get<StudyModel>().setEditStudy(Study);

        //this.get<UiManager>().openWindow<UiStudyEdit>("uiPack", "uiStudyEdit", NodeParent.UI);
    }
}

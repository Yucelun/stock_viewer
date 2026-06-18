using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiStudyMenu : BaseUIComponent
{
    [SerializeField]
    protected GameObject groupBig;

    [SerializeField]
    protected GameObject groupSmall;

    [SerializeField]
    protected List listStudyBig;

    [SerializeField]
    protected List listStudySmall;

    [SerializeField]
    protected Button btnMask;

    [SerializeField]
    protected Button btnOpen;

    [SerializeField]
    protected Button btnClose;


    [SerializeField]
    protected Toggle toggleEdit;

    protected void Awake()
    {
        this.btnOpen.onClick.AddListener(this.doOpen);
        this.btnClose.onClick.AddListener(this.doClose);
        this.btnMask.onClick.AddListener(this.doMask);

        this.toggleEdit.onValueChanged.AddListener(this.doEdit);

        this.on<StudyModel>("studyList", this.refreshStudyList);
        this.on<StudyModel>("isOpenStudy", this.refreshIsOpenStudy);
        this.on<StudyModel>("isEdit", this.refreshIsEdit);
        this.on<PageModel>("pageIndex", this.refreshPageIndex);
    }

    protected void Start()
    {
        this.refreshStudyList(this.get<StudyModel>().studyList);
        this.refreshIsOpenStudy(this.get<StudyModel>().isOpenStudy);
        this.refreshIsEdit(this.get<StudyModel>().isEdit);
        this.refreshPageIndex(this.get<PageModel>().pageIndex);
    }

    protected void refreshPageIndex(object obj)
    {
        var pageIndex = (PageIndex)obj;
        this.gameObject.SetActive(pageIndex == PageIndex.Study);
    }

    protected void refreshIsEdit(object obj)
    {
        var isEdit = (bool)obj;
        this.toggleEdit.SetIsOnWithoutNotify(isEdit);
        this.refreshListStudyBig(this.get<StudyModel>().studyList, isEdit);
        this.btnMask.gameObject.SetActive(isEdit);
    }

    protected void refreshIsOpenStudy(object obj)
    {
        var isOpenStudy = (bool)obj;
        this.groupBig.SetActive(isOpenStudy);
        this.groupSmall.SetActive(!isOpenStudy);
    }

    protected void refreshListStudyBig(List<Study> studyList, bool isEdit)
    {
        var _studyList = studyList.Select(x => x.copy()).ToList();
        _studyList.ForEach(x => x.isEdit = isEdit);
        if (isEdit) {
            _studyList.Add(null);
        }
        this.listStudyBig.replace(_studyList.ToList<object>());
    }

    protected void refreshStudyList(object obj)
    {
        var studyList = obj as List<Study>;
        this.listStudySmall.replace(studyList.ToList<object>());
        this.refreshListStudyBig(studyList, this.get<StudyModel>().isEdit);
    }

    protected void doOpen()
    {
        this.get<StudyModel>().switchStudy(true);
    }

    protected void doClose()
    {
        this.get<StudyModel>().switchStudy(false);
        this.get<StudyModel>().switchEdit(false);
    }

    protected void doMask()
    {
        this.get<StudyModel>().switchStudy(false);
        this.get<StudyModel>().switchEdit(false);
    }

    protected void doEdit(bool isEdit)
    {
        this.get<StudyModel>().switchEdit(isEdit);
    }
}

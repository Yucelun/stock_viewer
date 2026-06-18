using UnityEngine;
using UnityEngine.UI;

public class ItemStudySmall : ItemRenderer
{
    [SerializeField]
    protected Text textStudyCaption;

    [SerializeField]
    protected Button btnStudy;

    protected void Awake()
    {
        this.btnStudy.onClick.AddListener(this.doStudy);
    }

    protected override void doDataChanged()
    {
        if (this.data != null)
        {
            var name = (this.data as Study).name;
            this.textStudyCaption.text = name.Length > 2 ? name.Substring(0, 2) : name;
        }
    }

    protected void doStudy()
    {
        this.get<StudyModel>().useStudy(this.data as Study);
    }
}

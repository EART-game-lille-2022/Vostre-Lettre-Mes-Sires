using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideListOpener : MonoBehaviour
{
    [Tooltip("0 = LowerScrollDown, 1 = LowerScrollUp, 2 = ScrollListOpened, 3 = ScrollListClosed")]
    [SerializeField] List<RectTransform> Anchors;
    [SerializeField] RectTransform LowerScrollRT;
    [SerializeField] RectTransform ScrollListRT;
    [SerializeField] bool listIsOpen;

    private void Start()
    {
        listIsOpen = false;
        CloseList();
    }

    public void OnClick()
    {
        if (listIsOpen)
        {
            listIsOpen = false;
            CloseList();
        }
        else
        {
            listIsOpen = true;
            OpenList();
        }
    }

    void OpenList()
    {

        LowerScrollRT.DOMove(Anchors[0].position, 1f);

        ScrollListRT.DOMove(Anchors[2].position, 1f);
        DOTween.To(() => ScrollListRT.sizeDelta, x => ScrollListRT.sizeDelta = x, Anchors[2].sizeDelta, 1);
    }

    void CloseList()
    {
        LowerScrollRT.DOMove(Anchors[1].position, 1f);

        ScrollListRT.DOMove(Anchors[3].position, 1f);
        DOTween.To(() => ScrollListRT.sizeDelta, x => ScrollListRT.sizeDelta = x, Anchors[3].sizeDelta, 1);

    }

/*    IEnumerator TestOpenClose()
    {
        OpenList();
        yield return new WaitForSeconds(3f);
        CloseList();
        yield return new WaitForSeconds(3f);
        StartCoroutine(TestOpenClose());
    }*/
}

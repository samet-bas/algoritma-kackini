using UnityEngine;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public RectTransform BlokPanel;
    public RectTransform KodPanel;
    public GameObject program;
    public void HidePanelsOnRun()
    {
        BlokPanel.DOAnchorPosX(-700f, 1f);
        KodPanel.DOAnchorPosX(700f, 1f);
    }
    public void ShowPanelsOnStop()
    {
        BlokPanel.DOAnchorPosX(+210f, 1f);
        KodPanel.DOAnchorPosX(-210f, 1f);
    }

    public void ClearProgram()
    {
        foreach (Transform child in program.transform)
        {
            Destroy(child.gameObject);
        }
    }

}

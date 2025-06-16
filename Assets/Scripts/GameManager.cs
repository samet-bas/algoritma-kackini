using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public RectTransform BlokPanel;
    public RectTransform KodPanel;
    public GameObject program;
    public RectTransform victoryPanel;
    public void HidePanelsOnRun()
    {
        BlokPanel.DOAnchorPosX(-180f, 1f);
        KodPanel.DOAnchorPosX(-180f, 1.5f);
    }
    public void ShowPanelsOnStop()
    {
        BlokPanel.DOAnchorPosX(+220f, 1f);
        KodPanel.DOAnchorPosX(+220f, 1.5f);
    }

    public void ClearProgram()
    {
        foreach (Transform child in program.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void EndLevel()
    {
        HidePanelsOnRun();
       victoryPanel.gameObject.SetActive(true);
       victoryPanel.DOScale(0, 1f).From();

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(GameObject.FindGameObjectWithTag("teyip"));
    }

}

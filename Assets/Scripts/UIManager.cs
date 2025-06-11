using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   [SerializeField] private RectTransform menu_bg;
   [SerializeField] private RectTransform menu_title;  
   [SerializeField] private RectTransform menu_start,menu_options,menu_credits;
   [SerializeField] private RectTransform menu_exit;
   [SerializeField] private float revealDuration = 1f;
   [SerializeField] private Transform ui;
    
    void Start()
    {
        menu_bg.DOScale(Vector3.zero, revealDuration).From().SetDelay(0.2f);
        menu_title.DOAnchorPosY(700f, revealDuration).From().SetDelay(0.5f);
        menu_start.DOAnchorPosY(-700f, revealDuration).From().SetDelay(0.5f);
        menu_options.DOAnchorPosY(-700f, revealDuration).From().SetDelay(0.5f);
        menu_credits.DOAnchorPosY(-700f, revealDuration).From().SetDelay(0.5f);
        menu_exit.DOAnchorPosY(700f, revealDuration).From().SetDelay(0.5f);
    }

    public void OnClickTween(RectTransform rt)
    {
        if (DOTween.IsTweening(rt)) return; 
        rt.DOScale(rt.localScale * 1.25f, revealDuration/6f).SetLoops(2, LoopType.Yoyo);
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true); // Panel aktif edilmeli ki animasyon görülsün
        panel.transform.localScale = Vector3.zero;
        panel.transform.DOScale(Vector3.one, 0.2f).SetDelay(revealDuration/6f);
    }

    public void HidePanel(GameObject panel)
    {
        panel.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
        {
            panel.SetActive(false);
        });
    }
    
    public void Exit()
    {
        StartCoroutine(ExitWithDelay(revealDuration/6f));
    }

    private IEnumerator ExitWithDelay(float delay = 1f)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
        
    }

    public void ChangeScene(string scene)
    {
        StartCoroutine(ChangeSceneWithDelay(scene));
    }

    private IEnumerator ChangeSceneWithDelay(string scene,float delay = 1f)
    {
        
        yield return new WaitForSeconds(revealDuration/6f);
        ui.transform.DOScale(Vector3.zero, revealDuration).OnComplete(() =>
        {
            SceneManager.LoadScene(scene);
        });
        
    }
    
}

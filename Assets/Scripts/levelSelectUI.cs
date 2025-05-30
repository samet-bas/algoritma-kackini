using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSelectUI : MonoBehaviour
{
    [SerializeField] private Transform ui;
    [SerializeField] private RectTransform bg;
    [SerializeField] private RectTransform[] levels;
    private float revealDuration=1f;
    void Start()
    {
        bg.anchoredPosition = new Vector2(-2000f, -150f);
        ui.DOScale(Vector3.zero, revealDuration).From().OnComplete(() =>
        {
            bg.gameObject.SetActive(true);
            bg.DOAnchorPosX(0f, revealDuration/3f);
            float i = 0.1f;
                    foreach (RectTransform rt in levels)
                    {
                        rt.gameObject.SetActive(true);
                        rt.DOAnchorPosY(-1080f, revealDuration/3f).From().SetDelay(revealDuration/2+i);
                        i += 0.1f;
                    }
        });
        
        
    }
    public void OnClickTween(RectTransform rt)
    {
        if (DOTween.IsTweening(rt)) return; 
        rt.DOScale(rt.localScale * 1.25f, revealDuration/6f).SetLoops(2, LoopType.Yoyo);
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

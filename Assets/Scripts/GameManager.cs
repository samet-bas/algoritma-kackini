using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RectTransform zoom,dialog;
    public Transform floor, decor, robot;
    
    public RectTransform BlokPanel;
    public RectTransform KodPanel;
    public GameObject program;
    public RectTransform victoryPanel;
    public GameObject PausePanel;
    public Slider volumeSlider;     // UI Slider
    private AudioSource audioSource;
    private const string VolumeKey = "volume";

    void Awake()
    {
        // Eğer bu script sahneler arasında kalıcı olacaksa:
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = GameObject.FindWithTag("teyip").GetComponent<AudioSource>();
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.75f); // Varsayılan %75
        audioSource.volume = savedVolume;
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    public void OnVolumeChanged(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat(VolumeKey, value);
    }

    public void HidePanelsOnRun()
    {
        BlokPanel.DOAnchorPosX(-180f, 1f);
        KodPanel.DOAnchorPosX(180f, 1f);
    }
    public void ShowPanelsOnStop()
    {
        BlokPanel.DOAnchorPosX(+250f, 1f);
        KodPanel.DOAnchorPosX(-250f, 1f);
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

    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        PausePanel.GetComponent<RectTransform>().DOScale(1f, 1f).From().SetEase(Ease.InOutBounce);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

}

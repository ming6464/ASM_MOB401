using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuDialog : MonoBehaviour
{
    [Serializable]
    private class ContentInfo
    {
        public GameObject obj;
        public TextMeshProUGUI scoreText;
    }
    [Serializable]
    private class ContentKillEnemy
    {
        public GameObject obj;
        public TextMeshProUGUI boarText, beeText, snailText;
    }

    [Serializable]
    private class Toggle
    {
        public Button btnClose, btnInfo, btnEnemyKilled, btnAudio, btnBack, btnRestart, btnNext;
    }

    [Serializable]
    private class ContentAudio
    {
        public GameObject obj;
        public Slider sfxSlider, musicSlider;
    }

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private ContentInfo _ctInfo;
    [SerializeField] private ContentKillEnemy _ctKillEnemy;
    [SerializeField] private ContentAudio _ctAudio;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private string _nextChapter;


    [SerializeField]
    private GameObject _iconCoin;

    private bool m_isWin;
    private string m_SceneName;
    void Start()
    {
        m_SceneName = SceneManager.GetActiveScene().name;
        AddAction();
        _titleText.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        _iconCoin.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        _ctAudio.musicSlider.onValueChanged.AddListener((value) => ChangeAudio(value, false));
        _ctAudio.sfxSlider.onValueChanged.AddListener((value) => ChangeAudio(value, true));

        _ctAudio.musicSlider.value = Data.GetAudio(false);
        _ctAudio.sfxSlider.value = Data.GetAudio(true);
    }

    private void AddAction()
    {
        if (_toggle.btnClose) _toggle.btnClose.onClick.AddListener(HandleClose);
        if (_toggle.btnNext) _toggle.btnNext.onClick.AddListener(HandleNext);
        if (_toggle.btnBack) _toggle.btnBack.onClick.AddListener(HandleBack);
        if (_toggle.btnBack) _toggle.btnRestart.onClick.AddListener(HandleRestart);
        if (_toggle.btnInfo) _toggle.btnInfo.onClick.AddListener(HandleShowInfo);
        if (_toggle.btnEnemyKilled) _toggle.btnEnemyKilled.onClick.AddListener(HandleShowEnemyKilled);
        if (_toggle.btnAudio) _toggle.btnAudio.onClick.AddListener(handleShowAudioManager);
    }
    public void ShowDialog(bool isPause = true, bool isWin = false)
    {
        if (_titleText)
        {
            string text = "Over!";
            if (isWin) text = "Won!";
            else if (isPause) text = "Pause!";
            _titleText.text = text;
        }

        Time.timeScale = 0f;
        gameObject.SetActive(true);
        ShowContent(1);
        _toggle.btnBack.gameObject.SetActive(true);
        _toggle.btnRestart.gameObject.SetActive(!isWin);
        _toggle.btnNext.gameObject.SetActive(isWin);
        _toggle.btnClose.gameObject.SetActive(isPause);
        int coin = Data.GetData(true, TagConst.NameInfo.SCORE);
        string ext = "";
        if (coin < 100)
        {
            ext += "0";
            if (coin < 10) ext += "0";
        }

        _ctInfo.scoreText.text = ext + coin;
        _ctKillEnemy.boarText.text = " X " + Data.GetData(false, TagConst.NameEnemy.BOAR);
        _ctKillEnemy.snailText.text = " X " + Data.GetData(false, TagConst.NameEnemy.SNAIL);
        _ctKillEnemy.beeText.text = " X " + Data.GetData(false, TagConst.NameEnemy.BEE);
    }

    private void HandleClose()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void HandleNext()
    {
        Time.timeScale = 1f;
        if (_nextChapter.Length == 0)
        {
            HandleRestart();
        }
        else SceneManager.LoadScene(_nextChapter);
    }

    private void HandleBack()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }

    private void HandleRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(m_SceneName);
    }

    private void HandleShowInfo()
    {
        ShowContent(1);
    }

    private void HandleShowEnemyKilled()
    {
        ShowContent(2);
    }

    private void handleShowAudioManager()
    {
        ShowContent(3);
    }

    private void ShowContent(int state)
    {
        _ctInfo.obj.SetActive(false);
        _ctKillEnemy.obj.SetActive(false);
        _ctAudio.obj.SetActive(false);
        switch (state)
        {
            case 1:
                _ctInfo.obj.SetActive(true);
                break;
            case 2:
                _ctKillEnemy.obj.SetActive(true);
                break;
            case 3:
                _ctAudio.obj.SetActive(true);
                break;
        }
    }


    public void ChangeAudio(float val, bool isSfx)
    {
        AudioManager.Ins.AudioVol(val, isSfx);
        Data.UpdateAudio(val, isSfx);
    }

}

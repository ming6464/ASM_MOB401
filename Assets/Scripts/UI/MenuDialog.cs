using System;
using System.Collections;
using System.Collections.Generic;
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
        public TextMeshProUGUI scoreText, coinText;
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
        public Button btnClose, btnInfo, btnEnemyKilled, btnBack, btnRestart, btnNext;
    }

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private ContentInfo _ctInfo;
    [SerializeField] private ContentKillEnemy _ctKillEnemy;
    [SerializeField] private Toggle _toggle;

    private bool m_isWin;
    void Start()
    {
        AddAction();
    }

    private void AddAction()
    {
        if(_toggle.btnClose) _toggle.btnClose.onClick.AddListener(HandleClose);
        if(_toggle.btnNext) _toggle.btnNext.onClick.AddListener(HandleNext);
        if(_toggle.btnBack) _toggle.btnBack.onClick.AddListener(HandleBack);
        if(_toggle.btnBack) _toggle.btnRestart.onClick.AddListener(HandleRestart);
        if(_toggle.btnInfo) _toggle.btnInfo.onClick.AddListener(HandleShowInfo);
        if(_toggle.btnEnemyKilled) _toggle.btnEnemyKilled.onClick.AddListener(HandleShowEnemyKilled);
    }
    public void ShowDialog(bool isPause = true,bool isWin = false)
    {
        if (_titleText)
        {
            string text = "Over!";
            if (isWin)text = "Won!";
            else if (isPause) text = "Pause!";
            _titleText.text = text;
        }
        
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        ShowContent(true);
        _toggle.btnBack.gameObject.SetActive(true);
        _toggle.btnRestart.gameObject.SetActive(!isWin);
        _toggle.btnNext.gameObject.SetActive(isWin);
        _toggle.btnClose.gameObject.SetActive(isPause);
        _ctInfo.coinText.text = "Coin : " + Data.GetData(true, TagConst.NameInfo.COIN);
        _ctInfo.scoreText.text = "Score : " + Data.GetData(true, TagConst.NameInfo.SCORE);
        _ctKillEnemy.boarText.text = " X "  + Data.GetData(false, TagConst.NameEnemy.BOAR);
        _ctKillEnemy.snailText.text = " X "  + Data.GetData(false, TagConst.NameEnemy.SNAIL);
        _ctKillEnemy.beeText.text = " X " + Data.GetData(false, TagConst.NameEnemy.BEE);
    }

    private void HandleClose()
    { 
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    private void HandleNext(){}
    private void HandleBack(){}

    private void HandleRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Map1");
    }

    private void HandleShowInfo()
    {
        ShowContent(true);
    }

    private void HandleShowEnemyKilled()
    {
        ShowContent(false);
    }

    private void ShowContent(bool isInfo)
    {
        LoadContent(isInfo);
        if(_ctInfo.obj) _ctInfo.obj.SetActive(isInfo);
        if(_ctKillEnemy.obj) _ctKillEnemy.obj.SetActive(!isInfo);
        
    }
    private void LoadContent(bool isInfo)
    {
        if (isInfo)
        {
            Debug.Log("Load Content Info !");
            return;
        }
        Debug.Log("Load Content Enemy Killed !");
    }
    
}

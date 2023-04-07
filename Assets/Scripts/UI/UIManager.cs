using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _keyImg;

    public override void Start()
    {
    }

    public override void Update()
    {
        base.Update();
    }

    public void UpdateScore(int val)
    {
        _scoreText.text = val.ToString();
    }

    public void ShowKey()
    {
        _keyImg.gameObject.SetActive(true);
    }
    
}

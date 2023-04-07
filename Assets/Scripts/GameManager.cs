using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool isOverGame;
    public Color low, high;

    [SerializeField] private MenuDialog _menuDialog;

    public override void Start()
    {
        
    }

    public override void Update()
    {
        if (isOverGame) return;
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape) && _menuDialog)
        {
            _menuDialog.ShowDialog();
        }
    }

    public void StateGame(bool isWin)
    {
        _menuDialog.ShowDialog(false,isWin);
        isOverGame = !isWin;
    }
}
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool isOverGame;
    public Color colorLowHealth = Color.red, colorHighHealth = Color.green;

    [SerializeField] private MenuDialog _menuDialog;

    [SerializeField]
    private AudioManager _audioManager;

    [SerializeField] private AfterImagePool _afteImagePool;

    public override void Awake()
    {
        if (!GameObject.FindGameObjectWithTag(TagConst.AUDIOMANAGER))
        {
            if (!_audioManager) _audioManager = Resources.Load<AudioManager>(TagConst.URL_PREFABS + "AudioManager");
            Instantiate(_audioManager);
        }

        if (!GameObject.FindGameObjectWithTag(TagConst.AFTERIMAGEPOOL))
        {
            if (SceneManager.GetActiveScene().name == "Start") return;
            if (!_afteImagePool) _audioManager = Resources.Load<AudioManager>(TagConst.URL_PREFABS + "AfterImagePool");
            Instantiate(_afteImagePool);
        }

        if (!_menuDialog)
            _menuDialog = GameObject.FindGameObjectWithTag(TagConst.MENUDIALOG)?.GetComponent<MenuDialog>();
    }

    public override void Start()
    {
        if (colorLowHealth.a == 0) colorLowHealth.a = 255;
        if (colorHighHealth.a == 0) colorHighHealth.a = 255;
    }

    public override void Update()
    {
        if (isOverGame) return;
        base.Update();
        if (Input.GetKeyDown(KeyCode.Escape))
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
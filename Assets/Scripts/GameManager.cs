using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool isOverGame, isShowTutorial,isShowDialog;
    public Color colorLowHealth = Color.red, colorHighHealth = Color.green;

    [SerializeField] private MenuDialog _menuDialog;

    [SerializeField]
    private AudioManager _audioManager;

    [SerializeField] private AfterImagePool _afteImagePool;

    [SerializeField] private GameObject _tutorialUI;

    public override void Awake()
    {
        if (!GameObject.FindGameObjectWithTag(TagConst.AUDIOMANAGER))
        {
            if (!_audioManager) _audioManager = Resources.Load<AudioManager>(TagConst.URL_PREFABS + "AudioManager");
            Instantiate(_audioManager);
        }

        string nameScene = SceneManager.GetActiveScene().name;
        
        if (nameScene is "Start") return;

        if (nameScene is "Map1")
        {
            isShowTutorial = true;
            if(!_tutorialUI) _tutorialUI = GameObject.FindGameObjectWithTag("TutorialUI");
            if (_tutorialUI)
            {
                _tutorialUI.SetActive(isShowTutorial);
                _tutorialUI.GetComponent<Animator>()?.Play("Play");
            }
        }

        if (!GameObject.FindGameObjectWithTag(TagConst.AFTERIMAGEPOOL))
        {
            if (!_afteImagePool) _audioManager = Resources.Load<AudioManager>(TagConst.URL_PREFABS + "AfterImagePool");
            Instantiate(_afteImagePool);
        }

        if (!_menuDialog)
            _menuDialog = FindObjectOfType<MenuDialog>();
        
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
        if (isShowTutorial)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!_tutorialUI) return;
                isShowTutorial = false;
                _tutorialUI.SetActive(isShowTutorial);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_menuDialog) return;
            if(!isShowDialog) _menuDialog.ShowDialog();
            else _menuDialog.HandleClose();
        }
    }

    public void StateGame(bool isWin)
    {
        _menuDialog.ShowDialog(false,isWin);
        isOverGame = !isWin;
    }
}
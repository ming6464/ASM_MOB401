using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayMapBtn : MonoBehaviour
{
    private int levelSeleted;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        if (levelSeleted != 0) SceneManager.LoadScene("Map" + levelSeleted);
    }

    public void SelectedLevel(int level)
    {
        levelSeleted = level;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        HideGameOverMenu();
        HideGameMenu();
    }

    public void ShowMainMenu()
    {
        menuCanvas.enabled = true;
    }

    public void HideMainMenu()
    {
        menuCanvas.enabled = false;
    }

    public void ShowGameMenu()
    {
        gameCanvas.enabled = true;
    }

    public void HideGameMenu()
    {
        gameCanvas.enabled = false;
    }
    
    public void ShowGameOverMenu()
    {
        gameOverCanvas.enabled = true;
    }

    public void HideGameOverMenu()
    {
        gameOverCanvas.enabled = false;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        ShowMainMenu();
        HideGameOverMenu();
        HideGameMenu();
        #endif
    }
}

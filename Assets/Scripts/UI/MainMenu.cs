using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        InputManager.SetInputStatus(false);
    }

    public void PlayGame(){
        InputManager.SetInputStatus(true);
        GameManager.GetInstance().SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);  
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void Continue(){
        InputManager.SetInputStatus(true);
        GameManager.GetInstance().LoadGame();
    }

}

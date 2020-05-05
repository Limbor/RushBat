using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;

    private void Awake()
    {
        if(_gameManager == null)
        {
            _gameManager = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    public void Restart()
    {
        UIManager.GetInstance().EndScene();
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static GameManager GetInstance()
    {
        return _gameManager;
    }
}

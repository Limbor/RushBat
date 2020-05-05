using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private int coinNumber = 0;
    private GameObject player;
    private List<GameObject> enemies;
    private List<GameObject> doors;

    public static readonly string GroundTrap = "GroundTrap";
    public static readonly string Enemy = "Enemy";
    public static readonly string FlyingTrap = "FlyingTrap";
    
    protected override void Init()
    {
        player = null;
        enemies = new List<GameObject>();
        doors = new List<GameObject>();
    }

    private void LevelComplete()
    {
        
    }
    
    public int GetCoinNumber()
    {
        return coinNumber;
    }

    public void SetCoinNumber(int change)
    {
        coinNumber += change;
        UIManager.GetInstance().SetCoinNumber(coinNumber);
    }

    public void RegisterPlayer(GameObject player)
    {
        this.player = player;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
    
    public void RegisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy)) return;
        enemies.Add(enemy);
    }

    public void DelEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy)) return;
        enemies.Remove(enemy);
    }
    
    
    public static void Restart()
    {
        UIManager.GetInstance().EndScene();
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

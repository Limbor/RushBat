using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
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
        OpenDoors();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (enemies.Count == 0)
        {
            LevelComplete();
        }
    }

    public void RegisterDoor(GameObject door)
    {
        if (doors.Contains(door)) return;
        doors.Add(door);
    }

    public void OpenDoors()
    {
        foreach (var door in doors)
        {
            door.GetComponent<Door>().Open(false);
        }
        doors.Clear();
    }
    
    public static void Restart()
    {
        UIManager.GetInstance().EndScene();
    }
}

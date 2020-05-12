using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    private GameObject player;
    // 记录所有装备，已出现过的装备不再出现
    private List<string> equipmentList;
    private List<GameObject> enemies;
    private List<GameObject> doors;

    private static readonly object _lock = new object();
    
    public static readonly string GroundTrap = "GroundTrap";
    public static readonly string Enemy = "Enemy";
    public static readonly string FlyingTrap = "FlyingTrap";
    
    protected override void Init()
    {
        player = null;
        enemies = new List<GameObject>();
        doors = new List<GameObject>();
        equipmentList = new List<string>();
        ReadTextAssets();
    }

    private void ReadTextAssets()
    {
        TextAsset text = Resources.Load<TextAsset>("TextAssets/Equipment");
        EquipmentInfoList equipmentInfoList = JsonUtility.FromJson<EquipmentInfoList>(text.text);
        foreach (var equipmentInfo in equipmentInfoList.equipmentList)
        {
            equipmentList.Add(equipmentInfo.enName);
        }
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

    public string RegisterEquipment(string name)
    {
        lock (_lock)
        {
            if (name.Equals("Random"))
            {
                if (equipmentList.Count > 0)
                {
                    var index = Random.Range(0, equipmentList.Count);
                    name = equipmentList[index];
                    equipmentList.RemoveAt(index);
                }
                else name = "Null";
            }
            else
            {
                int i;
                for (i = 0; i < equipmentList.Count; i++)
                {
                    if (equipmentList[i].Equals(name))
                    {
                        equipmentList.RemoveAt(i);
                        break;
                    }
                }
                if (i == equipmentList.Count) name = "Null";
            }
            return name;
        }
    }
    
    public static void Restart()
    {
        UIManager.GetInstance().EndScene();
    }
}

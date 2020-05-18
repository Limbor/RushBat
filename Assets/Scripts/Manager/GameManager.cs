using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    private GameObject player;
    // 记录未出现过的装备
    private List<string> equipmentList;
    // 记录所有装备
    private List<string> allEquipemnt;
    private List<GameObject> enemies;
    private List<GameObject> doors;
    private List<Room> rooms;
    private Room currentRoom;
    private Dictionary<string, EquipmentInfo> equipmentInfoMap;

    private static readonly object _lock = new object();
    
    public static readonly string GroundTrap = "GroundTrap";
    public static readonly string Enemy = "Enemy";
    public static readonly string FlyingTrap = "FlyingTrap";
    public static readonly string Environment = "Environment";
    
    protected override void Init()
    {
        player = null;
        enemies = new List<GameObject>();
        doors = new List<GameObject>();
        rooms = new List<Room>();
        equipmentList = new List<string>();
        allEquipemnt = new List<string>();
        equipmentInfoMap = new Dictionary<string, EquipmentInfo>();
        ReadTextAssets();
    }

    private void ReadTextAssets()
    {
        TextAsset text = Resources.Load<TextAsset>("TextAssets/Equipment");
        EquipmentInfoList equipmentInfoList = JsonUtility.FromJson<EquipmentInfoList>(text.text);
        foreach (var equipmentInfo in equipmentInfoList.equipmentList)
        {
            allEquipemnt.Add(equipmentInfo.enName);
            equipmentInfoMap.Add(equipmentInfo.enName, equipmentInfo);
        }
        allEquipemnt.ForEach(i => equipmentList.Add(i));
    }

    private void LevelComplete()
    {
       
    }

    public Room GetRoomById(int id)
    {
        return rooms.FirstOrDefault(room => room.GetRoomId() == id);
    }
    
    public void NextRoom(float direction, int nextRoomId)
    {
        UIManager.GetInstance().EndScene(() =>
        {
            var nextRoom = GetRoomById(nextRoomId).GetComponent<Room>();
            UIManager.GetInstance().ChangeMiniMap(currentRoom, false);
            currentRoom = nextRoom;
            UIManager.GetInstance().ChangeMiniMap(nextRoom);
            nextRoom.BeginRoom();
            UIManager.GetInstance().StartScene();
            player.transform.Translate( 10f * direction * Vector3.left);
        });
    }

    private void Reset()
    {
        player = null;
        equipmentList.Clear();
        allEquipemnt.ForEach(i => equipmentList.Add(i));
        rooms.Clear();
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

    public void RegisterRooms(Room room)
    {
        if (rooms.Contains(room)) return;
        rooms.Add(room);
        if (room.GetRoomId() == 0)
        {
            currentRoom = room;
            UIManager.GetInstance().ChangeMiniMap(room);
            room.BeginRoom();
            room.EnterRoom();
        }
    }
    
    public string RegisterEquipment(string originName)
    {
        lock (_lock)
        {
            if (originName.Equals("Random"))
            {
                if (equipmentList.Count > 0)
                {
                    var index = Random.Range(0, equipmentList.Count);
                    originName = equipmentList[index];
                    equipmentList.RemoveAt(index);
                }
                else originName = "Null";
            }
            else
            {
                int i;
                int count = equipmentList.Count;
                for (i = 0; i < equipmentList.Count; i++)
                {
                    if (equipmentList[i].Equals(originName))
                    {
                        equipmentList.RemoveAt(i);
                        break;
                    }
                }
                if (count == equipmentList.Count) originName = "Null";
            }
            return originName;
        }
    }
    
    public void GameOver()
    {
        UIManager.GetInstance().EndScene(() =>
        {
            Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public EquipmentInfo GetEquipmentInfo(string equipmentName)
    {
        return equipmentInfoMap[equipmentName];
    }

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            UIManager.GetInstance().menu.SetActive(true);
        }
    }
}

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
    private GameObject portal;
    // 记录未出现过的装备
    private List<string> equipmentList;
    // 记录所有装备
    private List<string> allEquipment;
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
        rooms = new List<Room>();
        equipmentList = new List<string>();
        allEquipment = new List<string>();
        equipmentInfoMap = new Dictionary<string, EquipmentInfo>();
        ReadTextAssets();
    }

    private void ReadTextAssets()
    {
        TextAsset text = Resources.Load<TextAsset>("TextAssets/Equipment");
        EquipmentInfoList equipmentInfoList = JsonUtility.FromJson<EquipmentInfoList>(text.text);
        foreach (var equipmentInfo in equipmentInfoList.equipmentList)
        {
            allEquipment.Add(equipmentInfo.enName);
            equipmentInfoMap.Add(equipmentInfo.enName, equipmentInfo);
        }
        allEquipment.ForEach(i => equipmentList.Add(i));
    }

    public void GameOver()
    {
        UIManager.GetInstance().EndScene(() =>
        {
            Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public void NextLevel()
    {
        player.GetComponent<PlayerAnimation>().EnterRoom();
        UIManager.GetInstance().EndScene(() =>
        {
            player = null;
            rooms.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
    
    private void LevelComplete()
    {
        portal.SetActive(true);
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
        allEquipment.ForEach(i => equipmentList.Add(i));
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

    public void RoomComplete(int id)
    {
        foreach (var room in rooms)
        {
            if (room.GetRoomId() > id) return;
        }
        LevelComplete();
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

    public void RegisterExitPortal(GameObject portal)
    {
        this.portal = portal;
        this.portal.SetActive(false);
    }

    public EquipmentInfo GetEquipmentInfo(string equipmentName)
    {
        return equipmentInfoMap[equipmentName];
    }

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyCode.Escape))
        {
            var menu = UIManager.GetInstance().menu;
            if (!menu.activeSelf)
            {
                Time.timeScale = 0;
                menu.GetComponent<RectTransform>().SetAsLastSibling();
                menu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                menu.SetActive(false);
            }
        }
    }
}

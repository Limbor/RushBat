using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Object = System.Object;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    private int currentLevel;
    
    private GameObject player;
    private GameObject portal;
    // 记录未出现过的装备
    private List<string> equipmentList;
    // 记录所有装备
    private List<string> allEquipment;
    private List<Room> rooms;
    private Room currentRoom;
    
    private Dictionary<string, EquipmentInfo> equipmentInfoMap;
    private Dictionary<string, Skill> skillInfoMap;
    private List<string> skillList;
        
    // 当前关卡出现的装备
    private List<Equipment> thisLevelEquipment;

    private static readonly object _lock = new object();
    
    public static readonly string GroundTrap = "GroundTrap";
    public static readonly string Enemy = "Enemy";
    public static readonly string FlyingTrap = "FlyingTrap";
    public static readonly string Environment = "Environment";
    
    protected override void Init()
    {
        currentLevel = 0;
        player = null;
        rooms = new List<Room>();
        equipmentList = new List<string>();
        allEquipment = new List<string>();
        thisLevelEquipment = new List<Equipment>();
        equipmentInfoMap = new Dictionary<string, EquipmentInfo>();
        skillInfoMap = new Dictionary<string, Skill>();
        skillList = new List<string>();
        ReadTextAssets();
        LoadGame();
    }

    private void ReadTextAssets()
    {
        // 所有装备信息
        var text = Resources.Load<TextAsset>("TextAssets/Equipment");
        EquipmentInfoList equipmentInfoList = JsonUtility.FromJson<EquipmentInfoList>(text.text);
        foreach (var equipmentInfo in equipmentInfoList.equipmentList)
        {
            allEquipment.Add(equipmentInfo.enName);
            equipmentInfoMap.Add(equipmentInfo.enName, equipmentInfo);
        }
        allEquipment.ForEach(i => equipmentList.Add(i));
        // 所有技能信息
        text = Resources.Load<TextAsset>("TextAssets/Skill");
        SkillList skillList = JsonUtility.FromJson<SkillList>(text.text);
        foreach (var skill in skillList.skillList)
        {
            this.skillList.Add(skill.enName);
            skillInfoMap.Add(skill.enName, skill);
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

    public void NextLevel()
    {
        player.GetComponent<PlayerAnimation>().EnterRoom();
        UIManager.GetInstance().EndScene(() =>
        {
            player = null;
            rooms.Clear();
            thisLevelEquipment.ForEach(item => equipmentList.Add(item.equipmentName));
            thisLevelEquipment.Clear();
            if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
            {
                currentLevel += 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                currentLevel = 0;
                SceneManager.LoadScene(0);
            }
            SaveGame();
        });
    }
    
    public void LevelComplete()
    {
        portal.SetActive(true);
    }

    public Room GetRoomById(int id)
    {
        var targetRoom = rooms.FirstOrDefault(room => room.GetRoomId() == id);
        if (targetRoom == null)
        {
            targetRoom = GameObject.Find("Room" + (id + 1)).GetComponent<Room>();
        }
        return targetRoom;
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

    public string RegisterEquipment(string originName, Equipment equipment)
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
            if(!originName.Equals("Null") && equipment != null) thisLevelEquipment.Add(equipment);
            return originName;
        }
    }

    public void DelEquipment(Equipment equipment)
    {
        if (thisLevelEquipment.Contains(equipment)) thisLevelEquipment.Remove(equipment);
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

    public Skill GetSkillInfo(string skillName)
    {
        return skillInfoMap[skillName];
    }

    public List<string> GetSkillList()
    {
        return skillList;
    }

    public void SaveGame()
    {
        var saveData = new SaveData(currentLevel);
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData");
        }
        var formatter = new BinaryFormatter();
        var file = File.Create(Application.persistentDataPath + "/SaveData/game.save");
        var json = JsonUtility.ToJson(saveData);
        formatter.Serialize(file, json);
        file.Close();
    }

    public void LoadGame()
    {
        if (!File.Exists(Application.persistentDataPath + "/SaveData/game.save"))
        {
            return;
        }
        var formatter = new BinaryFormatter();
        var file = File.Open(Application.persistentDataPath + "/SaveData/game.save", FileMode.Open);
        var json = (string)formatter.Deserialize(file);
        var saveData = JsonUtility.FromJson<SaveData>(json);
        file.Close();

        currentLevel = saveData.level;
        Player.GetInstance().SetPlayer(saveData.player);
        saveData.player.equipments.ForEach(item => RegisterEquipment(item, null));
        SceneManager.LoadScene(currentLevel);
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

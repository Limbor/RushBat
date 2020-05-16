using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Boundary")]
    public Vector2 leftBottom, rightTop;

    public int roomId;
    
    private bool roomComplete;
    private bool discovered;
    private List<GameObject> enemies = new List<GameObject>();
    private List<Door> doors = new List<Door>();

    private void Start()
    {
        GameManager.GetInstance().RegisterRooms(this);
    }

    public void BeginRoom()
    {
        if (!discovered)
        {
            discovered = true;
            var player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerProperty>();
            if (player.HaveEquipment("HealHorn"))
            {
                player.SetHealth(1);
            }
        }
        CameraController camera = Camera.main.GetComponent<CameraController>();
        camera.maxPos = rightTop;
        camera.minPos = leftBottom;
        camera.gameObject.transform.position = transform.position;
        if (enemies.Count == 0)
        {
            roomComplete = true;
        }
    }

    public void EnterRoom()
    {
        if (roomComplete) return;
        foreach (var door in doors)
        {
            door.Close();
        }
        foreach (var enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }

    private void RoomComplete()
    {
        roomComplete = true;
        OpenDoors();
        AudioManager.GetInstance().PlayDoorAudio();
    }
    
    public void RegisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy)) return;
        enemies.Add(enemy);
        if(roomId != 0) enemy.SetActive(false);
    }

    public void DelEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy)) return;
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            RoomComplete();
        }
    }

    public void RegisterDoor(Door door)
    {
        if (doors.Contains(door)) return;
        doors.Add(door);
    }

    public void OpenDoors()
    {
        foreach (var door in doors)
        {
            door.Open(false);
        }
    }
    
    // 是否是第一次进入房间
    public bool FirstEnterRoom()
    {
        return !discovered;
    }

    public int GetRoomId()
    {
        return roomId;
    }

    public bool IsRoomCompleted()
    {
        return roomComplete;
    }

    public List<Room> GetConnectedRoom()
    {
        return doors.Select(door => GameManager.GetInstance().GetRoomById(door.correspondRoomId)).ToList();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnterRoom();
        }
    }
}

using System;

[Serializable]
public class SaveData
{
    public Player player;
    public int level;

    public SaveData(int level)
    {
        player = Player.GetInstance();
        this.level = level;
    }
}
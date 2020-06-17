using System;
using System.Collections.Generic;

[Serializable]
public class EquipmentInfo
{
    public string enName;
    public string name;
    public string intro;
    public int price;
}

[Serializable]
public class EquipmentInfoList
{
    public List<EquipmentInfo> equipmentList;    
}

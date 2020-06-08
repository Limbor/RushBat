using System;
using System.Collections.Generic;


[Serializable]
public class SkillLevel
{
    public int level;
    public int price;
    public string intro;
}

[Serializable]
public class Skill
{
    public string name;
    public string enName;
    public List<SkillLevel> levelList;
}

[Serializable]
public class SkillList
{
    public List<Skill> skillList;
}

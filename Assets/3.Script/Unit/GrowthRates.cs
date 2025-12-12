using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GrowthRates
{
    public int MaxHpGrowth; // 레벨업 시, HP 올라갈 확률
    public int PowerGrowth; // 레벨업 시, 힘, 마력이 올라갈 확률ㅇ
    public int SkillGrowth; // 레벨업 시, Skill 올라갈 확률
    public int SpdGrowth;   // 레벨업 시, 스피드 올라갈 확률
    public int LuckGrowth;  // 레벨업 시, 운 올라갈 확률
    public int DefGrowth;   // 레벨업 시, 수비 올라갈 확률
    public int ResGrowth;   // 레벨업 시, 마방 올라갈 확률
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats
{

    public int MAXHP; // MaxHP
    public int Power; // 마력, 힘
    public int Skill; // 정확도
    public int Spd; // 스피드
    public int Luck; // 운
    public int Def; // 수비
    public int Res; // 마방
    public int Move; // 움직일 수 있는 거리
    public int Con; // 체격
    public int Aid; // 구출할 수 있는 캐릭터 체격

    public Stats Clone()
    {
        return new Stats()
        {
            MAXHP = this.MAXHP,
            Power = this.Power,
            Skill = this.Skill,
            Spd = this.Spd,
            Luck = this.Luck,
            Def = this.Def,
            Res = this.Res,
            Move = this.Move,
            Con = this.Con,
            Aid = this.Aid
        };
    }

}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum WeaponType
{
    Sword,
    Spear,
    Axe,
    Bow,
    Magic,
    BlackMagic,
    Staff,
    WhiteMagic,
    None
}

[Serializable]
public enum WeaponRank
{
    None,
    E,
    D,
    C,
    B,
    A,
    S
}

[Serializable]
public class WeaponRankData
{
    public WeaponType WeaponType; // 검, 창, 도끼, 활, 마법
    public string Rank; // 무기 랭크
    public int Exp; // 무기 경험치
}

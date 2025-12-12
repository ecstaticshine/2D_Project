using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Unit : MonoBehaviour
{
    public UnitJsonData jsonData;
    public UnitSaveData saveData;

    public int CurrentHP => saveData.currentHp;
}
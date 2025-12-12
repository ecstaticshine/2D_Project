using System;
using System.Collections.Generic;
[Serializable]
public class SaveData
{
    public int StageLevel; // 스테이지
    public int turnNumber; // 현재 턴
    public bool isPlayerTurn; // 지금 플레이어 턴인가?

    public List<UnitSaveData> CharacterData;
    public List<int> clearedStages; // 지금까지 클리어한 스테이지

    public Dictionary<string, bool> mapEvents;
}

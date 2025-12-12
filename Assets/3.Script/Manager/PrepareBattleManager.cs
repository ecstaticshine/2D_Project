using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PrepareBattleManager : MonoBehaviour
{
    private readonly int maxUnitCount = 11;
    private int autoSelected = 0;
    //캐릭터
    public void StartBattle()
    {
        var saveData = SaveManager.Instance.currentSaveData.CharacterData;

        int selectedUnitCount = saveData.Count(unit => unit.PosX >= 0 && unit.PosY >= 0);
        if (selectedUnitCount == 0)
        {
            foreach (var unitData in SaveManager.Instance.currentSaveData.CharacterData)
            {
                if (autoSelected < maxUnitCount)
                {
                    ++autoSelected;

                    unitData.PosX = 0;
                    unitData.PosY = 0;

                    //Debug.Log($"[초기출전] {unitData.unitId} 좌표: {unitData.PosX},{unitData.PosY}");

                }
                else
                {

                    unitData.PosX = -1;
                    unitData.PosY = -1;

                    //Debug.Log($"[초기비출전] {unitData.unitId} 좌표: {unitData.PosX},{unitData.PosY}");
                }
            }
        }
        
            SceneManager.LoadScene("Stage");
    }
}

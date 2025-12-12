using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    //싱글톤으로 만들어야 하는 이유는 세이브데이터를 준비화면에서 캐릭터 불러올 때랑,
    //게임화면에서 캐릭터가 죽거나 레벨업한 것을 저장을 할 수 있도록 하기 위해서
    public static SaveManager Instance;
    public SaveData currentSaveData;

    private static int currentSlot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    public void SelectSaveSlot(int index)
    {
        currentSlot = index;
    }

    public bool HasSaveData(int index)
    {
        //C:/Users/user/AppData/LocalLow/PalePurpleGames/2D_SRPG
        string saveDataPath = Path.Combine(Application.persistentDataPath, $"save{index}.json");
        bool isDataExist = File.Exists(saveDataPath);
        return isDataExist;
    }

    public int GetSaveSlotLevel(int index)
    {
        string path = GetSaveFilePath(index);
        if (!File.Exists(path))
        {
            return -1;
        }
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        return data.StageLevel;
    }

    public bool Load(int index)
    {
        string saveDataPath = Path.Combine(Application.persistentDataPath, $"save{index}.json");
        if (!File.Exists(saveDataPath))
        {
            Debug.Log("없어요");
            return false;
        }
        string json = File.ReadAllText(saveDataPath);
        currentSaveData = JsonUtility.FromJson<SaveData>(json);

        return true;
    }

    public void CreateSaveData(int index)
    {
        currentSlot = index;
        currentSaveData = new SaveData();

        currentSaveData.StageLevel = 16;
        currentSaveData.turnNumber = 0;
        currentSaveData.isPlayerTurn = true;

        currentSaveData.CharacterData = new List<UnitSaveData>();
        currentSaveData.clearedStages = new List<int>() {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};

        currentSaveData.mapEvents = new Dictionary<string, bool>();

        foreach(var unitId in StartUnits())
        {
            var json = LoadUnitJson(unitId);
            var saveUnit = CreateUnitSaveData(json);
            currentSaveData.CharacterData.Add(saveUnit);
        }

        Save();
    }

    public List<string> StartUnits()
    {
        List<string> list = new List<string>() { "Lyn", "Kent", "Sain", "Florina", "Wil", "Dorcas", "Erk", "Matthew", "Eliwood", "Marcus", "Lowen", "Rebecca", "Bartre", "Hector", "Oswin", "Guy", "Priscilla", "Serra" };


        return list;
    }  

    public UnitSaveData CreateUnitSaveData(UnitJsonData json)
    {
        UnitSaveData save = new UnitSaveData();
        save.unitId = json.UnitId;
        save.currentClassId = json.BaseClassId;
        save.unitName = json.UnitName;
        save.isAlive = true;
        save.level = json.Level;
        save.exp = 0;
        save.currentStats = json.BaseStats.Clone();
        save.currentHp = json.BaseStats.MAXHP;
        save.PosX = -1; // 참전 안한 상태로  참전하면 좌표 전달
        save.PosY = -1; // 참전 안한 상태로  참전하면 좌표 전달
        save.Inventory = new List<Item>();
        if (json.WeaponRanks != null)
        {
            foreach (var Item in json.Inventory)
            {
                save.Inventory.Add(new Item()
                {
                    itemId = Item.itemId,
                    Durability = Item.Durability,
                });
            }
        }
        save.weaponRanks = new List<WeaponRankData>();
        if (json.WeaponRanks != null)
        {
            foreach(var weaponRank in json.WeaponRanks)
            {
                save.weaponRanks.Add(new WeaponRankData()
                {
                    WeaponType = weaponRank.WeaponType,
                    Rank = weaponRank.Rank,
                    Exp = weaponRank.Exp
                });
            }
        }
        return save;
    }

    public void Save()
    {
        if(currentSaveData == null)
        {
            Debug.Log("저장할 SaveData가 없습니다.");
            return;
        }

        string json = JsonUtility.ToJson(currentSaveData, true);
        string path = GetSaveFilePath(currentSlot);

        File.WriteAllText(path, json);

        Debug.Log($"세이브 완료! Slot: {currentSlot}, Path: {path}");
    }

    public string GetSaveFilePath(int index)
    {
        return Path.Combine(Application.persistentDataPath, $"save{index}.json");
    }

    // 유닛 JSON 파일 읽기
    public UnitJsonData LoadUnitJson(string unitId)
    {
        string path = $"Json/Units/{unitId}";
        TextAsset textAsset = Resources.Load<TextAsset>(path);

        if(textAsset == null)
        {
            Debug.Log("No JSON");
            return null;
        }
        return JsonUtility.FromJson<UnitJsonData>(textAsset.text);
    }


}

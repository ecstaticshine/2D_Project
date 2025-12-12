using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotManager : MonoBehaviour
{
    [SerializeField]
    private Button[] SaveSlotList;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        for (int i = 0; i < SaveSlotList.Length; i++)
        {
            int slotIndex = i;
            SaveSlotList[i].onClick.AddListener(() => OnSlotClicked(slotIndex));

        }
        Debug.Log(Application.persistentDataPath);

        ShowSlots();
    }


    private void OnSlotClicked(int index)
    {
        SaveManager.Instance.SelectSaveSlot(index);

        if (SaveManager.Instance.HasSaveData(index))
        {
            SaveManager.Instance.Load(index);
        }
        else
        {
            SaveManager.Instance.CreateSaveData(index);
        }
        SceneManager.LoadScene("PrepareStage");
    }


    // 세이브 슬롯의 내용을 표시.
    private void ShowSlots()
    {
        for (int i = 0; i< SaveSlotList.Length; i++)
        {
            Text text = SaveSlotList[i].GetComponentInChildren<Text>();

            if (SaveManager.Instance.HasSaveData(i))
            {
                int stage = SaveManager.Instance.GetSaveSlotLevel(i);
                text.text = $"스테이지 {stage}";
            }
            else
            {
                text.text = $"- 빈 슬롯 -";
            }
        }
    }
}

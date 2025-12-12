using UnityEngine;
using UnityEngine.UI;

public class UnitPrefab : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Button button;

    private UnitWarData unitWarData;
    private UnitListManager manager;

    private void Start()
    {

        if (unitWarData.isSelected)
        {
            portraitImage.color = Color.white;
        }
        else
        {
            portraitImage.color = Color.gray;
            unitWarData.isSelected = false;
        }

    }

    public void SetUp(Sprite portrait, string characterName, UnitWarData data, UnitListManager unitListManager)
    {
        this.unitWarData = data;
        this.manager = unitListManager;

        portraitImage.sprite = portrait;
        nameText.text = characterName;
        button.onClick.AddListener(OnClick);

        Refresh();
    }

    void OnClick()
    {
        manager.CheckUnitSelected(unitWarData);
    }

    public void Refresh()
    {
        if (unitWarData.isSelected)
        {
            portraitImage.color = Color.white;
        }
        else
        {
            portraitImage.color = Color.gray;
        }
    }
}

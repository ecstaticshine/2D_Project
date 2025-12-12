using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.EventSystems;

public enum MapStyle
{
    Flat = 6,           //평지 설정한 레이어에 맞게 6부터 시작 
    Floor,              //바닥
    Wall,               //벽
    BreakableWall,       //부서질 수 있는 벽
    Stair,              //계단
    Pillar,             //기둥 -> 수비 + 1, 회피 + 20
    Throne,             //왕좌 -> 수비 + 3, 마방 + 5, 회피 + 30, HP + 10프로 회복
    Door,               //열어야 할 문
    TreasureChest,      //보물상자
}

public class MapTile
{
    public bool isWall;
    public bool isOpened = false;
    public int X, Y;    //좌표
    public int DEF, RES, AVOID, RECOVERY;  // 방어력, 마법방어력, 회피력, 회복 계산
    public MapStyle mapStyle;

    public MapTile(MapStyle mapStyle, bool isWall, int x, int y)
    {
        this.mapStyle = mapStyle;
        this.isWall = isWall;
        this.X = x;
        this.Y = y;
        this.DEF = 0;
        this.RES = 0;
        this.AVOID = 0;
        this.RECOVERY = 0;
    }
    public MapTile(MapStyle mapStyle, bool isWall, int x, int y, int DEF, int MDEF, int AVOID, int RECOVERY) : this(mapStyle, isWall, x, y)
    {
        this.DEF = DEF;
        this.RES = MDEF;
        this.AVOID = AVOID;
        this.RECOVERY = RECOVERY;
    }
}
public class GameManager : MonoBehaviour
{
    public Tilemap tiles;

    private Vector3 initalScale;

    private TileData[,] mapTileArray; // 좌표 사용을 위한 2차원 배열

    [SerializeField] private GameObject cursor;
    [SerializeField]
    private TMP_Text tileType, defValue, avoidValue;

    [SerializeField]
    private Vector2Int bottomLeft, topRight, endPos;

    public GameObject StartPos_ob, EndPos_ob; //좌표를 가지고 오기 위한 오브젝트
    public GameObject BottomLeft_ob, TopRight_ob;//좌표를 가지고 오기 위한 오브젝트

    [SerializeField]
    private GameObject TilePanel, CharacterPanel, PurposePanel;
    [SerializeField]
    private GameObject TilePanelMove, CharacterPanelMove, PurposePanelMove, Temp;


    [SerializeField]
    private TMP_Text CharacterName, CharacterHp, CharacterMaxHp;
    [SerializeField]
    private Slider CharacterHpSlider;
    [SerializeField]
    Image CharacterMugShot;

    private float scrollSpeed = 200f;

    private bool isUIMoved = false;

    RaycastHit2D hit;

    private void Awake()
    {
        SetPosition();
        
    }

    private void Start()
    {
        mapTileArray = GridManager.Instance.GetTiles();
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x > bottomLeft.x && mousePos.x < topRight.x && mousePos.y > bottomLeft.y && mousePos.y < topRight.y)
        {
            getTileInfo(mousePos);
            pointOverUIElement();

        }


        if (Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.transform.position.y < 28f)
        {
            Camera.main.transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.transform.position.y > 10f)
        {
            Camera.main.transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
        }

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    mousePos += Vector3.up;
        //    Debug.Log(mousePos);
        //}
        hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                CharacterPanel.GetComponent<Image>().color = Color.aliceBlue;
                UnitSaveData unit =  hit.collider.GetComponent<UnitController>().data;
                CharacterName.text = unit.unitName;
                CharacterHp.text = $"{unit.currentHp}";
                CharacterMaxHp.text = $"{unit.currentStats.MAXHP}";
                CharacterHpSlider.value = (float)unit.currentHp / unit.currentStats.MAXHP;
                CharacterMugShot.sprite = LoadPortrait(unit.unitId);
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                CharacterPanel.GetComponent<Image>().color = Color.indianRed;
                EnemyUnitController enemy = hit.collider.GetComponent<EnemyUnitController>();
                CharacterName.text = enemy.unitName;
                CharacterHp.text = $"{enemy.currentHp}";
                CharacterMaxHp.text = $"{enemy.maxHp}";
                CharacterHpSlider.value = (float)enemy.currentHp / enemy.currentStats.MAXHP;
                CharacterMugShot.sprite = LoadPortrait(enemy.unitId);


            }
        }

    }
    // UI 움직이기
    private void pointOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (var result in raycastResults)
        {
            if (result.gameObject.name.Equals("Border") || result.gameObject.name.Equals("Border2"))
            {
                if (isUIMoved)
                {
                    ResetPosition();
                    isUIMoved = false;
                }
            }
            else
            {
                switch (result.gameObject.name)
                {
                    case "CharacterPanelMove":
                        CharacterPanel.transform.SetParent(TilePanelMove.transform);
                        CharacterPanel.GetComponent<RectTransform>().pivot = Vector2.zero;
                        CharacterPanel.GetComponent<RectTransform>().localPosition = Vector2.zero;
                        TilePanel.transform.SetParent(Temp.transform);
                        TilePanel.GetComponent<RectTransform>().pivot = Vector2.zero;
                        TilePanel.GetComponent<RectTransform>().localPosition = new Vector3(-10f, -70f, 0);
                        isUIMoved = true;
                        break;
                    case "TilePanelMove":
                        TilePanel.transform.SetParent(Temp.transform);
                        TilePanel.GetComponent<RectTransform>().localPosition = new Vector3(145f, 80f, 0);
                        isUIMoved = true;
                        break;
                    case "PurposePanelMove":
                        PurposePanel.transform.SetParent(Temp.transform);
                        PurposePanel.GetComponent<RectTransform>().localPosition = Vector3.zero;
                        isUIMoved = true;
                        break;
                }
            }
        }
    }
    // UI 원래대로 돌리기
    private void ResetPosition()
    {
        CharacterPanel.transform.SetParent(CharacterPanelMove.transform);
        CharacterPanel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        CharacterPanel.GetComponent<RectTransform>().localPosition = new Vector3(300f,-125f,0);
        TilePanel.transform.SetParent(TilePanelMove.transform);
        TilePanel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        TilePanel.GetComponent<RectTransform>().localPosition = new Vector3(150f,155f,0);
        PurposePanel.transform.SetParent(PurposePanelMove.transform);
        PurposePanel.GetComponent<RectTransform>().localPosition = new Vector3(-300f, -75f, 0);
    }


    private void getTileInfo(Vector3 mousePos)
    {
        Vector3Int cell = GridManager.Instance.Map.WorldToCell(mousePos);
        Vector2Int mousePosition = new Vector2Int(cell.x, cell.y);

        cursor.transform.position = GridManager.Instance.Map.CellToWorld(cell) + new Vector3(0.5f, 0.5f, 0);

        if (mousePosition.x - bottomLeft.x >= mapTileArray.GetLength(0) ||
            mousePosition.x - bottomLeft.x < 0 ||
            mousePosition.y - bottomLeft.y >= mapTileArray.GetLength(1) ||
            mousePosition.y - bottomLeft.y < 0)
            return;

        TileType targetTile = GridManager.Instance.GetTileType(mousePosition);
        switch (targetTile)
        {
            case TileType.Flat:
                tileType.text = "평지";
                break;
            case TileType.Floor:
                tileType.text = "바닥";
                break;
            case TileType.Wall:
                tileType.text = "벽";
                break;
            case TileType.BreakableWall:
                tileType.text = "벽(파)";
                break;
            case TileType.Stair:
                tileType.text = "계단";
                break;
            case TileType.Pillar:
                tileType.text = "기둥";
                break;
            case TileType.Throne:
                tileType.text = "왕좌";
                break;
            case TileType.Door:
                tileType.text = "문";
                break;
            case TileType.TreasureChest:
                tileType.text = "상자";
                break;
            default:
                tileType.text = "--";
                break;
        }
        defValue.text = GridManager.Instance.GetDef(mousePosition).ToString();
        avoidValue.text = GridManager.Instance.GetAvoid(mousePosition).ToString();
    }

    private void SetPosition()
    {
        bottomLeft = new Vector2Int((int)BottomLeft_ob.transform.position.x, (int)BottomLeft_ob.transform.position.y);
        topRight = new Vector2Int((int)TopRight_ob.transform.position.x, (int)TopRight_ob.transform.position.y);
    }

    private Sprite LoadPortrait(string unitId)
    {
        var sprite = Resources.Load<Sprite>($"Portraits/{unitId}");

        return sprite;

    }
}

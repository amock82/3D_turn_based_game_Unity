using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    public GameObject[] _targetObjects;
    public int targetNum;

    public GameObject _targetPosition;

    public Material[] _moveMaterial;
    public MeshRenderer[] _moveTile;
    public Tile[] _tile;

    private RaycastHit hit = new RaycastHit();
    private Ray ray;

    LineRenderer _line;

    public bool isMove = false;

    public int actionable;

    public static TargetManager instance;

    private void Awake()
    {
        instance = this;

        _line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _targetObjects = new GameObject[GameObject.Find("Pawn").transform.childCount];
        _moveTile = new MeshRenderer[GameObject.Find("GridGroup").transform.childCount];
        _tile = new Tile[GameObject.Find("GridGroup").transform.childCount];

        for (int i = 0; i < _targetObjects.Length; i++)
        {   
            _targetObjects[i] = GameObject.Find("Pawn").transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < _tile.Length; i++)
        {
            _tile[i] = GameObject.Find("GridGroup").transform.GetChild(i).GetComponent<Tile>();
        }

        for (int i = 0; i < _moveTile.Length; i++)
        {
            _moveTile[i] = GameObject.Find("GridGroup").transform.GetChild(i).GetChild(0).GetComponent<MeshRenderer>();
        }

        actionable = _targetObjects.Length;

        CameraTarget.instance.SetCameraTarget(_targetObjects[0]);
        targetNum = 0;
    }

    private void Update()
    {
        //Debug.Log(actionable);

        DrawTile();

        if (TurnManager.instance.turn == TurnManager.Turn.player)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);        

            if (Physics.Raycast(ray, out hit) && isMove == false)
            {               

                if (hit.transform.tag == "Ground" && hit.transform.gameObject.GetComponent<Tile>().isMoveable == true)
                {
                    _line.positionCount = 5;

                    _line.SetWidth(0.1f, 0.1f);

                    //if ((hit.transform.position - _targetObjects[targetNum].transform.position).magnitude <= _targetObjects[targetNum].GetComponent<Pawn>().GetAction() * 5)
                    if (hit.transform.GetChild(0).GetComponent<MeshRenderer>().material.color == _moveMaterial[1].color ||
                        hit.transform.GetChild(0).GetComponent<MeshRenderer>().material.color == _moveMaterial[2].color)
                    {
                        //Debug.Log(hit.transform.name);    // 마우스 커서 위치의 타일을 로그메세지

                        _targetPosition = hit.transform.gameObject;

                        _line.startColor = new Color(1, 1, 0);
                        _line.endColor = new Color(1, 1, 0);

                        _line.SetPosition(0, new Vector3(hit.transform.position.x - 1, 0.7f, hit.transform.position.z - 1));
                        _line.SetPosition(1, new Vector3(hit.transform.position.x + 1, 0.7f, hit.transform.position.z - 1));
                        _line.SetPosition(2, new Vector3(hit.transform.position.x + 1, 0.7f, hit.transform.position.z + 1));
                        _line.SetPosition(3, new Vector3(hit.transform.position.x - 1, 0.7f, hit.transform.position.z + 1));
                        _line.SetPosition(4, new Vector3(hit.transform.position.x - 1, 0.7f, hit.transform.position.z - 1));
                    }
                    //if ((hit.transform.position - _targetObjects[targetNum].transform.position).magnitude <= (_targetObjects[targetNum].GetComponent<Pawn>().GetAction() - 1) * 5)
                    
                    if (hit.transform.GetChild(0).GetComponent<MeshRenderer>().material.color == _moveMaterial[2].color)
                    {
                        _line.startColor = new Color(0.3125f, 1, 1);
                        _line.endColor = new Color(0.3125f, 1, 1);
                    }
                }
            }
        }

        if (actionable == 0 && TurnManager.instance.turn == TurnManager.Turn.player)
        {
            TurnManager.instance.NextTurn();
        }

        if (TurnManager.instance.turn == TurnManager.Turn.enemy)
        {
            Debug.Log("적턴 스킵");
            TurnManager.instance.NextTurn();
        }
    }

    public void NextTarget()
    {
        for (int i = 0; i < _targetObjects.Length; i++)
        {
            targetNum++;

            if (targetNum >= _targetObjects.Length)
            {
                targetNum = 0;
            }

            if (_targetObjects[targetNum].GetComponent<Pawn>().GetAction() > 0)
                break;
        }

        CameraTarget.instance.SetCameraTarget(_targetObjects[targetNum]);
        _line.positionCount = 0;

        isMove = false;
    }

    public void PreviousTarget()
    {
        for (int i = 0; i < _targetObjects.Length; i++)
        {
            targetNum--;

            if (targetNum < 0)
            {
                targetNum = _targetObjects.Length - 1;
            }

            if (_targetObjects[targetNum].GetComponent<Pawn>().GetAction() > 0)
                break;
        }

        CameraTarget.instance.SetCameraTarget(_targetObjects[targetNum]);
        _line.positionCount = 0;

        isMove = false;
    }

    public void ClickTarget(GameObject target)
    {
        for(int i = 0; i < _targetObjects.Length; i++)
        {
            if (_targetObjects[i] == target)
            {
                targetNum = i;
                CameraTarget.instance.SetCameraTarget(_targetObjects[targetNum]);

                break;
            }
        }

        _line.positionCount = 0;

        isMove = false;
    }

    public void TargetMove(GameObject targetObject)
    {
        if (targetObject == _targetPosition)
        {
            _targetObjects[targetNum].GetComponent<Pawn>().Move(targetObject);

            isMove = true;

            _targetObjects[targetNum].GetComponent<Pawn>().UseAction(targetObject.GetComponent<Tile>().GetUseAction());
        }
    }

    public void DrawTile()
    {
        Vector3 targetPos = _targetObjects[targetNum].transform.position;
        Vector3 tilePos;

        GameObject grid = GameObject.Find("GridGroup");

        int gridX = (int)grid.GetComponent<RectTransform>().rect.width / (int)grid.GetComponent<GridLayoutGroup>().cellSize.x;
        int gridZ = (int)grid.GetComponent<RectTransform>().rect.height / (int)grid.GetComponent<GridLayoutGroup>().cellSize.y;

        //Debug.Log(gridX);
        //Debug.Log(gridZ);

        for (int i = 0; i < _moveTile.Length; i++)
        {
            _moveTile[i].material = _moveMaterial[0];       // 이동불가지역
        }

        MoveManager.instance.CalcMovable(_targetObjects[targetNum]);


        // 이하 코드는 장애물을 가정하지 않은 이동거리 측정
        //targetPos.y = 0;

        //for (int i = 0; i < _moveTile.Length; i++)
        //{
        //    tilePos = _moveTile[i].transform.position;
        //    tilePos.y = 0;

        //    _moveTile[i].material = _moveMaterial[0];       // 이동불가지역

        //    if (isMove == false && _tile[i].isMoveable == true)
        //    {
        //        if (_targetObjects[targetNum].GetComponent<Pawn>().GetAction() == 2)
        //        {
        //            if ((tilePos - targetPos).magnitude <= 5.1)
        //            {
        //                _moveTile[i].material = _moveMaterial[2];   // 이동가능지역 (행동가능)
        //                _tile[i].GetComponent<Tile>().SetUseAction(1);
        //            }
        //            else if ((tilePos - targetPos).magnitude <= 10.1)
        //            {
        //                _moveTile[i].material = _moveMaterial[1];   // 이동가능지역 (행동불가)
        //                _tile[i].GetComponent<Tile>().SetUseAction(2);
        //            }
        //        }
        //        else if (_targetObjects[targetNum].GetComponent<Pawn>().GetAction() == 1)
        //        {
        //            if ((_moveTile[i].transform.position - _targetObjects[targetNum].transform.position).magnitude <= 5)
        //            {
        //                _moveTile[i].material = _moveMaterial[1];   // 이동가능지역 (행동불가)
        //                _tile[i].GetComponent<Tile>().SetUseAction(1);
        //            }
        //        }
        //    }

        //}
    }

    public void PawnActionReset()
    {
        for (int i = 0; i < _targetObjects.Length; i++)
        {
            _targetObjects[i].GetComponent<Pawn>().ActionReset();
        }

        actionable = _targetObjects.Length;
    }
}

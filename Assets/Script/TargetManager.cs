using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject[] _targetObjects;
    public int targetNum;

    public GameObject _targetPosition;

    public Material[] _moveMaterial;
    public MeshRenderer[] _moveTile;

    private RaycastHit hit = new RaycastHit();
    private Ray ray;

    LineRenderer _line;

    public bool isMove = false;

    public static TargetManager instance;

    private void Awake()
    {
        instance = this;

        _line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _targetObjects = new GameObject[GameObject.Find("Pawn").transform.childCount];
        _moveTile = new MeshRenderer[GameObject.Find("GridGroupMoveable").transform.childCount];

        for (int i = 0; i < _targetObjects.Length; i++)
        {   
            _targetObjects[i] = GameObject.Find("Pawn").transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < _moveTile.Length; i++)
        {
            _moveTile[i] = GameObject.Find("GridGroupMoveable").transform.GetChild(i).GetComponent<MeshRenderer>();
        }

        CameraTarget.instance.SetCameraTarget(_targetObjects[0]);
        targetNum = 0;
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        DrawTile();

        if (Physics.Raycast(ray, out hit) && isMove == false)
        {
            if (hit.transform.tag == "Ground")
            {
                _line.positionCount = 5;

                _line.SetWidth(0.1f, 0.1f);

                if ((hit.transform.position - _targetObjects[targetNum].transform.position).magnitude <= 10)
                {
                    _targetPosition = hit.transform.gameObject;

                    _line.startColor = new Color(1, 1, 0);
                    _line.endColor = new Color(1, 1, 0);

                    _line.SetPosition(0, new Vector3(hit.transform.position.x - 1, 0.7f, hit.transform.position.z - 1));
                    _line.SetPosition(1, new Vector3(hit.transform.position.x + 1, 0.7f, hit.transform.position.z - 1));
                    _line.SetPosition(2, new Vector3(hit.transform.position.x + 1, 0.7f, hit.transform.position.z + 1));
                    _line.SetPosition(3, new Vector3(hit.transform.position.x - 1, 0.7f, hit.transform.position.z + 1));
                    _line.SetPosition(4, new Vector3(hit.transform.position.x - 1, 0.7f, hit.transform.position.z - 1));
                }
                if ((hit.transform.position - _targetObjects[targetNum].transform.position).magnitude <= 5)
                {
                    _line.startColor = new Color(0.3125f, 1, 1);
                    _line.endColor = new Color(0.3125f, 1, 1);
                }
            }
        }
    }

    public void NextTarget()
    {
        targetNum++;

        if (targetNum >= _targetObjects.Length)
        {
            targetNum = 0;
        }

        CameraTarget.instance.SetCameraTarget(_targetObjects[targetNum]);

        isMove = false;
    }

    public void PreviousTarget()
    {
        targetNum--;

        if (targetNum < 0)
        {
            targetNum = _targetObjects.Length - 1;
        }

        CameraTarget.instance.SetCameraTarget(_targetObjects[targetNum]);

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

        isMove = false;
    }

    public void TargetMove(GameObject targetObject)
    {
        if (targetObject == _targetPosition)
        {
            _targetObjects[targetNum].GetComponent<Pawn>().move(targetObject);

            isMove = true;
        }
    }

    public void DrawTile()
    {
        for(int i = 0; i< _moveTile.Length; i++)
        {
            _moveTile[i].material = _moveMaterial[0];       // 이동불가지역

            if (isMove == false)
            {
                if ((_moveTile[i].transform.position - _targetObjects[targetNum].transform.position).magnitude <= 5)
                {
                    _moveTile[i].material = _moveMaterial[2];   // 이동가능지역 (행동가능)
                }
                else if ((_moveTile[i].transform.position - _targetObjects[targetNum].transform.position).magnitude <= 10)
                {
                    _moveTile[i].material = _moveMaterial[1];   // 이동가능지역 (행동불가)
                }
            }
        }      
    }
}

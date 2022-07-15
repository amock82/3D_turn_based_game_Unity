using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveManager : MonoBehaviour
{
    public Tile[] _tile;
    public MeshRenderer[] _moveTile;
    public Material[] _moveMaterial;
    public bool isMove;
    public int gridX;
    public int gridY;
    float[] minDist;

    public static MoveManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gridX = (int)GridTileManager.instance.x;
        gridY = (int)GridTileManager.instance.y;
        minDist = new float[gridX * gridY];
    }

    private void Update()
    {
        
    }

    public void CalcMovable(GameObject pawn)
    {
        Pawn targetPawn = pawn.GetComponent<Pawn>();
        int i;
        float mob = targetPawn.GetMobility();

        _tile = TargetManager.instance._tile;     
        _moveTile = TargetManager.instance._moveTile;
        _moveMaterial = TargetManager.instance._moveMaterial;
        isMove = TargetManager.instance.isMove;

        for(i = 0; i < gridX * gridY; i++)
        {
            if(_tile[i] == targetPawn.GetTile())
            {
                break;
            }
        }

        for (int j = 0; j < gridX * gridY; j ++)
        {
            minDist[j] = -1;
        }

        if (isMove == false)
        {
            CheckCost(mob * targetPawn.GetAction(), 0, i, mob);
        }     
    }

    public void CheckCost(float mobility, float dist, int _tileNum, float pawnMob)
    {
        float remainingDistance = mobility - dist;

        if ( 0 <= _tileNum && _tileNum < gridX * gridY)
        {
            if (minDist[_tileNum] < remainingDistance)
            {
                minDist[_tileNum] = remainingDistance;
            }
            else
            {
                return;
            }
        }
        else 
        {
            return;
        }

        if(remainingDistance < 0)
        {
            return;
        }

        if (dist != 0  && _tile[_tileNum].isMoveable == false)
        {
            return;
        }    

        if (remainingDistance > pawnMob)
        {
            _moveTile[_tileNum].material = _moveMaterial[2];        // 이동 후 행동가능 지역
            _tile[_tileNum].GetComponent<Tile>().SetUseAction(1);   // 해당 타일 이동시 행동력 소모 1로 변경
        }
        else if (remainingDistance < pawnMob)
        {
            _moveTile[_tileNum].material = _moveMaterial[1];        // 이동 후 행동불가 지역
            _tile[_tileNum].GetComponent<Tile>().SetUseAction(2);   // 해당 타일 이동시 행동력 소모 2로 변경
        }

        if (remainingDistance == pawnMob * 2)
        {
            _moveTile[_tileNum].material = _moveMaterial[0];
        }

        if (_tileNum % gridX != 0 && _tileNum % gridX != 9)
        {
            CheckCost(remainingDistance, 2, _tileNum - 1, pawnMob);
            CheckCost(remainingDistance, 2, _tileNum + 1, pawnMob);
            CheckCost(remainingDistance, 2, _tileNum - gridY, pawnMob);
            CheckCost(remainingDistance, 2, _tileNum + gridY, pawnMob);

            CheckCost(remainingDistance, 2 * Mathf.Sqrt(2), _tileNum - gridY - 1, pawnMob);
            CheckCost(remainingDistance, 2 * Mathf.Sqrt(2), _tileNum - gridY + 1, pawnMob);
            CheckCost(remainingDistance, 2 * Mathf.Sqrt(2), _tileNum + gridY - 1, pawnMob);
            CheckCost(remainingDistance, 2 * Mathf.Sqrt(2), _tileNum + gridY + 1, pawnMob);
        }
        else if (_tileNum % gridX == 0)
        {
            CheckCost(remainingDistance, 2, _tileNum + 1, pawnMob);
            CheckCost(remainingDistance, 2, _tileNum - gridY, pawnMob);
            CheckCost(remainingDistance, 2, _tileNum + gridY, pawnMob);

            CheckCost(remainingDistance, 2 * Mathf.Sqrt(2), _tileNum - gridY + 1, pawnMob);
            CheckCost(remainingDistance, 2 * Mathf.Sqrt(2), _tileNum + gridY + 1, pawnMob);
        }
        else if (_tileNum % gridX == 9)
        {
            CheckCost(remainingDistance, 2, _tileNum - 1, pawnMob);
            CheckCost(remainingDistance, 2, _tileNum - gridY, pawnMob);
            CheckCost(remainingDistance, 2, _tileNum + gridY, pawnMob);

            CheckCost(remainingDistance, 2 * Mathf.Sqrt(2), _tileNum - gridY - 1, pawnMob);
            CheckCost(remainingDistance, 2 * Mathf.Sqrt(2), _tileNum + gridY - 1, pawnMob);
        }
        
    }
}

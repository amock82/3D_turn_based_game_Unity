using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pawn : MonoBehaviour
{
    int maxHp = 5;
    int curHp = 5;

    float mobility = 5;
    int action = 2;

    Transform _targetPoint;

    bool isMoving = false;
    bool isActionable = true;

    LineRenderer _line;
    Tile _tile;

    NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isMoving == true)
        {
            Vector3 dir = _targetPoint.position - transform.position;

            dir.y = 0;

            if (dir.magnitude >= 1)
                dir.Normalize();            

            //transform.position += dir * Time.deltaTime * 10; 

            if (TargetManager.instance._targetObjects[TargetManager.instance.targetNum] == gameObject)
            {
                CameraTarget.instance.transform.position = transform.position;
            }

            if (dir.magnitude  <= 0.005f)
            {
                if (TargetManager.instance._targetObjects[TargetManager.instance.targetNum] == gameObject)
                {
                    TargetManager.instance.isMove = false;

                    if (action == 0)
                    {
                        TargetManager.instance.NextTarget();
                    }
                }

                isMoving = false;
            }
        }

        if(action <= 0)
        {
            EndOfAction();
        }
    }

    // 원하는 오브젝트(타일) 위치로 이동
    public void Move(GameObject targetObject)
    {
        _targetPoint = targetObject.transform;
     
        _agent.SetDestination(targetObject.transform.position);  

        isMoving = true;
    }

    public void EndOfAction()
    {
        if (isActionable == true)
        {
            isActionable = false;
            TargetManager.instance.actionable--;
        }
    }

    public void ActionReset()
    {
        action = 2;

        isActionable = true;
    }

    public void LineDraw()
    {
        
    }

    public int GetAction()
    {
        return action;
    }

    public void UseAction(int used)
    {
        action -= used;

        if (action < 0)
        {
            action = 0;
        }
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public int GetCurHp()
    {
        return curHp;
    }

    public float GetMobility()
    {
        return mobility;
    }

    public void SetTile(Tile _underTile)
    {
        _tile = _underTile;
    }

    public Tile GetTile()
    {
        return _tile;
    }
}

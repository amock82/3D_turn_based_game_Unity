using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    int mobility = 3;

    Transform _targetPoint;

    bool isMoving = false;

    LineRenderer line;

    private void Update()
    {
        if (isMoving == true)
        {
            Vector3 dir = _targetPoint.position - transform.position;

            dir.y = 0;

            if (dir.magnitude >= 1)
                dir.Normalize();            

            transform.position += dir * Time.deltaTime * 10;
            if (TargetManager.instance._targetObjects[TargetManager.instance.targetNum] == gameObject)
            {
                CameraTarget.instance.transform.position = transform.position;
            }

            if (dir.magnitude  <= 0.001f)
            {
                if (TargetManager.instance._targetObjects[TargetManager.instance.targetNum] == gameObject)
                {
                    TargetManager.instance.isMove = false;
                }

                isMoving = false;
            }
        }
    }

    // 원하는 오브젝트(타일) 위치로 이동
    public void move(GameObject targetObject)
    {
        _targetPoint = targetObject.transform;

        isMoving = true;
    }

    public void LineDraw()
    {
        
    }
}

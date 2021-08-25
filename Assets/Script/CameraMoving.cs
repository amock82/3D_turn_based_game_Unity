using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    bool up, down, left, right;

    private void Start()
    {
        up = false;
        down = false;
        left = false;
        right = false;
    }

    private void Update()
    {
        if (up || down || left || right)
        {
            CameraTarget.instance.CameraMove(up, down, left, right);
        }
    }

    public void CameraUp()
    {
        up = true;
    }

    public void CameraUpCancel()
    {
        up = false;
    }

    public void CameraDown()
    {
        down = true;
    }

    public void CameraDownCancel()
    {
        down = false;
    }

    public void CameraLeft()
    {
        left = true;
    }

    public void CameraLeftCancel()
    {
        left = false;
    }

    public void CameraRight()
    {
        right = true;
    }

    public void CameraRightCancel()
    {
        right = false;
    }
}

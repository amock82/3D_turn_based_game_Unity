using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    bool up, down, left, right;
    float zoom;
    float maxZoom;
    float zoomPower;
    Transform _cameraTransform;


    private void Start()
    {
        up = false;
        down = false;
        left = false;
        right = false;

        zoom = 0;
        maxZoom = 1.5f;
        zoomPower = 10;
        _cameraTransform = GameObject.Find("Main Camera").transform;
    }

    private void LateUpdate()
    {
        if (up || down || left || right)
        {
            CameraTarget.instance.CameraMove(up, down, left, right);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom < maxZoom)
            {
                _cameraTransform.position += Vector3.Normalize(_cameraTransform.forward) * Input.GetAxis("Mouse ScrollWheel") * zoomPower;
                zoom += Input.GetAxis("Mouse ScrollWheel");
                
                if (zoom > maxZoom)
                    zoom = maxZoom;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom > 0)
            {
                _cameraTransform.position += Vector3.Normalize(_cameraTransform.forward) * Input.GetAxis("Mouse ScrollWheel") * zoomPower;
                zoom += Input.GetAxis("Mouse ScrollWheel");

                if (zoom < 0)
                    zoom = 0;
            }
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

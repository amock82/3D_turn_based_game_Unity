using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    GameObject _cameraTarget;

    float targetTimer;
    float rotateTimer;

    float cameraSpeed;

    int rot = 0;

    public static CameraTarget instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        targetTimer -= Time.deltaTime;
        rotateTimer -= Time.deltaTime;

        if (targetTimer >= 0)
        {
            transform.position = Vector3.Lerp(transform.position, _cameraTarget.transform.position, cameraSpeed);
        }

        if (rotateTimer >= 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rot, 0), 0.15f);
        }
    }

    public void SetCameraTarget(GameObject target)
    {
        _cameraTarget = target;
        cameraSpeed = 0.15f;

        targetTimer = 0.5f;
    }

    public void SetCameraTracking(GameObject target)
    {
        _cameraTarget = target;
        cameraSpeed = 1f;

        targetTimer = 0.01f;
    }

    public void CameraRot(bool positive)
    {
        if (positive == true)
        {
            rot += 90;
        }
        else
        {
            rot -= 90;
        }

        rotateTimer = 0.5f;
    }

    public void CameraMove(bool up, bool down, bool left, bool right)
    {
        float cameraSpeed = 30;

        double sinX = Math.Sin(Mathf.Deg2Rad * (rot + 1));
        double cosZ = Math.Cos(Mathf.Deg2Rad * (rot + 1));

        int dirZ;
        int dirX;

        if (sinX < 0)
        {
            dirX = -1;
        }
        else
        {
            dirX = 1;
        }

        if (cosZ < 0)
        {
            dirZ = -1;
        }
        else
        {
            dirZ = 1;
        }

        if (up == true)
        {
            transform.position += Vector3.forward * Time.deltaTime * cameraSpeed * dirZ;
            transform.position += Vector3.right * Time.deltaTime * cameraSpeed * dirX;
        }

        if (down == true)
        {
            transform.position += Vector3.back * Time.deltaTime * cameraSpeed * dirZ;
            transform.position += Vector3.left * Time.deltaTime * cameraSpeed * dirX;
        }

        if (left == true)
        {
            transform.position += Vector3.forward * Time.deltaTime * cameraSpeed * dirX;
            transform.position += Vector3.left * Time.deltaTime * cameraSpeed * dirZ;
        }

        if (right == true)
        {
            transform.position += Vector3.back * Time.deltaTime * cameraSpeed * dirX;
            transform.position += Vector3.right * Time.deltaTime * cameraSpeed * dirZ;
        }
    }
}

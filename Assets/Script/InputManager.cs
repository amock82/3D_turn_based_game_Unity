using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class InputManager : MonoBehaviour
{
    private RaycastHit hit = new RaycastHit();
    private Ray ray;

    void Update()
    {
        if (Input.GetButtonDown("Tab"))
        {
            TargetManager.instance.NextTarget();
        }

        if (Input.GetButtonDown("Shift"))
        {
            TargetManager.instance.PreviousTarget();
        }

        if (Input.GetButtonDown("Q"))
        {
            CameraTarget.instance.CameraRot(true);
        }

        if (Input.GetButtonDown("E"))
        {
            CameraTarget.instance.CameraRot(false);
        }

        if (Input.GetButtonDown("Y"))
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Confined;           
            else
                Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Pawn")
                {
                    TargetManager.instance.ClickTarget(hit.transform.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    TargetManager.instance.TargetMove(hit.transform.gameObject);

                    //Debug.Log(hit.transform.gameObject);
                }
            }
        }
    }
}

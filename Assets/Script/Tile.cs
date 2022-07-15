using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isMoveable;
    public int useAction;

    GameObject _onTileObject;

    private void Start()
    {
        isMoveable = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Object" && _onTileObject == null)
        {
            isMoveable = false;
            _onTileObject = collision.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Pawn" && _onTileObject == null)
        {
            if (other.transform.tag == "Pawn")
            {
                other.gameObject.GetComponent<Pawn>().SetTile(this);
            }

            isMoveable = false;
            _onTileObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Pawn" && _onTileObject == other.gameObject)
        {
            isMoveable = true;
            _onTileObject = null;
        }
    }

    public void SetUseAction(int action)
    {
        useAction = action;
    }

    public int GetUseAction()
    {
        return useAction;
    }
}

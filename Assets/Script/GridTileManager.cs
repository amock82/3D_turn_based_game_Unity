using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTileManager : MonoBehaviour
{
    public float x;
    public float y;

    public static GridTileManager instance;

    private void Awake()
    {
        instance = this;

        x = this.GetComponent<RectTransform>().rect.width / this.GetComponent<GridLayoutGroup>().cellSize.x;
        y = this.GetComponent<RectTransform>().rect.height / this.GetComponent<GridLayoutGroup>().cellSize.y;

        Debug.Log(x);
        Debug.Log(y);
    }

}

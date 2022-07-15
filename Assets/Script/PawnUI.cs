using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnUI : MonoBehaviour
{
    Transform _hpBarTransform;
    Slider _hpBar;
    Vector3 lookPosition;

    GameObject _cameraTarget;

    Pawn _pawn;

    private void Awake()
    {
        _hpBarTransform = transform.Find("HpBar");
        _hpBar = transform.Find("HpBar").GetComponent<Slider>();

        _cameraTarget = GameObject.Find("CameraTarget");

        _pawn = transform.GetComponentInParent<Pawn>();

        Debug.Log(_pawn.gameObject.name);
    }

    void Start()
    {

    }

    void Update()
    {
        _hpBarTransform.rotation = Quaternion.Euler(45, 45 + _cameraTarget.transform.rotation.eulerAngles.y, 0);

        _hpBar.value = (float)_pawn.GetCurHp() / _pawn.GetMaxHp();
    }
}